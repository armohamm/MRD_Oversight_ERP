Public Class BuscaTarjetas

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
    Public scarddescription As String = ""
    Public scardlegacyid As String = ""

    Public isEdit As Boolean = False

    Public isHistoric As Boolean = False

    Public isBase As Boolean = False
    Public isModel As Boolean = False

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False
    Private viewIndirectCost As Boolean = False
    Private viewProfit As Boolean = False
    Private viewAmount As Boolean = False

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
                    btnCrear.Enabled = True
                End If

                If permission = "Modificar" Then
                    openPermission = True
                    btnAbrir.Enabled = True
                End If

                If permission = "Ver Indirecto" Then
                    viewIndirectCost = True
                End If

                If permission = "Ver Utilidad" Then
                    viewProfit = True
                End If

                If permission = "Ver Importe" Then
                    viewAmount = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Tarjetas', 'OK')")

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


    Private Sub BuscaTarjetas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Tarjetas con la Tarjeta " & icardid & " : " & scardlegacyid & " : " & scarddescription & " seleccionada', 'OK')")

    End Sub


    Private Sub BuscaTarjetas_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaTarjetas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnAbrir
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        txtBuscar.Text = querystring

        Dim queryBusqueda As String
        queryBusqueda = "" & _
        "SELECT btf.icardid, btf.scardlegacyid AS 'ID', btf.scarddescription AS 'Descripcion Tarjeta', btf.scardunit AS 'Unidad', " & _
        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)), 2) AS 'Importe Indirectos', " & _
        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)), 2) AS 'Importe Utilidad', " & _
        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS 'Importe Total' " & _
        "FROM basecards btf " & _
        "JOIN cardlegacycategories tflc ON btf.scardlegacycategoryid = tflc.scardlegacycategoryid " & _
        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM basecardinputs btfi JOIN basecards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM basecards btf LEFT JOIN basecardinputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM basecards btf LEFT JOIN basecardinputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM basecards btf LEFT JOIN basecardinputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btf.icardid, btfi.ibaseid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
        "WHERE " & _
        "scardlegacyid LIKE '%" & querystring & "%' OR " & _
        "scarddescription LIKE '%" & querystring & "%' "

        setDataGridView(dgvTarjetas, queryBusqueda, True)

        dgvTarjetas.Columns(0).Visible = False

        If viewIndirectCost = False Then
            dgvTarjetas.Columns(4).Visible = False
        End If

        If viewProfit = False Then
            dgvTarjetas.Columns(5).Visible = False
        End If

        If viewAmount = False Then
            dgvTarjetas.Columns(6).Visible = False
        End If

        dgvTarjetas.Columns(0).Width = 30
        dgvTarjetas.Columns(1).Width = 60
        dgvTarjetas.Columns(2).Width = 300
        dgvTarjetas.Columns(3).Width = 60
        dgvTarjetas.Columns(4).Width = 70
        dgvTarjetas.Columns(5).Width = 70
        dgvTarjetas.Columns(6).Width = 70

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Tarjetas', 'OK')")

        dgvTarjetas.Select()

        txtBuscar.Select()
        txtBuscar.Focus()
        txtBuscar.SelectionStart() = txtBuscar.Text.Length

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

    End Sub


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = txtBuscar.Text.Replace(" ", "%").Replace("--", "").Replace("'", " PIES")

        If txtBuscar.Text.Contains("'") = True Then
            txtBuscar.Text = txtBuscar.Text.Replace("'", "")
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If txtBuscar.Text.Length < 3 Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim queryBusqueda As String

        queryBusqueda = "" & _
        "SELECT btf.icardid, btf.scardlegacyid AS 'ID', btf.scarddescription AS 'Descripcion Tarjeta', btf.scardunit AS 'Unidad', " & _
        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(btf.dcardindirectcostspercentage)), 2) AS 'Importe Indirectos', " & _
        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(btf.dcardgainpercentage)), 2) AS 'Importe Utilidad', " & _
        "FORMAT(((IF(costoMAT.costo is null, 0, costoMAT.costo) + IF(costoMO.costo is null, 0, costoMO.costo) + IF(costoEQ.costo is null, 0, costoEQ.costo))*(1+btf.dcardindirectcostspercentage)*(1+btf.dcardgainpercentage)), 2) AS 'Importe Total' " & _
        "FROM basecards btf " & _
        "JOIN cardlegacycategories tflc ON btf.scardlegacycategoryid = tflc.scardlegacycategoryid " & _
        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, (costoMO.costo*btfi.dcardinputqty) AS costo FROM basecardinputs btfi JOIN basecards btf ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid JOIN (SELECT btfi.ibaseid, btfi.icardid AS icardid, 0 AS iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM basecards btf LEFT JOIN basecardinputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btfi.iinputid = costoMO.iinputid AND btfi.icardid = costoMO.icardid GROUP BY btfi.icardid, btfi.ibaseid) AS costoEQ ON btf.ibaseid = costoEQ.ibaseid AND btf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM basecards btf LEFT JOIN basecardinputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY btf.icardid, btfi.ibaseid) AS costoMO ON btf.ibaseid = costoMO.ibaseid AND btf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT btfi.ibaseid, btfi.icardid, btfi.iinputid, SUM(btfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM basecards btf LEFT JOIN basecardinputs btfi ON btf.ibaseid = btfi.ibaseid AND btf.icardid = btfi.icardid LEFT JOIN inputs i ON btfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, ibaseid) bp ON btfi.ibaseid = bp.ibaseid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY btf.icardid, btfi.ibaseid) AS costoMAT ON btf.ibaseid = costoMAT.ibaseid AND btf.icardid = costoMAT.icardid " & _
        "WHERE " & _
        "scardlegacyid LIKE '%" & querystring & "%' OR " & _
        "scarddescription LIKE '%" & querystring & "%' "

        setDataGridView(dgvTarjetas, queryBusqueda, True)

        dgvTarjetas.Columns(0).Visible = False

        If viewIndirectCost = False Then
            dgvTarjetas.Columns(4).Visible = False
        End If

        If viewProfit = False Then
            dgvTarjetas.Columns(5).Visible = False
        End If

        If viewAmount = False Then
            dgvTarjetas.Columns(6).Visible = False
        End If

        dgvTarjetas.Columns(0).Width = 30
        dgvTarjetas.Columns(1).Width = 60
        dgvTarjetas.Columns(2).Width = 300
        dgvTarjetas.Columns(3).Width = 60
        dgvTarjetas.Columns(4).Width = 70
        dgvTarjetas.Columns(5).Width = 70
        dgvTarjetas.Columns(6).Width = 70

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTarjetas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTarjetas.CellClick

        Try

            If dgvTarjetas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            icardid = CInt(dgvTarjetas.SelectedRows().Item(0).Cells(0).Value)
            scardlegacyid = dgvTarjetas.SelectedRows().Item(0).Cells(1).Value
            scarddescription = dgvTarjetas.SelectedRows().Item(0).Cells(2).Value

        Catch ex As Exception

            icardid = 0
            scardlegacyid = ""
            scarddescription = ""

        End Try

    End Sub


    Private Sub dgvTarjetas_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTarjetas.CellContentClick

        Try

            If dgvTarjetas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            icardid = CInt(dgvTarjetas.SelectedRows().Item(0).Cells(0).Value)
            scardlegacyid = dgvTarjetas.SelectedRows().Item(0).Cells(1).Value
            scarddescription = dgvTarjetas.SelectedRows().Item(0).Cells(2).Value

        Catch ex As Exception

            icardid = 0
            scardlegacyid = ""
            scarddescription = ""

        End Try

    End Sub


    Private Sub dgvTarjetas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTarjetas.SelectionChanged

        Try

            If dgvTarjetas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            icardid = CInt(dgvTarjetas.SelectedRows().Item(0).Cells(0).Value)
            scardlegacyid = dgvTarjetas.SelectedRows().Item(0).Cells(1).Value
            scarddescription = dgvTarjetas.SelectedRows().Item(0).Cells(2).Value

        Catch ex As Exception

            icardid = 0
            scardlegacyid = ""
            scarddescription = ""

        End Try

    End Sub


    Private Sub dgvTarjetas_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTarjetas.CellContentDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvTarjetas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            icardid = CInt(dgvTarjetas.SelectedRows().Item(0).Cells(0).Value)
            scardlegacyid = dgvTarjetas.SelectedRows().Item(0).Cells(1).Value
            scarddescription = dgvTarjetas.SelectedRows().Item(0).Cells(2).Value

        Catch ex As Exception

            icardid = 0
            scardlegacyid = ""
            scarddescription = ""

        End Try


        If dgvTarjetas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim af As New AgregarTarjeta

            af.susername = susername
            af.bactive = bactive
            af.bonline = bonline
            af.suserfullname = suserfullname
            af.suseremail = suseremail
            af.susersession = susersession
            af.susermachinename = susermachinename
            af.suserip = suserip

            af.IsBase = isBase
            af.IsModel = isModel

            If isBase = True Then

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If iprojectid = baseid Then
                    af.IsHistoric = False
                Else
                    af.IsHistoric = True
                End If

            Else

                af.IsHistoric = isHistoric

            End If

            af.IsEdit = True

            af.iprojectid = iprojectid
            af.icardid = icardid

            If Me.WindowState = FormWindowState.Maximized Then
                af.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            af.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTarjetas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTarjetas.CellDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvTarjetas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            icardid = CInt(dgvTarjetas.SelectedRows().Item(0).Cells(0).Value)
            scardlegacyid = dgvTarjetas.SelectedRows().Item(0).Cells(1).Value
            scarddescription = dgvTarjetas.SelectedRows().Item(0).Cells(2).Value

        Catch ex As Exception

            icardid = 0
            scardlegacyid = ""
            scarddescription = ""

        End Try


        If dgvTarjetas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim af As New AgregarTarjeta

            af.susername = susername
            af.bactive = bactive
            af.bonline = bonline
            af.suserfullname = suserfullname
            af.suseremail = suseremail
            af.susersession = susersession
            af.susermachinename = susermachinename
            af.suserip = suserip

            af.IsBase = isBase
            af.IsModel = isModel


            If isBase = True Then

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If iprojectid = baseid Then
                    af.IsHistoric = False
                Else
                    af.IsHistoric = True
                End If

            Else

                af.IsHistoric = isHistoric

            End If

            af.IsEdit = True

            af.iprojectid = iprojectid
            af.icardid = icardid

            If Me.WindowState = FormWindowState.Maximized Then
                af.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            af.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        querystring = ""

        icardid = 0
        scarddescription = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If dgvTarjetas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim af As New AgregarTarjeta

            af.susername = susername
            af.bactive = bactive
            af.bonline = bonline
            af.suserfullname = suserfullname
            af.suseremail = suseremail
            af.susersession = susersession
            af.susermachinename = susermachinename
            af.suserip = suserip

            af.IsBase = isBase
            af.IsModel = isModel


            If isBase = True Then

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If iprojectid = baseid Then
                    af.IsHistoric = False
                Else
                    af.IsHistoric = True
                End If

            Else

                af.IsHistoric = isHistoric

            End If

            af.IsEdit = True

            af.iprojectid = iprojectid
            af.icardid = icardid

            If Me.WindowState = FormWindowState.Maximized Then
                af.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            af.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub btnCrear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrear.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim af As New AgregarTarjeta

        af.susername = susername
        af.bactive = bactive
        af.bonline = bonline
        af.suserfullname = suserfullname
        af.suseremail = suseremail
        af.susersession = susersession
        af.susermachinename = susermachinename
        af.suserip = suserip

        af.iprojectid = iprojectid

        If Me.WindowState = FormWindowState.Maximized Then
            af.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        af.ShowDialog(Me)
        Me.Visible = True

        If af.DialogResult = Windows.Forms.DialogResult.OK Then

            icardid = af.icardid
            scarddescription = af.scarddescription
            scardlegacyid = af.scardlegacyid

            If af.wasCreated = True Then
                Me.wasCreated = True
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class