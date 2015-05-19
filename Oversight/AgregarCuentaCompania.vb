Public Class AgregarCuentaCompania

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

    Public iaccountid As Integer = 0
    Public ibankid As Integer = 0
    Public scompanyaccountname As String = ""

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

                If permission = "Modificar" Then
                    btnGuardar.Enabled = True
                End If

                If permission = "Ver Bancos" Then
                    openPermission = True
                    btnBancos.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Cuenta de la Compañía', 'OK')")

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


    Private Sub AgregarCuentaCompania_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM companyaccounts " & _
        "WHERE iaccountid = '" & iaccountid & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc WHERE companyaccounts.iaccountid = tclc.iaccountid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tclc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc JOIN companyaccounts clc ON tclc.iaccountid = clc.iaccountid WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM companyaccounts clc WHERE tclc.iaccountid = clc.iaccountid AND clc.iaccountid = '" & iaccountid & "') ")

        If conteo1 + conteo2 + conteo3 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaCuentaCompania(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Cuenta de la Compañia está incompleta. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesCompanyAccountIsOpen As Integer = 1

            timesCompanyAccountIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CompanyAccount" & iaccountid & "'")

            If timesCompanyAccountIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Cuenta de la Compañia. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Cuenta de la Compania?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesCompanyAccountIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CompanyAccount" & iaccountid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(4) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid + newIdAddition
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid + newIdAddition & " SET iaccountid = " & iaccountid + newIdAddition & " WHERE iaccountid = " & iaccountid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    iaccountid = iaccountid + newIdAddition
                End If

            End If


            Dim queries(4) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM companyaccounts " & _
            "WHERE iaccountid = '" & iaccountid & "' AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc WHERE companyaccounts.iaccountid = tclc.iaccountid) "

            queries(1) = "" & _
            "UPDATE companyaccounts clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc ON tclc.iaccountid = clc.iaccountid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ibankid = tclc.ibankid, clc.scompanyaccountname = tclc.scompanyaccountname WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

            queries(2) = "" & _
            "INSERT INTO companyaccounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc " & _
            "WHERE NOT EXISTS (SELECT * FROM companyaccounts clc WHERE tclc.iaccountid = clc.iaccountid AND clc.iaccountid = '" & iaccountid & "') "

            queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Cuenta de la Compania " & iaccountid & " : " & txtDescripcion.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub
            End If

            wasCreated = True

        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If



        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim queriesDelete(3) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid
        queriesDelete(1) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Cuenta de la Compania " & iaccountid & " : " & txtDescripcion.Text.Replace("'", "").Replace("--", "") & "', 'OK')"
        queriesDelete(2) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'CompanyAccount', 'Cuenta de la Compañía', '" & iaccountid & "', '" & txtDescripcion.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarCuentaCompania_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarCuentaCompania_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesCompanyAccountIsOpen As Integer = 0

        timesCompanyAccountIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CompanyAccount" & iaccountid & "'")

        If timesCompanyAccountIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Cuenta de la Compañia. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo esta Cuenta de la Compañia?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        If isRecover = False Then

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " ( `iaccountid` int(11) NOT NULL AUTO_INCREMENT, `ibankid` int(11) NOT NULL, `scompanyaccountname` varchar(1000) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`iaccountid`), KEY `bankid` (`ibankid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        cmbBancos.DataSource = getSQLQueryAsDataTable(0, "SELECT ibankid, sbankname FROM banks")
        cmbBancos.DisplayMember = "sbankname"
        cmbBancos.ValueMember = "ibankid"
        cmbBancos.SelectedIndex = -1

        If isEdit = False Then

            txtDescripcion.Text = ""

        Else

            If isRecover = False Then

                Dim queriesInsert(1) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " SELECT * FROM companyaccounts WHERE iaccountid = '" & iaccountid & "'"

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsCuentaCompania As DataSet
            dsCuentaCompania = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " WHERE iaccountid = '" & iaccountid & "'")

            Try

                If dsCuentaCompania.Tables(0).Rows.Count > 0 Then

                    txtDescripcion.Text = dsCuentaCompania.Tables(0).Rows(0).Item("scompanyaccountname")
                    cmbBancos.SelectedValue = dsCuentaCompania.Tables(0).Rows(0).Item("ibankid")

                End If

            Catch ex As Exception

            End Try


        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Cuenta de la Compania " & iaccountid & " : " & txtDescripcion.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'CompanyAccount', 'Cuenta de la Compañía', '" & iaccountid & "', '" & txtDescripcion.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

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


    Private Sub btnBancos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBancos.Click

        Dim bb As New BuscaBancos
        bb.susername = susername
        bb.bactive = bactive
        bb.bonline = bonline
        bb.suserfullname = suserfullname
        bb.suseremail = suseremail
        bb.susersession = susersession
        bb.susermachinename = susermachinename
        bb.suserip = suserip

        bb.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bb.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bb.ShowDialog(Me)
        Me.Visible = True

        If bb.DialogResult = Windows.Forms.DialogResult.OK Then

            cmbBancos.DataSource = getSQLQueryAsDataTable(0, "SELECT ibankid, sbankname FROM banks")
            cmbBancos.DisplayMember = "sbankname"
            cmbBancos.ValueMember = "ibankid"
            cmbBancos.SelectedIndex = -1

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

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


    Private Function validaCuentaCompania(ByVal silent As Boolean) As Boolean

        txtDescripcion.Text = txtDescripcion.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If cmbBancos.SelectedIndex = -1 Then

            If silent = False Then
                MsgBox("¿Podrías escoger un Banco?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            cmbBancos.Focus()
            Return False

        End If

        If txtDescripcion.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una Descripción a la Cuenta de la Compañía?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtDescripcion.Select()
            txtDescripcion.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'iaccountid = 0
        'ibankid = 0
        'scompanyaccountname = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaCuentaCompania(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesCompanyAccountIsOpen As Integer = 1

        timesCompanyAccountIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CompanyAccount" & iaccountid & "'")

        If timesCompanyAccountIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Cuenta de la Compañia. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Cuenta de la Compania?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesCompanyAccountIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%CompanyAccount" & iaccountid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(4) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid + newIdAddition
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid + newIdAddition & " SET iaccountid = " & iaccountid + newIdAddition & " WHERE iaccountid = " & iaccountid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                iaccountid = iaccountid + newIdAddition
            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        If iaccountid = 0 Then

            Dim queriesCreation(2) As String

            executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount0")

            iaccountid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iaccountid) + 1 IS NULL, 1, MAX(iaccountid) + 1) AS iaccountid FROM companyaccounts ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
            scompanyaccountname = txtDescripcion.Text.Replace("--", "").Replace("'", "")
            ibankid = cmbBancos.SelectedValue.ToString

            queriesCreation(0) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " ( `iaccountid` int(11) NOT NULL AUTO_INCREMENT, `ibankid` int(11) NOT NULL, `scompanyaccountname` varchar(1000) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) NOT NULL, `supdateusername` varchar(100) NOT NULL, PRIMARY KEY (`iaccountid`), KEY `bankid` (`ibankid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1"
            queriesCreation(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " VALUES (" & iaccountid & ", " & cmbBancos.SelectedValue.ToString & ", '" & txtDescripcion.Text.Replace("--", "").Replace("'", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " SET ibankid = " & cmbBancos.SelectedValue.ToString & ", scompanyaccountname = '" & txtDescripcion.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iaccountid = '" & iaccountid & "'")
            scompanyaccountname = txtDescripcion.Text.Replace("--", "").Replace("'", "")
            ibankid = cmbBancos.SelectedValue.ToString

        End If

        Dim queries(4) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM companyaccounts " & _
        "WHERE iaccountid = '" & iaccountid & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc WHERE companyaccounts.iaccountid = tclc.iaccountid) "

        queries(1) = "" & _
        "UPDATE companyaccounts clc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc ON tclc.iaccountid = clc.iaccountid SET clc.iupdatedate = tclc.iupdatedate, clc.supdatetime = tclc.supdatetime, clc.supdateusername = tclc.supdateusername, clc.ibankid = tclc.ibankid, clc.scompanyaccountname = tclc.scompanyaccountname WHERE STR_TO_DATE(CONCAT(tclc.iupdatedate, ' ', tclc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(clc.iupdatedate, ' ', clc.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO companyaccounts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "CompanyAccount" & iaccountid & " tclc " & _
        "WHERE NOT EXISTS (SELECT * FROM companyaccounts clc WHERE tclc.iaccountid = clc.iaccountid AND clc.iaccountid = '" & iaccountid & "') "

        queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Cuenta de la Compania " & iaccountid & " : " & txtDescripcion.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            wasCreated = True
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class