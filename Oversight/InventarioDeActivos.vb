Public Class InventarioDeActivos

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public isEdit As Boolean = False

    Private isFormReadyForAction As Boolean = False
    Private isDgvInventarioReady As Boolean = False

    Public querystring As String = ""

    Private iselectedassetid As Integer = 0
    Private dselectedassetqty As Double = 1.0
    Private sselectedassetdescription As String = ""

    Private WithEvents txtCantidadDgvInventario As TextBox
    Private WithEvents txtNombreDgvInventario As TextBox
    Private WithEvents txtLugarDgvInventario As TextBox

    Private txtCantidadDgvInventario_OldText As String = ""
    Private txtNombreDgvInventario_OldText As String = ""
    Private txtLugarDgvInventario_OldText As String = ""

    Private viewPermission As Boolean = False

    Private addAsset As Boolean = False
    Private openAsset As Boolean = False
    Private modifyAssetQty As Boolean = False
    Private deleteAsset As Boolean = False

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

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    'btnGuardar.Enabled = True
                    'btnGuardarYCerrar.Enabled = True
                End If

                If permission = "Ver Inventario" Then
                    dgvInventario.Visible = True
                End If

                If permission = "Agregar Activo" Then
                    addAsset = True
                    dgvInventario.Visible = True
                    btnNuevoActivo.Enabled = True
                End If

                If permission = "Modificar Activo" Then
                    openAsset = True
                    modifyAssetQty = True
                    dgvInventario.Visible = True
                    btnInsertarActivo.Enabled = True
                End If

                If permission = "Eliminar Activo" Then
                    deleteAsset = True
                    dgvInventario.Visible = True
                    btnEliminarActivo.Enabled = True
                End If

                If permission = "Generar Hoja de Confirmacion" Then
                    btnGenerarHojaConfirmacion.Enabled = True
                End If

                If permission = "Exportar Inventario" Then
                    btnExportarExcel.Enabled = True
                End If

                If permission = "Rehacer" Then
                    btnRehacerInventarioMostrado.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Revisar Inventario', 'OK')")

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


    Private Sub Inventario_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Inventario de Activos', 'OK')")

    End Sub


    Private Sub InventarioDeActivos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub Inventario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        isFormReadyForAction = False

        Dim query1 As String

        querystring = ""

        query1 = "" & _
        "SELECT * FROM ( " & _
        "SELECT * FROM ( " & _
        "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
        "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
        "pa.sassetstatus AS 'Status' " & _
        "FROM assetsphysicalinventory pa " & _
        "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
        "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
        "ORDER BY 6 DESC, 7 DESC " & _
        ") pa " & _
        "GROUP BY iassetid " & _
        ") pa " & _
        "WHERE Cantidad > 0 " & _
        "ORDER BY 2 "

        setDataGridView(dgvInventario, query1, False)

        dgvInventario.Columns(0).Visible = False

        dgvInventario.Columns(0).ReadOnly = True
        dgvInventario.Columns(1).ReadOnly = True
        dgvInventario.Columns(3).ReadOnly = True
        dgvInventario.Columns(5).ReadOnly = True

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Inventario de Activos', 'OK')")

        dgvInventario.Select()

        txtBuscar.Focus()

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

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtBuscar.Text.Contains(arrayCaractProhib(carp)) Then
                txtBuscar.Text = txtBuscar.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtBuscar.Select(txtBuscar.Text.Length, 0)
        End If

        txtBuscar.Text = txtBuscar.Text.Replace("--", "").Replace("'", "")
        txtBuscar.Text = txtBuscar.Text.Trim

    End Sub


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = txtBuscar.Text

        Dim query1 As String

        query1 = "" & _
        "SELECT * FROM ( " & _
        "SELECT * FROM ( " & _
        "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
        "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
        "pa.sassetstatus AS 'Status' " & _
        "FROM assetsphysicalinventory pa " & _
        "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
        "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
        "ORDER BY 6 DESC, 7 DESC " & _
        ") pa " & _
        "GROUP BY iassetid " & _
        ") pa " & _
        "WHERE Cantidad > 0 " & _
        "ORDER BY 2 "

        setDataGridView(dgvInventario, query1, False)

        dgvInventario.Columns(0).Visible = False

        dgvInventario.Columns(0).ReadOnly = True
        dgvInventario.Columns(1).ReadOnly = True
        dgvInventario.Columns(3).ReadOnly = True
        dgvInventario.Columns(5).ReadOnly = True

        txtBuscar.Focus()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInventario_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInventario.CellClick

        If dgvInventario.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        Try
            iselectedassetid = CInt(dgvInventario.Rows(e.RowIndex).Cells(0).Value)
            sselectedassetdescription = dgvInventario.Rows(e.RowIndex).Cells(1).Value
            dselectedassetqty = CDbl(dgvInventario.Rows(e.RowIndex).Cells(2).Value)
        Catch ex As Exception
            iselectedassetid = 0
            sselectedassetdescription = ""
            dselectedassetqty = 0.0
        End Try

    End Sub


    Private Sub dgvInventario_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInventario.CellContentClick

        If dgvInventario.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        Try
            iselectedassetid = CInt(dgvInventario.Rows(e.RowIndex).Cells(0).Value)
            sselectedassetdescription = dgvInventario.Rows(e.RowIndex).Cells(1).Value
            dselectedassetqty = CDbl(dgvInventario.Rows(e.RowIndex).Cells(2).Value)
        Catch ex As Exception
            iselectedassetid = 0
            sselectedassetdescription = ""
            dselectedassetqty = 0.0
        End Try

    End Sub


    Private Sub dgvInventario_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvInventario.SelectionChanged

        txtCantidadDgvInventario = Nothing
        txtCantidadDgvInventario_OldText = Nothing
        txtNombreDgvInventario = Nothing
        txtNombreDgvInventario_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If dgvInventario.CurrentRow.IsNewRow Then
            Exit Sub
        End If

        Try
            iselectedassetid = CInt(dgvInventario.CurrentRow.Cells(0).Value)
            sselectedassetdescription = dgvInventario.CurrentRow.Cells(1).Value
            dselectedassetqty = CDbl(dgvInventario.CurrentRow.Cells(2).Value)
        Catch ex As Exception
            iselectedassetid = 0
            sselectedassetdescription = ""
            dselectedassetqty = 0.0
        End Try

    End Sub


    Private Sub dgvInventario_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInventario.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = txtBuscar.Text.Replace("--", "").Replace("'", "")

        Dim query1 As String

        query1 = "" & _
        "SELECT * FROM ( " & _
        "SELECT * FROM ( " & _
        "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
        "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
        "pa.sassetstatus AS 'Status' " & _
        "FROM assetsphysicalinventory pa " & _
        "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
        "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
        "ORDER BY 6 DESC, 7 DESC " & _
        ") pa " & _
        "GROUP BY iassetid " & _
        ") pa " & _
        "WHERE Cantidad > 0 " & _
        "ORDER BY 2 "

        setDataGridView(dgvInventario, query1, False)

        dgvInventario.Columns(0).Visible = False

        dgvInventario.Columns(0).ReadOnly = True
        dgvInventario.Columns(1).ReadOnly = True
        dgvInventario.Columns(3).ReadOnly = True
        dgvInventario.Columns(5).ReadOnly = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInventario_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvInventario.CellValueChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 1, 2, 4 y 6: sassetdescription, dassetqty, sassetlocation y Status

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If e.ColumnIndex = 1 Then   'sassetdescription

            If dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Activo del Inventario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Activo del Inventario") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", 0, '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

                Else

                    'dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedassetdescription

                End If

            Else

                Dim ba As New BuscaActivos

                ba.susername = susername
                ba.bactive = bactive
                ba.bonline = bonline
                ba.suserfullname = suserfullname

                ba.suseremail = suseremail
                ba.susersession = susersession
                ba.susermachinename = susermachinename
                ba.suserip = suserip

                ba.isEdit = False

                ba.querystring = dgvInventario.CurrentCell.EditedFormattedValue

                If Me.WindowState = FormWindowState.Maximized Then
                    ba.WindowState = FormWindowState.Maximized
                End If

                ba.ShowDialog(Me)

                If ba.DialogResult = Windows.Forms.DialogResult.OK Then

                    If MsgBox("¿Estás seguro de que quieres reemplazar el Activo?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Activo del Inventario") = MsgBoxResult.Yes Then

                        executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", 0, '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")
                        executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & ba.iassetid & ", " & dgvInventario.Rows(e.RowIndex).Cells(2).Value & ", '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

                    Else

                        'dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedassetdescription

                    End If

                End If

            End If


        ElseIf e.ColumnIndex = 2 Then  'dassetqty


            If dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Activo del Inventario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Activo del Inventario") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", 0, '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

                Else

                    dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedassetqty

                End If

            ElseIf dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Activo del Inventario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Activo del Inventario") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", 0, '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

                Else

                    'dgvInventario.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedassetqty

                End If

            Else

                Dim cantidadinventario As Double = 1.0

                Try
                    cantidadinventario = CDbl(dgvInventario.Rows(e.RowIndex).Cells(2).Value)
                Catch ex As Exception
                    cantidadinventario = dselectedassetqty
                End Try

                'executeSQLCommand(0, "UPDATE assetsphysicalinventory ipi JOIN (SELECT * FROM (SELECT pa.iassetid, pa.dassetqty, at.sassettypedescription, pa.sassetlocation, pa.iupdatedate, pa.supdatetime FROM assetsphysicalinventory pa JOIN assets a ON pa.iassetid = a.iassetid ORDER BY 5 DESC, 6 DESC) pa WHERE dassetqty > 0 GROUP BY iassetid) ipi2 ON ipa.iupdatedate = ipi2.iupdatedate AND ipa.supdatetime = ipi2.supdatetime AND ipa.iassetid = ipi2.iassetid SET ipa.dassetqty = " & cantidadinventario & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ipa.iassetid = " & iselectedassetid)
                executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", " & cantidadinventario & ", '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")


            End If


        ElseIf e.ColumnIndex = 4 Then  'sassetlocation

            'executeSQLCommand(0, "UPDATE assetsphysicalinventory ipi JOIN (SELECT * FROM (SELECT pa.iassetid, pa.dassetqty, at.sassettypedescription, pa.sassetlocation, pa.iupdatedate, pa.supdatetime FROM assetsphysicalinventory pa JOIN assets a ON pa.iassetid = a.iassetid ORDER BY 5 DESC, 6 DESC) pa WHERE dassetqty > 0 GROUP BY iassetid) ipi2 ON ipa.iupdatedate = ipi2.iupdatedate AND ipa.supdatetime = ipi2.supdatetime AND ipa.iassetid = ipi2.iassetid SET ipa.sassetlocation = '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ipa.iassetid = " & iselectedassetid)
            executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", " & dgvInventario.Rows(e.RowIndex).Cells(2).Value & ", '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        ElseIf e.ColumnIndex = 6 Then  'sassetstatus

            'executeSQLCommand(0, "UPDATE assetsphysicalinventory ipi JOIN (SELECT * FROM (SELECT pa.iassetid, pa.dassetqty, at.sassettypedescription, pa.sassetlocation, pa.iupdatedate, pa.supdatetime FROM assetsphysicalinventory pa JOIN assets a ON pa.iassetid = a.iassetid ORDER BY 5 DESC, 6 DESC) pa WHERE dassetqty > 0 GROUP BY iassetid) ipi2 ON ipa.iupdatedate = ipi2.iupdatedate AND ipa.supdatetime = ipi2.supdatetime AND ipa.iassetid = ipi2.iassetid SET ipa.sassetstatis = '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE ipa.iassetid = " & iselectedassetid)
            executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & iselectedassetid & ", " & dgvInventario.Rows(e.RowIndex).Cells(2).Value & ", '" & dgvInventario.Rows(e.RowIndex).Cells(4).Value & "', '" & dgvInventario.Rows(e.RowIndex).Cells(6).Value & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvInventario_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvInventario.EditingControlShowing

        If dgvInventario.CurrentCell.ColumnIndex = 1 Then

            txtNombreDgvInventario = CType(e.Control, TextBox)
            txtNombreDgvInventario_OldText = txtNombreDgvInventario.Text

        ElseIf dgvInventario.CurrentCell.ColumnIndex = 2 Then

            txtCantidadDgvInventario = CType(e.Control, TextBox)
            txtCantidadDgvInventario_OldText = txtCantidadDgvInventario.Text

        ElseIf dgvInventario.CurrentCell.ColumnIndex = 4 Then

            txtLugarDgvInventario = CType(e.Control, TextBox)
            txtLugarDgvInventario_OldText = txtLugarDgvInventario.Text

        Else

            txtCantidadDgvInventario = Nothing
            txtCantidadDgvInventario_OldText = Nothing
            txtNombreDgvInventario = Nothing
            txtNombreDgvInventario_OldText = Nothing
            txtLugarDgvInventario = Nothing
            txtLugarDgvInventario_OldText = Nothing

        End If

    End Sub


    Private Sub txtNombreDgvInventario_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDgvInventario.KeyUp

        txtNombreDgvInventario.Text = txtNombreDgvInventario.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNombreDgvInventario.Text.Contains(arrayForbidden1(fc)) Then
                txtNombreDgvInventario.Text = txtNombreDgvInventario.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtCantidadDgvInventario_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvInventario.KeyUp

        If Not IsNumeric(txtCantidadDgvInventario.Text) Then

            Dim strForbidden2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

            For cp = 0 To arrayForbidden2.Length - 1

                If txtCantidadDgvInventario.Text.Contains(arrayForbidden2(cp)) Then
                    txtCantidadDgvInventario.Text = txtCantidadDgvInventario.Text.Replace(arrayForbidden2(cp), "")
                End If

            Next cp

            If txtCantidadDgvInventario.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvInventario.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvInventario.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvInventario.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvInventario.Text = txtCantidadDgvInventario.Text.Substring(0, lugar) & txtCantidadDgvInventario.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvInventario.Text = txtCantidadDgvInventario.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvInventario.Text = txtCantidadDgvInventario.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvInventario.Text = txtCantidadDgvInventario.Text.Trim

        Else
            txtCantidadDgvInventario_OldText = txtCantidadDgvInventario.Text
        End If

    End Sub


    Private Sub txtLugarDgvInventario_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLugarDgvInventario.KeyUp

        txtLugarDgvInventario.Text = txtLugarDgvInventario.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtLugarDgvInventario.Text.Contains(arrayForbidden1(fc)) Then
                txtLugarDgvInventario.Text = txtLugarDgvInventario.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub dgvInventario_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvInventario.KeyUp

        If e.KeyCode = Keys.Delete Then

            If MsgBox("¿Estás seguro de que quieres eliminar este Activo del Inventario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Activo del Inventario") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedassetid As Integer = 0
                Dim tmpselectedassetlocation As String = ""
                Dim tmpselectedassetstatus As String = ""

                Try
                    tmpselectedassetid = CInt(dgvInventario.CurrentRow.Cells(0).Value)
                    tmpselectedassetlocation = dgvInventario.CurrentRow.Cells(4).Value
                    tmpselectedassetstatus = CDbl(dgvInventario.CurrentRow.Cells(6).Value)
                Catch ex As Exception

                End Try

                executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & tmpselectedassetid & ", 0, '" & tmpselectedassetlocation & "', '" & tmpselectedassetstatus & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")


                Dim query1 As String

                querystring = txtBuscar.Text.Replace("--", "").Replace("'", "")

                query1 = "" & _
                "SELECT * FROM ( " & _
                "SELECT * FROM ( " & _
                "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
                "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
                "pa.sassetstatus AS 'Status' " & _
                "FROM assetsphysicalinventory pa " & _
                "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
                "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
                "ORDER BY 6 DESC, 7 DESC " & _
                ") pa " & _
                "GROUP BY iassetid " & _
                ") pa " & _
                "WHERE Cantidad > 0 " & _
                "ORDER BY 2 "

                setDataGridView(dgvInventario, query1, False)

                dgvInventario.Columns(0).Visible = False

                dgvInventario.Columns(0).ReadOnly = True
                dgvInventario.Columns(1).ReadOnly = True
                dgvInventario.Columns(3).ReadOnly = True
                dgvInventario.Columns(5).ReadOnly = True


                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvInventario_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvInventario.UserAddedRow

        Dim ba As New BuscaActivos

        ba.susername = susername
        ba.bactive = bactive
        ba.bonline = bonline
        ba.suserfullname = suserfullname

        ba.suseremail = suseremail
        ba.susersession = susersession
        ba.susermachinename = susermachinename
        ba.suserip = suserip

        ba.isEdit = False

        ba.querystring = dgvInventario.CurrentCell.EditedFormattedValue

        If Me.WindowState = FormWindowState.Maximized Then
            ba.WindowState = FormWindowState.Maximized
        End If

        ba.ShowDialog(Me)

        If ba.DialogResult = Windows.Forms.DialogResult.OK Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & ba.iassetid & ", 1, '', '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

            'dgvInventario.EndEdit()

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub tcInventario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcInventario.SelectedIndexChanged


        querystring = txtBuscar.Text

        If tcInventario.SelectedTab Is tpInventario Then

            Dim query1 As String

            query1 = "" & _
            "SELECT * FROM ( " & _
            "SELECT * FROM ( " & _
            "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
            "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
            "pa.sassetstatus AS 'Status' " & _
            "FROM assetsphysicalinventory pa " & _
            "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
            "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
            "ORDER BY 6 DESC, 7 DESC " & _
            ") pa " & _
            "GROUP BY iassetid " & _
            ") pa " & _
            "WHERE Cantidad > 0 " & _
            "ORDER BY 2 "

            setDataGridView(dgvInventario, query1, False)

            dgvInventario.Columns(0).Visible = False

            dgvInventario.Columns(0).ReadOnly = True
            dgvInventario.Columns(1).ReadOnly = True
            dgvInventario.Columns(3).ReadOnly = True
            dgvInventario.Columns(5).ReadOnly = True


        End If

    End Sub


    'Private Sub btnRehacerTodoElInventario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    If executeSQLCommand(0, "INSERT IGNORE INTO assetsphysicalinventory SELECT iassetid, 0, sassetlocation, 'REHACIENDO INVENTARIO', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "' FROM assetsphysicalinventory") = True Then

    '        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Se rehizo TODO el Inventario', 'OK')")

    '    End If

    '    txtBuscar.Text = ""

    '    querystring = txtBuscar.Text

    '    Dim query1 As String

    '    query1 = "" & _
    '    "SELECT * FROM ( " & _
    '    "SELECT * FROM ( " & _
    '    "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
    '    "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
    '    "pa.sassetstatus AS 'Status' " & _
    '    "FROM assetsphysicalinventory pa " & _
    '    "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
    '    "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
    '    "ORDER BY 6 DESC, 7 DESC " & _
    '    ") pa " & _
    '    "GROUP BY iassetid " & _
    '    ") pa " & _
    '    "WHERE Cantidad > 0 " & _
    '    "ORDER BY 2 "

    '    setDataGridView(dgvInventario, query1, False)

    '    dgvInventario.Columns(0).Visible = False

    '    dgvInventario.Columns(0).ReadOnly = True
    '    dgvInventario.Columns(1).ReadOnly = True
    '    dgvInventario.Columns(3).ReadOnly = True
    '    dgvInventario.Columns(5).ReadOnly = True


    '    isEdit = True

    '    MsgBox("Estamos listos para iniciar el Inventario", MsgBoxStyle.OkOnly, "Iniciar Inventario")

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    Private Sub btnRehacerInventarioMostrado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRehacerInventarioMostrado.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = txtBuscar.Text.Replace("--", "").Replace("'", "")

        executeSQLCommand(0, "INSERT IGNORE INTO assetsphysicalinventory SELECT iassetid, 0, sassetlocation, 'REHACIENDO INVENTARIO', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "' FROM (" & _
        "SELECT * FROM ( " & _
        "SELECT * FROM ( " & _
        "SELECT pa.iassetid, a.sassetdescription, pa.dassetqty, at.sassettypedescription, pa.sassetlocation, " & _
        "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T'), pa.sassetstatus " & _
        "FROM assetsphysicalinventory pa " & _
        "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
        "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
        "ORDER BY 6 DESC, 7 DESC " & _
        ") pa " & _
        "GROUP BY iassetid " & _
        ") pa " & _
        "WHERE Cantidad > 0 " & _
        "ORDER BY 2 " & _
        ") ri ")

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Se rehizo el Inventario Mostrado', 'OK')")

        Dim query1 As String

        query1 = "" & _
        "SELECT * FROM ( " & _
        "SELECT * FROM ( " & _
        "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
        "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
        "pa.sassetstatus AS 'Status' " & _
        "FROM assetsphysicalinventory pa " & _
        "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
        "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
        "ORDER BY 6 DESC, 7 DESC " & _
        ") pa " & _
        "GROUP BY iassetid " & _
        ") pa " & _
        "WHERE Cantidad > 0 " & _
        "ORDER BY 2 "

        setDataGridView(dgvInventario, query1, False)

        dgvInventario.Columns(0).Visible = False

        dgvInventario.Columns(0).ReadOnly = True
        dgvInventario.Columns(1).ReadOnly = True
        dgvInventario.Columns(3).ReadOnly = True
        dgvInventario.Columns(5).ReadOnly = True


        isEdit = True

        btnRehacerInventarioMostrado.Visible = False

        MsgBox("Estamos listos para iniciar el Inventario", MsgBoxStyle.OkOnly, "Iniciar Inventario")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnGenerarHojaConfirmacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarHojaConfirmacion.Click

        If dgvInventario.RowCount < 1 Then
            MsgBox("No hay nada que verificar", MsgBoxStyle.OkOnly, "No se generó el documento")
            Exit Sub
        End If

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

            msSaveFileDialog.Filter = "Word Files (*.doc) |*.doc"
            msSaveFileDialog.DefaultExt = "*.doc"

            msSaveFileDialog.FileName = "Recibo Confirmacion Inventario " & fecha

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToWord(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Recibo Exportado Correctamente!", "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar el recibo. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar el recibo")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Sub btnExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportarExcel.Click

        If dgvInventario.RowCount < 1 Then
            MsgBox("No hay nada que exportar", MsgBoxStyle.OkOnly, "No se hizo la exportación")
            Exit Sub
        End If

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

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            msSaveFileDialog.FileName = "Inventario " & fecha

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Inventario Exportado Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar el Inventario. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar el Inventario")
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
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""494.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""63""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""65.25"" ss:Span=""5""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""6"" ss:StyleID=""1""><Data ss:Type=""String"">INVENTARIO DE INSUMOS</Data></Cell>")
            fs.WriteLine("   </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""45"">")

            For Each col As DataGridViewColumn In dgvInventario.Columns

                If col.Visible Then

                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))

                End If

            Next


            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvInventario.Rows

                If dgvInventario.AllowUserToAddRows = True And row.Index = dgvInventario.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvInventario.Columns

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


    Private Function ExportToWord(ByVal pth As String) As Boolean

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            Dim wd As Microsoft.Office.Interop.Word.ApplicationClass
            wd = New Microsoft.Office.Interop.Word.ApplicationClass

            Dim missing As Object = System.Reflection.Missing.Value
            Dim fileName As Object = "normal.dot"
            Dim newTemplate As Object = False
            Dim docType As Object = 0
            Dim isVisible As Object = True

            Dim doc As Microsoft.Office.Interop.Word.Document = wd.Documents.Add(fileName, newTemplate, docType, isVisible)
            wd.Visible = True
            doc.Activate()

            wd.Selection.Font.Size = 12
            wd.Selection.Font.Name = "Arial"
            wd.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify

            wd.Selection.TypeText("HOJA DE CONTROL DE INVENTARIO DE ACTIVOS al " & Now.Day & " de " & Date.Today.ToString("MMMM") & " del " & Now.Year & vbNewLine & vbNewLine)

            wd.Selection.TypeText("Yo, " & getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = (SELECT ipeopleid FROM users WHERE susername = '" & susername & "')") & ", confirmo mediante mi firma que a esta fecha los siguientes productos se encontraban físicamente en el lugar indicado en el status indicado en cada uno. ")

            Dim inicioLista As Integer = wd.Selection.Start

            For Each row As DataGridViewRow In dgvInventario.Rows

                If dgvInventario.AllowUserToAddRows = True And row.Index = dgvInventario.Rows.Count - 1 Then
                    Exit For
                End If

                wd.Selection.TypeText("• " & row.Cells(2).Value.ToString & " " & row.Cells(3).Value.ToString & " DE " & row.Cells(1).Value.ToString & " EN " & row.Cells(4).Value.ToString & ". Status: " & row.Cells(6).Value.ToString & vbNewLine)

            Next row

            Dim finLista As Integer = wd.Selection.Start

            wd.Selection.TypeText(vbNewLine)

            wd.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter

            wd.Selection.TypeText("ATENTAMENTE" & vbNewLine)
            wd.Selection.TypeText(vbNewLine & vbNewLine & vbNewLine)
            wd.Selection.TypeText(getSQLQueryAsString(0, "SELECT speoplefullname FROM people WHERE ipeopleid = (SELECT ipeopleid FROM users WHERE susername = '" & susername & "')").ToUpper & " " & vbNewLine)


            doc.SaveAs(pth)

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Return True

        Catch ex As Exception

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Return False

        End Try

    End Function


    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub


    Private Sub btnNuevoActivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoActivo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim aa As New AgregarActivo

        aa.susername = susername
        aa.bactive = bactive
        aa.bonline = bonline
        aa.suserfullname = suserfullname

        aa.suseremail = suseremail
        aa.susersession = susersession
        aa.susermachinename = susermachinename
        aa.suserip = suserip

        aa.isEdit = False
        aa.isRecover = False

        If Me.WindowState = FormWindowState.Maximized Then
            aa.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        aa.ShowDialog(Me)
        Me.Visible = True

        If aa.DialogResult = Windows.Forms.DialogResult.OK Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & aa.iassetid & ", 1, '', '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

            querystring = txtBuscar.Text.Replace("--", "").Replace("'", "")

            Dim query1 As String

            query1 = "" & _
            "SELECT * FROM ( " & _
            "SELECT * FROM ( " & _
            "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
            "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
            "pa.sassetstatus AS 'Status' " & _
            "FROM assetsphysicalinventory pa " & _
            "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
            "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
            "ORDER BY 6 DESC, 7 DESC " & _
            ") pa " & _
            "GROUP BY iassetid " & _
            ") pa " & _
            "WHERE Cantidad > 0 " & _
            "ORDER BY 2 "

            setDataGridView(dgvInventario, query1, False)

            dgvInventario.Columns(0).Visible = False

            dgvInventario.Columns(0).ReadOnly = True
            dgvInventario.Columns(1).ReadOnly = True
            dgvInventario.Columns(3).ReadOnly = True
            dgvInventario.Columns(5).ReadOnly = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnInsertarActivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarActivo.Click

        Dim ba As New BuscaActivos

        ba.susername = susername
        ba.bactive = bactive
        ba.bonline = bonline
        ba.suserfullname = suserfullname

        ba.suseremail = suseremail
        ba.susersession = susersession
        ba.susermachinename = susermachinename
        ba.suserip = suserip

        ba.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ba.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ba.ShowDialog(Me)
        Me.Visible = True

        If ba.DialogResult = Windows.Forms.DialogResult.OK Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & ba.iassetid & ", 1, '', '', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

            querystring = txtBuscar.Text.Replace("--", "").Replace("'", "")

            Dim query1 As String

            query1 = "" & _
            "SELECT * FROM ( " & _
            "SELECT * FROM ( " & _
            "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
            "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
            "pa.sassetstatus AS 'Status' " & _
            "FROM assetsphysicalinventory pa " & _
            "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
            "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
            "ORDER BY 6 DESC, 7 DESC " & _
            ") pa " & _
            "GROUP BY iassetid " & _
            ") pa " & _
            "WHERE Cantidad > 0 " & _
            "ORDER BY 2 "

            setDataGridView(dgvInventario, query1, False)

            dgvInventario.Columns(0).Visible = False

            dgvInventario.Columns(0).ReadOnly = True
            dgvInventario.Columns(1).ReadOnly = True
            dgvInventario.Columns(3).ReadOnly = True
            dgvInventario.Columns(5).ReadOnly = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnEliminarActivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarActivo.Click

        If MsgBox("¿Estás seguro de que quieres eliminar este Activo del Inventario?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Activo del Inventario") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedassetid As Integer = 0
            Dim tmpselectedassetlocation As String = ""
            Dim tmpselectedassetstatus As String = ""

            Try
                tmpselectedassetid = CInt(dgvInventario.CurrentRow.Cells(0).Value)
                tmpselectedassetlocation = dgvInventario.CurrentRow.Cells(4).Value
                tmpselectedassetstatus = CDbl(dgvInventario.CurrentRow.Cells(6).Value)
            Catch ex As Exception

            End Try

            If tmpselectedassetid = 0 Then
                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If

            executeSQLCommand(0, "INSERT INTO assetsphysicalinventory VALUES (" & tmpselectedassetid & ", 0, '" & tmpselectedassetlocation & "', '" & tmpselectedassetstatus & "', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")


            Dim query1 As String

            querystring = txtBuscar.Text.Replace("--", "").Replace("'", "")

            query1 = "" & _
            "SELECT * FROM ( " & _
            "SELECT * FROM ( " & _
            "SELECT pa.iassetid, a.sassetdescription AS 'Material', pa.dassetqty AS 'Cantidad', at.sassettypedescription AS 'Tipo', pa.sassetlocation AS 'Lugar', " & _
            "STR_TO_DATE(CONCAT(pa.iupdatedate, ' ', pa.supdatetime), '%Y%c%d %T') AS 'Fecha Ultima Mod', " & _
            "pa.sassetstatus AS 'Status' " & _
            "FROM assetsphysicalinventory pa " & _
            "JOIN assets a ON pa.iassetid = a.iassetid JOIN assettypes at ON at.iassettypeid = a.iassettypeid " & _
            "WHERE a.sassetdescription LIKE '%" & querystring & "%' OR pa.dassetqty LIKE '%" & querystring & "%' OR at.sassettypedescription LIKE '%" & querystring & "%' OR pa.sassetlocation LIKE '%" & querystring & "%' OR pa.sassetstatus LIKE '%" & querystring & "%' " & _
            "ORDER BY 6 DESC, 7 DESC " & _
            ") pa " & _
            "GROUP BY iassetid " & _
            ") pa " & _
            "WHERE Cantidad > 0 " & _
            "ORDER BY 2 "

            setDataGridView(dgvInventario, query1, False)

            dgvInventario.Columns(0).Visible = False

            dgvInventario.Columns(0).ReadOnly = True
            dgvInventario.Columns(1).ReadOnly = True
            dgvInventario.Columns(3).ReadOnly = True
            dgvInventario.Columns(5).ReadOnly = True


            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click

        Dim br As New BuscaRevisiones

        br.susername = susername
        br.bactive = bactive
        br.bonline = bonline
        br.suserfullname = suserfullname
        br.suseremail = suseremail
        br.susersession = susersession
        br.susermachinename = susermachinename
        br.suserip = suserip

        br.isEdit = True

        br.srevisiondocument = "Inventario de Activos"
        br.sid = getMySQLDate() & " " & getAppTime()

        If Me.WindowState = FormWindowState.Maximized Then
            br.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        br.ShowDialog(Me)
        Me.Visible = True

    End Sub


End Class