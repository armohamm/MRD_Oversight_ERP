Public Class BuscaInsumosConPrecioDiferente

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public querystring As String = ""

    Public iprojectid As Integer = 0
    Public icardid As Integer = 0

    Public IsEdit As Boolean = False
    Public IsBase As Boolean = False
    Public IsModel As Boolean = False
    Public IsHistoric As Boolean = False

    Public iinputid As Integer = 0
    Public sinputdescription As String = ""

    Private openPermission As Boolean = False
    Private viewProjectModelPrice As Boolean = False
    Private viewBasePrice As Boolean = False

    Public messagesWindowIsAlreadyOpen As Boolean = False


    Private Sub checkMessages(ByVal username As String, ByVal x As Integer, ByVal y As Integer)

        Dim unreadmessagecount As Integer = 0
        unreadmessagecount = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM messages where susername = '" & username & "' AND bread = 0")

        If unreadmessagecount > 0 And messagesWindowIsAlreadyOpen = False Then

            Dim msg As New Mensajes
            Dim pt As Point

            msg.susername = username
            msg.bactive = bactive
            msg.bonline = bonline
            msg.suserfullname = suserfullname
            msg.suseremail = suseremail
            msg.susersession = susersession
            msg.susermachinename = susermachinename
            msg.suserip = suserip

            msg.StartPosition = FormStartPosition.Manual

            Dim tamañoPantalla As Integer = Screen.GetWorkingArea(Me).Height

            Dim tmpPt1 As Point = New Point(Me.Location.X, (tamañoPantalla - Me.Size.Height - msg.Size.Height) / 2) 'msg window
            Dim tmpPt2 As Point = New Point(Me.Location.X, tmpPt1.Y + msg.Size.Height) 'me

            If tmpPt1.Y > Screen.GetWorkingArea(Me).Location.Y Then

                pt = New Point(Me.Location.X, tmpPt1.Y)
                Me.Location = New Point(Me.Location.X, tmpPt2.Y)

            Else

                pt = New Point(x, y)

            End If

            msg.Location = pt
            msg.bAlreadyOpen = True

            messagesWindowIsAlreadyOpen = True

            msg.Show()

        End If

    End Sub


    Private Sub checkForKickoutsAndTimedOuts()

        Dim queryMySession As String = ""
        Dim dsMySession As DataSet

        queryMySession = "SELECT * FROM sessions s WHERE s.susername = '" & susername & "' AND s.susersession = '" & susersession & "' ORDER BY s.ilogindate DESC, s.slogintime DESC LIMIT 1 "

        dsMySession = getSQLQueryAsDataset(0, queryMySession)

        If dsMySession.Tables(0).Rows.Count > 0 Then

            If dsMySession.Tables(0).Rows(0).Item("btimedout") = "1" Then

                MsgBox("Tu sesión ha expirado. Es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Sesión expirada")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

            If dsMySession.Tables(0).Rows(0).Item("bkickedout") = "1" Then

                MsgBox("Has sido sacado del sistema. Para continuar es necesario que entres de nuevo al sistema con tu usuario y contraseña", MsgBoxStyle.Critical, "Logged out")

                susername = ""
                bactive = False
                bonline = False
                suserfullname = ""
                suseremail = ""
                susersession = 0
                susermachinename = ""
                suserip = "0.0.0.0"

                Dim l As New Login

                l.isEdit = True

                l.ShowDialog(Me)

                If l.DialogResult <> Windows.Forms.DialogResult.OK Then

                    MsgBox("Cerrando Aplicación SIN Guardar...", MsgBoxStyle.Critical, "Intento Fallido")
                    System.Environment.Exit(0)

                End If

            End If

        End If

    End Sub


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        Dim viewPermission As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Nuevo" Then
                    'btnCrear.Enabled = True
                End If

                If permission = "Modificar" Then
                    openPermission = True
                    'btnAbrir.Enabled = True
                End If

                If permission = "Ver Precio ProyectoModelo" Then
                    viewProjectModelPrice = True
                End If

                If permission = "Ver Precio Base" Then
                    viewBasePrice = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Insumos con Precios diferentes', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub BuscaInsumosConPrecioDiferente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Insumos con Precios diferentes con " & dgvInsumosConPrecioDiferente.Rows.Count & " Insumos sin Actualizar', 'OK')")

    End Sub


    Private Sub BuscaInsumosConPrecioDiferente_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.F5 Then

            If My.Computer.Info.OSFullName.StartsWith("Microsoft Windows 7") = True Then
                NotifyIcon1.Icon = Oversight.My.Resources.winmineVista16x16
            Else
                NotifyIcon1.Icon = Oversight.My.Resources.winmineXP16x16
            End If

            NotifyIcon1.Text = "Buscaminas"

            NotifyIcon1.Visible = True

            Me.Visible = False
            Do While Not fDone
                System.Windows.Forms.Application.DoEvents()
            Loop
            fDone = False
            Me.Visible = True

        End If

    End Sub


    Private Sub BuscaInsumosConPrecioDiferente_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim queryBusqueda As String = ""

        If IsModel = True Then

            queryBusqueda = "" & _
            "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
            "IF(mp.dinputfinalprice IS NULL, 0, mp.dinputfinalprice) AS 'Precio en el Modelo', " & _
            "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
            "FROM inputs i " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
            "WHERE " & _
            "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
            "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
            "AND mp.imodelid = " & iprojectid & " " & _
            "ORDER BY i.sinputdescription "

            lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE MODELO. "

        Else

            queryBusqueda = "" & _
            "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
            "IF(pp.dinputfinalprice IS NULL, 0, pp.dinputfinalprice) AS 'Precio en el Proyecto', " & _
            "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
            "FROM inputs i " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
            "WHERE " & _
            "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
            "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
            "AND pp.iprojectid = " & iprojectid & " " & _
            "ORDER BY i.sinputdescription "

            lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE PROYECTO. "

        End If

        setDataGridView(dgvInsumosConPrecioDiferente, queryBusqueda, True)

        dgvInsumosConPrecioDiferente.Columns(0).Visible = False

        If viewProjectModelPrice = False Then
            dgvInsumosConPrecioDiferente.Columns(3).Visible = False
        End If

        If viewBasePrice = False Then
            dgvInsumosConPrecioDiferente.Columns(4).Visible = False
        End If

        Dim count As Integer = 0

        If IsModel = True Then

            count = getSQLQueryAsInteger(0, "" & _
            "SELECT COUNT(*) " & _
            "FROM inputs i " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
            "WHERE " & _
            "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
            "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
            "AND mp.imodelid = " & iprojectid & " " & _
            "ORDER BY i.sinputdescription ")

        Else

            count = getSQLQueryAsInteger(0, "" & _
            "SELECT COUNT(*) " & _
            "FROM inputs i " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
            "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
            "WHERE " & _
            "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
            "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
            "AND pp.iprojectid = " & iprojectid & " " & _
            "ORDER BY i.sinputdescription ")

        End If

        If count = 0 Then

            btnIgnorar.Text = "Cerrar Ventana y Continuar"

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Insumos con Precios diferentes', 'OK')")

        dgvInsumosConPrecioDiferente.Select()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Dim n As New Loader

        n.isEdit = True

        n.ShowDialog()

        If n.DialogResult = Windows.Forms.DialogResult.OK Then

            fDone = True

        End If

    End Sub


    Private Sub dgvInsumosConPrecioDiferente_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumosConPrecioDiferente.CellClick

        Try

            If dgvInsumosConPrecioDiferente.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(1).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""

        End Try

    End Sub


    Private Sub dgvInsumosConPrecioDiferente_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumosConPrecioDiferente.CellContentClick

        Try

            If dgvInsumosConPrecioDiferente.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(1).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""

        End Try

    End Sub


    Private Sub dgvInsumosConPrecioDiferente_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInsumosConPrecioDiferente.SelectionChanged

        Try

            If dgvInsumosConPrecioDiferente.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumosConPrecioDiferente.CurrentRow.Cells(0).Value)
            sinputdescription = dgvInsumosConPrecioDiferente.CurrentRow.Cells(1).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""

        End Try

    End Sub


    Private Sub dgvInsumosConPrecioDiferente_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumosConPrecioDiferente.CellContentDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvInsumosConPrecioDiferente.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(1).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""

        End Try

        If dgvInsumosConPrecioDiferente.SelectedRows.Count = 1 Then

            Dim conteoCompound As Integer = 0

            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iinputid)

            If conteoCompound > 0 Then

                'Es un material compuesto

                MsgBox("Si quieres editar los RENDIMIENTOS de este Insumo Compuesto, te sugiero que vayas a Presupuestos Base," & Chr(13) & "encuentres la tarjeta que contiene este insumo y edites desde ahí el Insumo, de manera que no afectes a" & Chr(13) & "las demás tarjetas que contienen este Insumo." & Chr(13) & "Si lo que deseas es cambiar los PRECIOS de los insumos que componen este Insumo Compuesto," & Chr(13) & "busca el insumo que cambió de precio, no éste Insumo Compuesto." & Chr(13) & "Si deseas cambiar un insumo del Insumo Compuesto, te sugiero que NO lo hagas, ya que" & Chr(13) & "podrías afectar a otros proyectos/tarjetas que usen este Insumo Compuesto. Mejor crea un Nuevo Insumo" & Chr(13) & "con los nuevos insumos y rendimientos que deseas.", MsgBoxStyle.OkOnly, "Sugerencias")
                Exit Sub

                'Dim aic As New AgregarInsumoCompuesto

                'aic.susername = susername
                'aic.bactive = bactive
                'aic.bonline = bonline
                'aic.suserfullname = suserfullname
                'aic.suseremail = suseremail
                'aic.susersession = susersession
                'aic.susermachinename = susermachinename
                'aic.suserip = suserip

                'aic.iprojectid = iprojectid
                'aic.icardid = icardid
                'aic.iinputid = iinputid

                'aic.IsHistoric = IsHistoric

                'aic.IsBase = IsBase
                'aic.IsModel = IsModel

                'aic.IsEdit = True

                'If Me.WindowState = FormWindowState.Maximized Then
                '    aic.WindowState = FormWindowState.Maximized
                'End If

                'Me.Visible = False
                'aic.ShowDialog(Me)
                'Me.Visible = True

                'If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                '    Dim queryBusqueda As String = ""

                '    If IsModel = True Then

                '        queryBusqueda = "" & _
                '        "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                '        "IF(mp.dinputfinalprice IS NULL, 0, mp.dinputfinalprice) AS 'Precio en el Modelo', " & _
                '        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                '        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND mp.imodelid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription "

                '        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE MODELO. "

                '    Else

                '        queryBusqueda = "" & _
                '        "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                '        "IF(pp.dinputfinalprice IS NULL, 0, pp.dinputfinalprice) AS 'Precio en el Proyecto', " & _
                '        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                '        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND pp.iprojectid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription "

                '        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE PROYECTO. "

                '    End If

                '    setDataGridView(dgvInsumosConPrecioDiferente, queryBusqueda, True)

                '    dgvInsumosConPrecioDiferente.Columns(0).Visible = False

                '    Dim count As Integer = 0

                '    If IsModel = True Then

                '        count = getSQLQueryAsInteger(0, "" & _
                '        "SELECT COUNT(*) " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                '        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND mp.imodelid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription ")

                '    Else

                '        count = getSQLQueryAsInteger(0, "" & _
                '        "SELECT COUNT(*) " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                '        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND pp.iprojectid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription ")

                '    End If

                '    If count = 0 Then

                '        btnIgnorar.Text = "Cerrar Ventana y Continuar"

                '    End If

                'End If

            Else

                'NO Es un material compuesto
                Dim ai As New AgregarInsumo

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iprojectid = iprojectid
                ai.icardid = icardid
                ai.iinputid = iinputid

                ai.IsHistoric = IsHistoric

                ai.IsBase = IsBase
                ai.IsModel = IsModel

                ai.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True

                If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim queryBusqueda As String = ""

                    If IsModel = True Then

                        queryBusqueda = "" & _
                        "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                        "IF(mp.dinputfinalprice IS NULL, 0, mp.dinputfinalprice) AS 'Precio en el Modelo', " & _
                        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND mp.imodelid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription "

                        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE MODELO. "

                    Else

                        queryBusqueda = "" & _
                        "SELECT i.iinputid AS 'ID', i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                        "IF(pp.dinputfinalprice IS NULL, 0, pp.dinputfinalprice) AS 'Precio en el Proyecto', " & _
                        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND pp.iprojectid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription "

                        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE PROYECTO. "

                    End If

                    setDataGridView(dgvInsumosConPrecioDiferente, queryBusqueda, True)

                    dgvInsumosConPrecioDiferente.Columns(0).Visible = False

                    If viewProjectModelPrice = False Then
                        dgvInsumosConPrecioDiferente.Columns(3).Visible = False
                    End If

                    If viewBasePrice = False Then
                        dgvInsumosConPrecioDiferente.Columns(4).Visible = False
                    End If

                    Dim count As Integer = 0

                    If IsModel = True Then

                        count = getSQLQueryAsInteger(0, "" & _
                        "SELECT COUNT(*) " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND mp.imodelid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription ")

                    Else

                        count = getSQLQueryAsInteger(0, "" & _
                        "SELECT COUNT(*) " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND pp.iprojectid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription ")

                    End If

                    If count = 0 Then

                        btnIgnorar.Text = "Cerrar Ventana y Continuar"

                    End If

                End If

            End If

        Else

            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInsumosConPrecioDiferente_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInsumosConPrecioDiferente.CellDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvInsumosConPrecioDiferente.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iinputid = CInt(dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(0).Value)
            sinputdescription = dgvInsumosConPrecioDiferente.Rows(e.RowIndex).Cells(1).Value

        Catch ex As Exception

            iinputid = 0
            sinputdescription = ""

        End Try

        If dgvInsumosConPrecioDiferente.SelectedRows.Count = 1 Then

            Dim conteoCompound As Integer = 0

            conteoCompound = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & iprojectid & "CardCompoundInputs WHERE iinputid = " & iinputid)

            If conteoCompound > 0 Then

                'Es un material compuesto

                MsgBox("Si quieres editar los RENDIMIENTOS de este Insumo Compuesto, te sugiero que vayas a Presupuestos Base," & Chr(13) & "encuentres la tarjeta que contiene este insumo y edites desde ahí el Insumo, de manera que no afectes a" & Chr(13) & "las demás tarjetas que contienen este Insumo." & Chr(13) & "Si lo que deseas es cambiar los PRECIOS de los insumos que componen este Insumo Compuesto," & Chr(13) & "busca el insumo que cambió de precio, no éste Insumo Compuesto." & Chr(13) & "Si deseas cambiar un insumo del Insumo Compuesto, te sugiero que NO lo hagas, ya que" & Chr(13) & "podrías afectar a otros proyectos/tarjetas que usen este Insumo Compuesto. Mejor crea un Nuevo Insumo" & Chr(13) & "con los nuevos insumos y rendimientos que deseas.", MsgBoxStyle.OkOnly, "Sugerencias")
                Exit Sub

                'Dim aic As New AgregarInsumoCompuesto

                'aic.susername = susername
                'aic.bactive = bactive
                'aic.bonline = bonline
                'aic.suserfullname = suserfullname
                'aic.suseremail = suseremail
                'aic.susersession = susersession
                'aic.susermachinename = susermachinename
                'aic.suserip = suserip

                'aic.iprojectid = iprojectid
                'aic.icardid = icardid
                'aic.iinputid = iinputid

                'aic.IsHistoric = IsHistoric

                'aic.IsBase = IsBase
                'aic.IsModel = IsModel

                'aic.IsEdit = True

                'If Me.WindowState = FormWindowState.Maximized Then
                '    aic.WindowState = FormWindowState.Maximized
                'End If

                'Me.Visible = False
                'aic.ShowDialog(Me)
                'Me.Visible = True

                'If aic.DialogResult = Windows.Forms.DialogResult.OK Then

                '    Dim queryBusqueda As String = ""

                '    If IsModel = True Then

                '        queryBusqueda = "" & _
                '        "SELECT i.iinputid, i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                '        "IF(mp.dinputfinalprice IS NULL, 0, mp.dinputfinalprice) AS 'Precio en el Modelo', " & _
                '        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                '        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND mp.imodelid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription "

                '        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE MODELO. "

                '    Else

                '        queryBusqueda = "" & _
                '        "SELECT i.iinputid, i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                '        "IF(pp.dinputfinalprice IS NULL, 0, pp.dinputfinalprice) AS 'Precio en el Proyecto', " & _
                '        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                '        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND pp.iprojectid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription "

                '        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE PROYECTO. "

                '    End If

                '    setDataGridView(dgvInsumosConPrecioDiferente, queryBusqueda, True)

                '    Dim count As Integer = 0

                '    If IsModel = True Then

                '        count = getSQLQueryAsInteger(0, "" & _
                '        "SELECT COUNT(*) " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                '        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND mp.imodelid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription ")

                '    Else

                '        count = getSQLQueryAsInteger(0, "" & _
                '        "SELECT COUNT(*) " & _
                '        "FROM inputs i " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                '        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                '        "WHERE " & _
                '        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                '        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                '        "AND pp.iprojectid = " & iprojectid & " " & _
                '        "ORDER BY i.sinputdescription ")

                '    End If

                '    If count = 0 Then

                '        btnIgnorar.Text = "Cerrar Ventana y Continuar"

                '    End If

                'End If

            Else

                'NO Es un material compuesto
                Dim ai As New AgregarInsumo

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iprojectid = iprojectid
                ai.icardid = icardid
                ai.iinputid = iinputid

                ai.IsHistoric = IsHistoric

                ai.IsBase = IsBase
                ai.IsModel = IsModel

                ai.IsEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True

                If ai.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim queryBusqueda As String = ""

                    If IsModel = True Then

                        queryBusqueda = "" & _
                        "SELECT i.iinputid, i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                        "IF(mp.dinputfinalprice IS NULL, 0, mp.dinputfinalprice) AS 'Precio en el Modelo', " & _
                        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND mp.imodelid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription "

                        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE MODELO. "

                    Else

                        queryBusqueda = "" & _
                        "SELECT i.iinputid, i.sinputdescription AS 'Material', i.sinputunit AS 'Unidad', " & _
                        "IF(pp.dinputfinalprice IS NULL, 0, pp.dinputfinalprice) AS 'Precio en el Proyecto', " & _
                        "IF(bp.dinputfinalprice IS NULL, 0, bp.dinputfinalprice) AS 'Precio Base' " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND pp.iprojectid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription "

                        lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE PROYECTO. "

                    End If

                    setDataGridView(dgvInsumosConPrecioDiferente, queryBusqueda, True)

                    dgvInsumosConPrecioDiferente.Columns(0).Visible = False

                    If viewProjectModelPrice = False Then
                        dgvInsumosConPrecioDiferente.Columns(3).Visible = False
                    End If

                    If viewBasePrice = False Then
                        dgvInsumosConPrecioDiferente.Columns(4).Visible = False
                    End If

                    Dim count As Integer = 0

                    If IsModel = True Then

                        count = getSQLQueryAsInteger(0, "" & _
                        "SELECT COUNT(*) " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, iprojectid) mp ON i.iinputid = mp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(mp.iupdatedate, ' ', mp.supdatetime), '%Y%c%d %T') " & _
                        "AND mp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND mp.imodelid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription ")

                    Else

                        count = getSQLQueryAsInteger(0, "" & _
                        "SELECT COUNT(*) " & _
                        "FROM inputs i " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Project" & iprojectid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON i.iinputid = pp.iinputid " & _
                        "LEFT JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON i.iinputid = bp.iinputid " & _
                        "WHERE " & _
                        "STR_TO_DATE(CONCAT(bp.iupdatedate, ' ', bp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') " & _
                        "AND pp.dinputfinalprice <> bp.dinputfinalprice " & _
                        "AND pp.iprojectid = " & iprojectid & " " & _
                        "ORDER BY i.sinputdescription ")

                    End If

                    If count = 0 Then

                        btnIgnorar.Text = "Cerrar Ventana y Continuar"

                    End If

                End If

            End If

        Else

            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnIgnorar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIgnorar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If dgvInsumosConPrecioDiferente.RowCount > 0 Then

            If MsgBox("¿Estás seguro de que no deseas actualizar los precios de los Insumos que te faltan en la lista?", MsgBoxStyle.YesNo, "Confirmación para Ignorar Precios Nuevos") = MsgBoxResult.Yes Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub



End Class