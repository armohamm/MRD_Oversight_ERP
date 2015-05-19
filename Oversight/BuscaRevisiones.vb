Public Class BuscaRevisiones

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

    Public irevisionid As Integer = 0
    Public sid As String = ""
    Public srevisiondocument As String = ""
    Public srevisionresult As String = ""

    Public isEdit As Boolean = False

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False

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

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Revisiones', 'OK')")

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


    Private Sub BuscaRevisiones_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Revisiones con la Revision " & irevisionid & " : " & srevisionresult & " seleccionada', 'OK')")

    End Sub


    Private Sub BuscaRevisiones_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaRevisiones_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim queryBusqueda As String

        txtBuscar.Text = querystring

        queryBusqueda = "" & _
        "SELECT r.irevisionid, r.srevisiondocument, r.sid, pe.speoplefullname AS 'Usuario', " & _
        "rr.srevisionresult AS 'Resultado Revision', r.srevisioncomment AS 'Comentarios', " & _
        "STR_TO_DATE(CONCAT(r.iupdatedate, ' ', r.supdatetime), '%Y%c%d %T') AS 'Fecha Revision' " & _
        "FROM revisions r " & _
        "JOIN users u ON r.srevisionusername = u.susername " & _
        "JOIN people pe ON pe.ipeopleid = u.ipeopleid " & _
        "JOIN revisionresults rr ON r.irevisionresultid = rr.irevisionresultid " & _
        "WHERE r.srevisiondocument = '" & srevisiondocument & "' AND r.sid = '" & sid & "' " & _
        "AND (r.srevisioncomment LIKE '%" & querystring & "%' OR rr.srevisionresult LIKE '%" & querystring & "%') " & _
        "ORDER BY 7 DESC "

        txtBuscar.Enabled = True

        setDataGridView(dgvRevisiones, queryBusqueda, True)

        dgvRevisiones.Columns(0).Visible = False
        dgvRevisiones.Columns(1).Visible = False
        dgvRevisiones.Columns(2).Visible = False

        dgvRevisiones.Columns(3).Width = 100
        dgvRevisiones.Columns(4).Width = 70
        dgvRevisiones.Columns(5).Width = 100
        dgvRevisiones.Columns(6).Width = 70

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Revisiones', 'OK')")

        dgvRevisiones.Select()

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

        txtBuscar.Text = txtBuscar.Text.Replace("--", "").Replace("'", "")

        querystring = txtBuscar.Text

        Dim queryBusqueda As String

        queryBusqueda = "" & _
        "SELECT r.irevisionid, r.srevisiondocument, r.sid, pe.speoplefullname AS 'Usuario', " & _
        "rr.srevisionresult AS 'Resultado Revision', r.srevisioncomment AS 'Comentarios', " & _
        "STR_TO_DATE(CONCAT(r.iupdatedate, ' ', r.supdatetime), '%Y%c%d %T') AS 'Fecha Revision' " & _
        "FROM revisions r " & _
        "JOIN users u ON r.srevisionusername = u.susername " & _
        "JOIN people pe ON pe.ipeopleid = u.ipeopleid " & _
        "JOIN revisionresults rr ON r.irevisionresultid = rr.irevisionresultid " & _
        "WHERE r.srevisiondocument = '" & srevisiondocument & "' AND r.sid = '" & sid & "' " & _
        "AND (r.srevisioncomment LIKE '%" & querystring & "%' OR rr.srevisionresult LIKE '%" & querystring & "%') " & _
        "ORDER BY 7 DESC "

        txtBuscar.Enabled = True

        setDataGridView(dgvRevisiones, queryBusqueda, True)

        dgvRevisiones.Columns(0).Visible = False
        dgvRevisiones.Columns(1).Visible = False
        dgvRevisiones.Columns(2).Visible = False

        dgvRevisiones.Columns(3).Width = 100
        dgvRevisiones.Columns(4).Width = 70
        dgvRevisiones.Columns(5).Width = 100
        dgvRevisiones.Columns(6).Width = 70

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvRevisiones_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRevisiones.CellClick

        Try

            If dgvRevisiones.CurrentRow Is Nothing Then
                Exit Sub
            End If

            irevisionid = CInt(dgvRevisiones.Rows(e.RowIndex).Cells(0).Value)
            srevisiondocument = dgvRevisiones.Rows(e.RowIndex).Cells(1).Value
            sid = dgvRevisiones.Rows(e.RowIndex).Cells(2).Value
            srevisionresult = dgvRevisiones.Rows(e.RowIndex).Cells(3).Value

        Catch ex As Exception

            irevisionid = 0
            srevisiondocument = ""
            sid = ""
            srevisionresult = ""

        End Try

    End Sub


    Private Sub dgvRevisiones_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRevisiones.CellContentClick

        Try

            If dgvRevisiones.CurrentRow Is Nothing Then
                Exit Sub
            End If

            irevisionid = CInt(dgvRevisiones.Rows(e.RowIndex).Cells(0).Value)
            srevisiondocument = dgvRevisiones.Rows(e.RowIndex).Cells(1).Value
            sid = dgvRevisiones.Rows(e.RowIndex).Cells(2).Value
            srevisionresult = dgvRevisiones.Rows(e.RowIndex).Cells(3).Value

        Catch ex As Exception

            irevisionid = 0
            srevisiondocument = ""
            sid = ""
            srevisionresult = ""

        End Try

    End Sub


    Private Sub dgvRevisiones_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvRevisiones.SelectionChanged

        Try

            If dgvRevisiones.CurrentRow Is Nothing Then
                Exit Sub
            End If

            irevisionid = CInt(dgvRevisiones.CurrentRow.Cells(0).Value)
            srevisiondocument = dgvRevisiones.CurrentRow.Cells(1).Value
            sid = dgvRevisiones.CurrentRow.Cells(2).Value
            srevisionresult = dgvRevisiones.CurrentRow.Cells(3).Value

        Catch ex As Exception

            irevisionid = 0
            srevisiondocument = ""
            sid = ""
            srevisionresult = ""

        End Try

    End Sub


    Private Sub dgvRevisiones_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRevisiones.CellContentDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvRevisiones.CurrentRow Is Nothing Then
                Exit Sub
            End If

            irevisionid = CInt(dgvRevisiones.Rows(e.RowIndex).Cells(0).Value)
            srevisiondocument = dgvRevisiones.Rows(e.RowIndex).Cells(1).Value
            sid = dgvRevisiones.Rows(e.RowIndex).Cells(2).Value
            srevisionresult = dgvRevisiones.Rows(e.RowIndex).Cells(3).Value

        Catch ex As Exception

            irevisionid = 0
            srevisiondocument = ""
            sid = ""
            srevisionresult = ""

        End Try

        If dgvRevisiones.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim ab As New AgregarRevision

            ab.susername = susername
            ab.bactive = bactive
            ab.bonline = bonline
            ab.suserfullname = suserfullname
            ab.suseremail = suseremail
            ab.susersession = susersession
            ab.susermachinename = susermachinename
            ab.suserip = suserip

            ab.irevisionid = irevisionid

            ab.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ab.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ab.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvRevisiones_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvRevisiones.CellDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvRevisiones.CurrentRow Is Nothing Then
                Exit Sub
            End If

            irevisionid = CInt(dgvRevisiones.Rows(e.RowIndex).Cells(0).Value)
            srevisiondocument = dgvRevisiones.Rows(e.RowIndex).Cells(1).Value
            sid = dgvRevisiones.Rows(e.RowIndex).Cells(2).Value
            srevisionresult = dgvRevisiones.Rows(e.RowIndex).Cells(3).Value

        Catch ex As Exception

            irevisionid = 0
            srevisiondocument = ""
            sid = ""
            srevisionresult = ""

        End Try

        If dgvRevisiones.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim ab As New AgregarRevision

            ab.susername = susername
            ab.bactive = bactive
            ab.bonline = bonline
            ab.suserfullname = suserfullname
            ab.suseremail = suseremail
            ab.susersession = susersession
            ab.susermachinename = susermachinename
            ab.suserip = suserip

            ab.irevisionid = irevisionid

            ab.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ab.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ab.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        querystring = ""

        irevisionid = 0
        srevisionresult = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If dgvRevisiones.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim ab As New AgregarRevision

            ab.susername = susername
            ab.bactive = bactive
            ab.bonline = bonline
            ab.suserfullname = suserfullname
            ab.suseremail = suseremail
            ab.susersession = susersession
            ab.susermachinename = susermachinename
            ab.suserip = suserip

            ab.irevisionid = irevisionid

            ab.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ab.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ab.ShowDialog(Me)
            Me.Visible = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCrear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrear.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ar As New AgregarRevision
        ar.susername = susername
        ar.bactive = bactive
        ar.bonline = bonline
        ar.suserfullname = suserfullname
        ar.suseremail = suseremail
        ar.susersession = susersession
        ar.susermachinename = susermachinename
        ar.suserip = suserip

        ar.isEdit = False

        ar.srevisiondocument = srevisiondocument
        ar.sid = sid

        If Me.WindowState = FormWindowState.Maximized Then
            ar.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ar.ShowDialog(Me)
        Me.Visible = True

        If ar.DialogResult = Windows.Forms.DialogResult.OK Then

            Me.irevisionid = ar.irevisionid
            Me.srevisionresult = ar.srevisionresult

            If ar.wasCreated = True Then
                wasCreated = True
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class