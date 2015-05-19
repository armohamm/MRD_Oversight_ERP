Public Class AgregarRevision

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
    Public isRecover As Boolean = False
    Public wasCreated As Boolean = False

    Public irevisionid As Integer = 0
    Public srevisiondocument As String = ""
    Public srevisionresult As String = ""
    Public sid As String = ""

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


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        Dim viewPermission As Boolean = False

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        If dsPermissions.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If


        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    btnGuardar.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Ver/Editar Revisions', 'OK')")
            executeSQLCommand(0, "INSERT INTO revisions (susername, susersession, srevisioncomment, bread, imessagedate, srevisioncommenttime, iupdatedate, supdatetime, supdateusername) VALUES ('SYSTEM', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', 'SYSTEM')")

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")

            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

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


    Private Sub AgregarRevision_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Revisión " & irevisionid & "', 'OK')")

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarRevision_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarRevision_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        cmbResultado.DataSource = getSQLQueryAsDataTable(0, "SELECT irevisionresultid, srevisionresult FROM revisionresults")
        cmbResultado.DisplayMember = "srevisionresult"
        cmbResultado.ValueMember = "irevisionresultid"
        cmbResultado.SelectedIndex = -1

        If isEdit = False Then

            txtDescripcion.Text = ""

        Else

            Dim dsRevision As DataSet
            dsRevision = getSQLQueryAsDataset(0, "SELECT * FROM revisions WHERE irevisionid = " & irevisionid)

            Try

                If dsRevision.Tables(0).Rows.Count > 0 Then

                    txtDescripcion.Text = dsRevision.Tables(0).Rows(0).Item("srevisioncomment")
                    srevisiondocument = dsRevision.Tables(0).Rows(0).Item("srevisiondocument")
                    sid = dsRevision.Tables(0).Rows(0).Item("sid")
                    cmbResultado.SelectedValue = dsRevision.Tables(0).Rows(0).Item("irevisionresultid")

                End If

            Catch ex As Exception

            End Try

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Revisión " & irevisionid & "', 'OK')")

        txtDescripcion.Select()
        txtDescripcion.Focus()
        txtDescripcion.SelectionStart() = txtDescripcion.Text.Length

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


    Private Sub txtDescripcion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDescripcion.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDescripcion.Text.Contains(arrayCaractProhib(carp)) Then
                txtDescripcion.Text = txtDescripcion.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDescripcion.Select(txtDescripcion.Text.Length, 0)
        End If

        txtDescripcion.Text = txtDescripcion.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Function validaRevision(ByVal silent As Boolean) As Boolean

        txtDescripcion.Text = txtDescripcion.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If cmbResultado.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías escoger un Resultado para tu Revision?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbResultado.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'irevisionid = 0
        'srevisiondocument = ""
        'srevisioncomment = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaRevision(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If MsgBox("Guardar esta revisión constituye una firma válida tanto como una firma fisica autografa a cualquier documento. ¿Estás seguro de tu decisión?", MsgBoxStyle.YesNo, "Responsiva") = MsgBoxResult.No Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()


        If irevisionid = 0 Then

            irevisionid = getSQLQueryAsInteger(0, "SELECT IF(MAX(irevisionid) + 1 IS NULL, 1, MAX(irevisionid) + 1) AS irevisionid FROM revisions ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            srevisionresult = cmbResultado.SelectedValue.ToString

            executeSQLCommand(0, "INSERT INTO revisions VALUES (" & irevisionid & ", '" & srevisiondocument & "', '" & sid & "', '" & txtDescripcion.Text.Replace("--", "").Replace("'", "") & "', " & cmbResultado.SelectedValue.ToString & ", " & fecha & ", '" & hora & "', '" & susername & "', " & fecha & ", '" & hora & "', '" & susername & "')")

            Dim dsIterations As DataSet

            dsIterations = getSQLQueryAsDataset(0, "SELECT * FROM revisionresults WHERE irevisionresultid < " & cmbResultado.SelectedValue.ToString)

            If dsIterations.Tables(0).Rows.Count > 0 Then

                Dim firstrevisionid As Integer = 0
                firstrevisionid = irevisionid

                For i = 0 To dsIterations.Tables(0).Rows.Count - 1

                    irevisionid = getSQLQueryAsInteger(0, "SELECT IF(MAX(irevisionid) + 1 IS NULL, 1, MAX(irevisionid) + 1) AS irevisionid FROM revisions ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    executeSQLCommand(0, "INSERT INTO revisions VALUES (" & irevisionid & ", '" & srevisiondocument & "', '" & sid & "', '" & txtDescripcion.Text.Replace("--", "").Replace("'", "") & "', " & dsIterations.Tables(0).Rows(i).Item("irevisionresultid") & ", " & fecha & ", '" & hora & "', '" & susername & "', " & fecha & ", '" & hora & "', '" & susername & "')")

                Next i

                irevisionid = firstrevisionid

            End If

        Else

            srevisionresult = cmbResultado.SelectedValue.ToString

            executeSQLCommand(0, "UPDATE revisions SET irevisionresultid = " & cmbResultado.SelectedValue.ToString & ", srevisioncomment = '" & txtDescripcion.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE irevisionid = '" & irevisionid & "'")

        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Revisión " & irevisionid & "', 'OK')")

        wasCreated = True

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class