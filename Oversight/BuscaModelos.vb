Public Class BuscaModelos

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public selected As Boolean = False
    Public querystring As String = ""

    Public imodelid As Integer = 0
    Public smodelname As String = ""

    Public isEdit As Boolean = False

    Public wasCreated As Boolean = False

    Private isFormReadyForAction As Boolean = False

    Private openPermission As Boolean = False
    Private viewDgvModelPrice As Boolean = False
    Private viewDgvModelCost As Boolean = False

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
                    btnAbrir.Enabled = True
                End If

                If permission = "Ver Precio" Then
                    viewDgvModelPrice = True
                End If

                If permission = "Ver Costo" Then
                    viewDgvModelCost = True
                End If

                If permission = "Exportar" Then
                    btnExportar.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Modelos', 'OK')")

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


    Private Sub BuscaModelos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Modelo con el modelo " & imodelid & " : " & smodelname & " seleccionado', 'OK')")

    End Sub


    Private Sub BuscaModelos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaModelos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnAbrir
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        txtBuscar.Text = querystring

        Dim queryBusqueda As String = ""

        queryBusqueda = "" & _
        "SELECT m.imodelid AS 'ID', STR_TO_DATE(CONCAT(m.iupdatedate, ' ', m.supdatetime), '%Y%c%d %T') AS 'Fecha de Ultima Mod', m.smodelname AS 'Nombre del Modelo', m.dmodellength AS 'Medida Largo', m.dmodelwidth AS 'Medida Ancho', " & _
        "FORMAT(SUM(mtf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+mtf.dcardindirectcostspercentage)*(1+mtf.dcardgainpercentage)*(1+m.dmodelIVApercentage))), 2) AS 'Precio Ofrecido al Cliente', " & _
        "FORMAT(SUM(mtf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+mtf.dcardindirectcostspercentage))), 2) AS 'Costo Esperado de la Obra' " & _
        "FROM models m " & _
        "JOIN modelcards mtf ON mtf.imodelid = m.imodelid " & _
        "JOIN cardlegacycategories mtflc ON mtflc.scardlegacycategoryid = mtf.scardlegacycategoryid " & _
        "JOIN (SELECT mtfi.imodelid, mtfi.icardid, mtfi.iinputid, (costoMO.costo*mtfi.dcardinputqty) AS costo FROM modelcardinputs mtfi JOIN modelcards mtf ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid JOIN (SELECT mtfi.imodelid, mtfi.icardid AS icardid, 0 AS iinputid, SUM(mtfi.dcardinputqty*mp.dinputfinalprice) AS costo FROM modelcards mtf LEFT JOIN modelcardinputs mtfi ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid LEFT JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtf.icardid, mtfi.imodelid ) AS costoMO ON mtfi.iinputid = costoMO.iinputid AND mtfi.icardid = costoMO.icardid GROUP BY mtfi.icardid, mtfi.imodelid) AS costoEQ ON mtf.imodelid = costoEQ.imodelid AND mtf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT mtfi.imodelid, mtfi.icardid, mtfi.iinputid, SUM(mtfi.dcardinputqty*mp.dinputfinalprice) AS costo FROM modelcards mtf LEFT JOIN modelcardinputs mtfi ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid LEFT JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtf.icardid, mtfi.imodelid) AS costoMO ON mtf.imodelid = costoMO.imodelid AND mtf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT mtfi.imodelid, mtfi.icardid, IF(SUM(mtfi.dcardinputqty*mp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*mp.dinputfinalprice))+IF(SUM(mtfi.dcardinputqty*cimp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*cimp.dinputfinalprice)) AS costo FROM modelcards mtf LEFT JOIN modelcardinputs mtfi ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid LEFT JOIN inputs i ON mtfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid LEFT JOIN (SELECT mtfi.imodelid, mtfi.icardid, mtfi.iinputid, cimp.iupdatedate, cimp.supdatetime, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputfinalprice FROM modelcardinputs mtfi JOIN modelcardcompoundinputs mtfci ON mtfci.imodelid = mtfi.imodelid AND mtfci.icardid = mtfi.icardid AND mtfci.iinputid = mtfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) cimp GROUP BY iinputid, imodelid) cimp ON cimp.imodelid = mtfci.imodelid AND cimp.iinputid = mtfci.icompoundinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfi.iinputid) cimp ON mtfi.imodelid = cimp.imodelid AND mtfi.icardid = cimp.icardid AND i.iinputid = cimp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY mtfi.imodelid, mtfi.icardid ORDER BY mtfi.imodelid, mtfi.icardid, mtfi.iinputid) AS costoMAT ON mtf.imodelid = costoMAT.imodelid AND mtf.icardid = costoMAT.icardid " & _
        "WHERE " & _
        "CONCAT(STR_TO_DATE(CONCAT(m.iupdatedate, ' ', m.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
        "m.smodelname LIKE '%" & querystring & "%' OR " & _
        "m.smodeltype LIKE '%" & querystring & "%' OR " & _
        "m.dmodellength LIKE '%" & querystring & "%' OR " & _
        "m.dmodelwidth LIKE '%" & querystring & "%' OR " & _
        "m.smodelfileslocation LIKE '%" & querystring & "%' OR " & _
        "m.dmodelIVApercentage LIKE '%" & querystring & "%' OR " & _
        "mtf.scardlegacycategoryid LIKE '%" & querystring & "%' OR " & _
        "mtf.scarddescription LIKE '%" & querystring & "%' " & _
        "GROUP BY 1"

        setDataGridView(dgvModelos, queryBusqueda, True)

        dgvModelos.Columns(0).Visible = False

        If viewDgvModelPrice = False Then
            dgvModelos.Columns(5).Visible = False
        End If

        If viewDgvModelCost = False Then
            dgvModelos.Columns(6).Visible = False
        End If

        dgvModelos.Columns(0).Width = 20
        dgvModelos.Columns(1).Width = 100
        dgvModelos.Columns(2).Width = 150
        dgvModelos.Columns(3).Width = 60
        dgvModelos.Columns(4).Width = 60
        dgvModelos.Columns(5).Width = 70
        dgvModelos.Columns(6).Width = 70

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Modelos', 'OK')")

        dgvModelos.Select()

        txtBuscar.Select()
        txtBuscar.Focus()
        txtBuscar.SelectionStart() = txtBuscar.Text.Length

        isFormReadyForAction = True

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


    Private Sub txtBuscar_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBuscar.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtBuscar.Text.Contains(arrayCaractProhib(carp)) Then
                txtBuscar.Text = txtBuscar.Text.Replace(arrayCaractProhib(carp), "")
            End If

        Next carp

    End Sub


    Private Sub dgvModelos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvModelos.CellClick

        Try

            If dgvModelos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            If e.RowIndex = -1 Then
                Exit Sub
            End If

            imodelid = CInt(dgvModelos.Rows(e.RowIndex).Cells(0).Value)
            smodelname = dgvModelos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvModelos_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvModelos.CellContentClick

        Try

            If dgvModelos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            If e.RowIndex = -1 Then
                Exit Sub
            End If

            imodelid = CInt(dgvModelos.Rows(e.RowIndex).Cells(0).Value)
            smodelname = dgvModelos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvModelos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvModelos.SelectionChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Try

            If dgvModelos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            imodelid = CInt(dgvModelos.CurrentRow.Cells(0).Value)
            smodelname = dgvModelos.CurrentRow.Cells(2).Value

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvModelos_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvModelos.CellContentDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvModelos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            imodelid = CInt(dgvModelos.Rows(e.RowIndex).Cells(0).Value)
            smodelname = dgvModelos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try

        If dgvModelos.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim m As New AgregarModelo

            m.susername = susername
            m.bactive = bactive
            m.bonline = bonline
            m.suserfullname = suserfullname
            m.suseremail = suseremail
            m.susersession = susersession
            m.susermachinename = susermachinename
            m.suserip = suserip

            m.imodelid = imodelid

            m.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                m.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            m.ShowDialog(Me)
            Me.Visible = True

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvModelos_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvModelos.CellDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvModelos.CurrentRow Is Nothing Then
                Exit Sub
            End If

            imodelid = CInt(dgvModelos.Rows(e.RowIndex).Cells(0).Value)
            smodelname = dgvModelos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try

        If dgvModelos.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim m As New AgregarModelo

            m.susername = susername
            m.bactive = bactive
            m.bonline = bonline
            m.suserfullname = suserfullname
            m.suseremail = suseremail
            m.susersession = susersession
            m.susermachinename = susermachinename
            m.suserip = suserip

            m.imodelid = imodelid

            m.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                m.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            m.ShowDialog(Me)
            Me.Visible = True

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        querystring = ""

        imodelid = 0
        smodelname = ""
        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If imodelid <> 0 And isEdit = True Then

            Dim m As New AgregarModelo

            m.susername = susername
            m.bactive = bactive
            m.bonline = bonline
            m.suserfullname = suserfullname
            m.suseremail = suseremail
            m.susersession = susersession
            m.susermachinename = susermachinename
            m.suserip = suserip

            m.imodelid = imodelid

            m.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                m.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            m.ShowDialog(Me)
            Me.Visible = True

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        ElseIf imodelid <> 0 And isEdit = False Then

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtBuscar.Text = txtBuscar.Text.Replace("--", "").Replace("'", "")

        querystring = txtBuscar.Text

        Dim queryBusqueda As String = ""

        queryBusqueda = "" & _
        "SELECT m.imodelid AS 'ID', STR_TO_DATE(CONCAT(m.iupdatedate, ' ', m.supdatetime), '%Y%c%d %T') AS 'Fecha de Ultima Mod', m.smodelname AS 'Nombre del Modelo', m.dmodellength AS 'Medida Largo', m.dmodelwidth AS 'Medida Ancho', " & _
        "FORMAT(SUM(mtf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+mtf.dcardindirectcostspercentage)*(1+mtf.dcardgainpercentage)*(1+m.dmodelIVApercentage))), 2) AS 'Precio Ofrecido al Cliente', " & _
        "FORMAT(SUM(mtf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+mtf.dcardindirectcostspercentage))), 2) AS 'Costo Esperado de la Obra' " & _
        "FROM models m " & _
        "JOIN modelcards mtf ON mtf.imodelid = m.imodelid " & _
        "JOIN cardlegacycategories mtflc ON mtflc.scardlegacycategoryid = mtf.scardlegacycategoryid " & _
        "JOIN (SELECT mtfi.imodelid, mtfi.icardid, mtfi.iinputid, (costoMO.costo*mtfi.dcardinputqty) AS costo FROM modelcardinputs mtfi JOIN modelcards mtf ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid JOIN (SELECT mtfi.imodelid, mtfi.icardid AS icardid, 0 AS iinputid, SUM(mtfi.dcardinputqty*mp.dinputfinalprice) AS costo FROM modelcards mtf LEFT JOIN modelcardinputs mtfi ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid LEFT JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtf.icardid, mtfi.imodelid ) AS costoMO ON mtfi.iinputid = costoMO.iinputid AND mtfi.icardid = costoMO.icardid GROUP BY mtfi.icardid, mtfi.imodelid) AS costoEQ ON mtf.imodelid = costoEQ.imodelid AND mtf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT mtfi.imodelid, mtfi.icardid, mtfi.iinputid, SUM(mtfi.dcardinputqty*mp.dinputfinalprice) AS costo FROM modelcards mtf LEFT JOIN modelcardinputs mtfi ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid LEFT JOIN inputs i ON mtfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY mtf.icardid, mtfi.imodelid) AS costoMO ON mtf.imodelid = costoMO.imodelid AND mtf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT mtfi.imodelid, mtfi.icardid, IF(SUM(mtfi.dcardinputqty*mp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*mp.dinputfinalprice))+IF(SUM(mtfi.dcardinputqty*cimp.dinputfinalprice) IS NULL, 0, SUM(mtfi.dcardinputqty*cimp.dinputfinalprice)) AS costo FROM modelcards mtf LEFT JOIN modelcardinputs mtfi ON mtf.imodelid = mtfi.imodelid AND mtf.icardid = mtfi.icardid LEFT JOIN inputs i ON mtfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) mp GROUP BY iinputid, imodelid) mp ON mtfi.imodelid = mp.imodelid AND i.iinputid = mp.iinputid LEFT JOIN (SELECT mtfi.imodelid, mtfi.icardid, mtfi.iinputid, cimp.iupdatedate, cimp.supdatetime, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(mtfci.dcompoundinputqty*cimp.dinputfinalprice) AS dinputfinalprice FROM modelcardinputs mtfi JOIN modelcardcompoundinputs mtfci ON mtfci.imodelid = mtfi.imodelid AND mtfci.icardid = mtfi.icardid AND mtfci.iinputid = mtfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM modelprices ORDER BY iupdatedate DESC, supdatetime DESC) cimp GROUP BY iinputid, imodelid) cimp ON cimp.imodelid = mtfci.imodelid AND cimp.iinputid = mtfci.icompoundinputid GROUP BY mtfci.imodelid, mtfci.icardid, mtfi.iinputid) cimp ON mtfi.imodelid = cimp.imodelid AND mtfi.icardid = cimp.icardid AND i.iinputid = cimp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY mtfi.imodelid, mtfi.icardid ORDER BY mtfi.imodelid, mtfi.icardid, mtfi.iinputid) AS costoMAT ON mtf.imodelid = costoMAT.imodelid AND mtf.icardid = costoMAT.icardid " & _
        "WHERE " & _
        "CONCAT(STR_TO_DATE(CONCAT(m.iupdatedate, ' ', m.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
        "m.smodelname LIKE '%" & querystring & "%' OR " & _
        "m.smodeltype LIKE '%" & querystring & "%' OR " & _
        "m.dmodellength LIKE '%" & querystring & "%' OR " & _
        "m.dmodelwidth LIKE '%" & querystring & "%' OR " & _
        "m.smodelfileslocation LIKE '%" & querystring & "%' OR " & _
        "m.dmodelIVApercentage LIKE '%" & querystring & "%' OR " & _
        "mtf.scardlegacycategoryid LIKE '%" & querystring & "%' OR " & _
        "mtf.scarddescription LIKE '%" & querystring & "%' " & _
        "GROUP BY 1"

        setDataGridView(dgvModelos, queryBusqueda, True)

        dgvModelos.Columns(0).Visible = False

        If viewDgvModelPrice = False Then
            dgvModelos.Columns(5).Visible = False
        End If

        If viewDgvModelCost = False Then
            dgvModelos.Columns(6).Visible = False
        End If

        dgvModelos.Columns(0).Width = 20
        dgvModelos.Columns(1).Width = 100
        dgvModelos.Columns(2).Width = 150
        dgvModelos.Columns(3).Width = 60
        dgvModelos.Columns(4).Width = 60
        dgvModelos.Columns(5).Width = 70
        dgvModelos.Columns(6).Width = 70

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux



            msSaveFileDialog.FileName = "Modelos " & fecha
            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Modelos Exportados Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar los Modelos. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar los Modelos")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the project header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""137.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""147""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""62.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""58.5""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""119.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""126.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""147.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""72""/>")
            fs.WriteLine("   <Column ss:Width=""126""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""5"" ss:StyleID=""1""><Data ss:Type=""String"">MODELOS</Data></Cell>")
            fs.WriteLine("   </Row>")


            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next

            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""22.5"">")

            For Each col As DataGridViewColumn In dgvModelos.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvModelos.Rows

                If dgvModelos.AllowUserToAddRows = True And row.Index = dgvModelos.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvModelos.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))

                        Else

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

            'Write the separation between results and totals
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            'Write the totals 
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


End Class