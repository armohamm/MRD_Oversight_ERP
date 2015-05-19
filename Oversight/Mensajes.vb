Public Class Mensajes

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public imessageid As Integer = 0

    Public bAlreadyOpen As Boolean = False
    Public bShowAllMessages As Boolean = False

    Private openPermission As Boolean = False
    Private newPermission As Boolean = False
    Private markAsReadPermission As Boolean = False
    Private markAsUnreadPermission As Boolean = False


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
                    btnNuevo.Enabled = True
                End If

                If permission = "Marcar como leído" Then
                    btnLeido.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Mensajes', 'OK')")

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


    Private Sub Mensajes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Mensajes', 'OK')")

        bAlreadyOpen = False

    End Sub


    Private Sub Mensajes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub formatReadColumn()

        If dgvMensajes.RowCount < 1 Then
            Exit Sub
        End If

        For i = 0 To dgvMensajes.RowCount - 1

            If dgvMensajes.Rows(i).Cells(5).ToString = "0" Then
                If dgvMensajes.ColumnCount > 5 Then
                    dgvMensajes.Rows(i).Cells(6).Value() = ""
                End If
            Else
                Continue For
            End If

        Next i

    End Sub


    Private Sub Mensajes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()

        setControlsByPermissions(Me.Name, susername)

        Dim queryMessages As String = ""

        If bShowAllMessages = True Then

            queryMessages = "" & _
            "SELECT * FROM (" & _
            "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
            "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
            "bread, m.iupdatedatetime AS 'Fecha Hora Leido' " & _
            "FROM messages m " & _
            "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
            "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
            "WHERE m.susername = '" & susername & "' " & _
            "ORDER BY 5 DESC " & _
            " ) tmpA "

        Else

            queryMessages = "" & _
            "SELECT * FROM (" & _
            "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
            "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
            "bread " & _
            "FROM messages m " & _
            "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
            "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
            "WHERE m.susername = '" & susername & "' AND m.bread = 0 " & _
            "ORDER BY 5 DESC " & _
            " ) tmpA "

        End If

        setDataGridView(dgvMensajes, queryMessages, True)

        dgvMensajes.Columns(0).Visible = False
        dgvMensajes.Columns(5).Visible = False

        dgvMensajes.Columns(1).Width = 100
        dgvMensajes.Columns(2).Width = 200
        dgvMensajes.Columns(3).Width = 70
        dgvMensajes.Columns(4).Width = 130

        formatReadColumn()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Mensajes', 'OK')")

        bAlreadyOpen = True

        Timer1.Start()

        dgvMensajes.Select()

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


    Private Sub dgvMensajes_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMensajes.CellClick

        Try

            imessageid = CInt(dgvMensajes.Rows(e.RowIndex).Cells(0).Value)

        Catch ex As Exception

            imessageid = 0

        End Try

    End Sub


    Private Sub dgvMensajes_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMensajes.CellContentClick

        Try

            imessageid = CInt(dgvMensajes.Rows(e.RowIndex).Cells(0).Value)

        Catch ex As Exception

            imessageid = 0

        End Try

    End Sub


    Private Sub dgvMensajes_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvMensajes.DataError

    End Sub


    Private Sub dgvMensajes_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMensajes.SelectionChanged

        Try

            imessageid = CInt(dgvMensajes.CurrentRow.Cells(0).Value)

        Catch ex As Exception

            imessageid = 0

        End Try

    End Sub


    Private Sub btnLeido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeido.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim tmpselectedmessageid As Integer = 0
        Try
            tmpselectedmessageid = CInt(dgvMensajes.CurrentRow.Cells(0).Value)
        Catch ex As Exception

        End Try

        executeSQLCommand(0, "UPDATE messages SET bread = 1, iupdatedatetime = NOW() WHERE imessageid = " & tmpselectedmessageid)

        Dim queryMessages As String = ""

        If bShowAllMessages = True Then

            queryMessages = "" & _
            "SELECT * FROM (" & _
            "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
            "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
            "bread, m.iupdatedatetime AS 'Fecha Hora Leido' " & _
            "FROM messages m " & _
            "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
            "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
            "WHERE m.susername = '" & susername & "' " & _
            "ORDER BY 5 DESC " & _
            " ) tmpA "

        Else

            queryMessages = "" & _
            "SELECT * FROM (" & _
            "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
            "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
            "bread " & _
            "FROM messages m " & _
            "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
            "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
            "WHERE m.susername = '" & susername & "' AND m.bread = 0 " & _
            "ORDER BY 5 DESC " & _
            " ) tmpA "

        End If

        setDataGridView(dgvMensajes, queryMessages, True)

        dgvMensajes.Columns(0).Visible = False
        dgvMensajes.Columns(5).Visible = False

        dgvMensajes.Columns(1).Width = 100
        dgvMensajes.Columns(2).Width = 200
        dgvMensajes.Columns(3).Width = 70
        dgvMensajes.Columns(4).Width = 130



        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Marco como leido el mensaje " & tmpselectedmessageid & "', 'OK')")

        dgvMensajes.Select()

        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim am As New AgregarMensaje
        am.susername = susername
        am.bactive = bactive
        am.bonline = bonline
        am.suserfullname = suserfullname
        am.suseremail = suseremail
        am.susersession = susersession
        am.susermachinename = susermachinename
        am.suserip = suserip

        am.isEdit = False

        Me.Visible = False
        am.ShowDialog(Me)
        Me.Visible = True

        If am.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queryMessages As String = ""

            If bShowAllMessages = True Then

                queryMessages = "" & _
                "SELECT * FROM (" & _
                "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
                "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
                "bread, m.iupdatedatetime AS 'Fecha Hora Leido' " & _
                "FROM messages m " & _
                "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
                "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
                "WHERE m.susername = '" & susername & "' " & _
                "ORDER BY 5 DESC " & _
                " ) tmpA "

            Else

                queryMessages = "" & _
                "SELECT * FROM (" & _
                "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
                "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
                "bread " & _
                "FROM messages m " & _
                "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
                "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
                "WHERE m.susername = '" & susername & "' AND m.bread = 0 " & _
                "ORDER BY 5 DESC " & _
                " ) tmpA "

            End If

            setDataGridView(dgvMensajes, queryMessages, True)

            dgvMensajes.Columns(0).Visible = False
            dgvMensajes.Columns(5).Visible = False

            dgvMensajes.Columns(1).Width = 100
            dgvMensajes.Columns(2).Width = 200
            dgvMensajes.Columns(3).Width = 70
            dgvMensajes.Columns(4).Width = 130


            dgvMensajes.Select()

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Dim queryMessages As String = ""

        If bShowAllMessages = True Then

            queryMessages = "" & _
            "SELECT * FROM (" & _
            "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
            "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
            "bread, m.iupdatedatetime AS 'Fecha Hora Leido' " & _
            "FROM messages m " & _
            "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
            "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
            "WHERE m.susername = '" & susername & "' " & _
            "ORDER BY 5 DESC " & _
            " ) tmpA "

        Else

            queryMessages = "" & _
            "SELECT * FROM (" & _
            "SELECT m.imessageid AS 'ID', IF(p.speoplefullname IS NULL, m.smessagecreatorusername, p.speoplefullname) AS 'Remitente', " & _
            "m.smessage AS 'Mensaje', m.susersession AS 'Sesion', m.imessagedatetime AS 'Fecha Hora del Mensaje', " & _
            "bread " & _
            "FROM messages m " & _
            "LEFT JOIN users u ON m.smessagecreatorusername = u.susername " & _
            "LEFT JOIN people p ON u.ipeopleid = p.ipeopleid " & _
            "WHERE m.susername = '" & susername & "' AND m.bread = 0 " & _
            "ORDER BY 5 DESC " & _
            " ) tmpA "

        End If

        setDataGridView(dgvMensajes, queryMessages, True)

        dgvMensajes.Columns(0).Visible = False
        dgvMensajes.Columns(5).Visible = False

        dgvMensajes.Columns(1).Width = 100
        dgvMensajes.Columns(2).Width = 200
        dgvMensajes.Columns(3).Width = 70
        dgvMensajes.Columns(4).Width = 130


    End Sub

End Class