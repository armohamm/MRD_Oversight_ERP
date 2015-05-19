Public Class AgregarUnidad

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public isUnit As Boolean = False
    Public isEdit As Boolean = False
    Public isRecover As Boolean = False
    Public wasCreated As Boolean = False

    Public sunit1 As String = ""
    Public sunit2 As String = ""
    Public dfactor As Double = 1.0

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

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar/Modificar Unidad', 'OK')")

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


    Private Sub AgregarUnidad_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM transformationunits " & _
        "WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu WHERE transformationunits.soriginunit = ttu.soriginunit AND transformationunits.sdestinationunit = ttu.sdestinationunit) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(ttu.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu JOIN transformationunits tu ON ttu.soriginunit = tu.soriginunit AND ttu.sdestinationunit = tu.sdestinationunit WHERE STR_TO_DATE(CONCAT(ttu.iupdatedate, ' ', ttu.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(tu.iupdatedate, ' ', tu.supdatetime), '%Y%c%d %T') ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu " & _
        "WHERE NOT EXISTS (SELECT * FROM transformationunits tu WHERE tu.soriginunit = ttu.soriginunit AND tu.sdestinationunit = ttu.sdestinationunit AND tu.soriginunit = '" & sunit1 & "' AND tu.sdestinationunit = '" & sunit2 & "') ")

        If conteo1 + conteo2 + conteo3 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaUnidad(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Esta Unidad está incompleta. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesUnitIsOpen As Integer = 1

            timesUnitIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Unit" & sunit1 & "'")

            If timesUnitIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto la misma Unidad. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Unidad?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesUnitIsOpen > 1 And isEdit = False Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario esta ingresando esta misma Unidad o Conversión. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Unidad?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            End If


            Dim queries(4) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM transformationunits " & _
            "WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "' AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu WHERE transformationunits.soriginunit = ttu.soriginunit AND transformationunits.sdestinationunit = ttu.sdestinationunit) "

            queries(1) = "" & _
            "UPDATE transformationunits tu JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu ON ttu.soriginunit = tu.soriginunit AND ttu.sdestinationunit = tu.sdestinationunit SET tu.iupdatedate = ttu.iupdatedate, tu.supdatetime = ttu.supdatetime, tu.supdateusername = ttu.supdateusername, tu.soriginunit = ttu.soriginunit, tu.sdestinationunit = ttu.sdestinationunit WHERE STR_TO_DATE(CONCAT(ttu.iupdatedate, ' ', ttu.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(tu.iupdatedate, ' ', tu.supdatetime), '%Y%c%d %T') "

            queries(2) = "" & _
            "INSERT INTO transformationunits " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu " & _
            "WHERE NOT EXISTS (SELECT * FROM transformationunits tu WHERE tu.soriginunit = ttu.soriginunit AND tu.sdestinationunit = ttu.sdestinationunit AND tu.soriginunit = '" & sunit1 & "' AND tu.sdestinationunit = '" & sunit2 & "') "

            queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Unidad de Transformación " & txtUnidadOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtUnidadDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

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

        Dim queriesDelete(4) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UnitToUnit"
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2
        queriesDelete(2) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Unidad de Transformación " & txtUnidadOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtUnidadDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"
        'queriesDelete(3) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Unidad', '" & sunit1 & "', '" & sunit2 & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarUnidad_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarUnidad_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesUnitIsOpen As Integer = 0

        timesUnitIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Unit" & sunit1 & "'")

        If timesUnitIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto la misma Unidad. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo la Unidad?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

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

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ( `soriginunit` varchar(150) CHARACTER SET latin1 NOT NULL, `sdestinationunit` varchar(150) CHARACTER SET latin1 NOT NULL, `factor` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`soriginunit`,`sdestinationunit`), KEY `originunit` (`soriginunit`), KEY `destinationunit` (`sdestinationunit`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        If isEdit = False Then

            txtUnidadOrigen.Text = ""
            txtUnidadDestino.Text = ""
            txtFactorConversion.Text = "1.00"

        Else

            If isRecover = False Then

                Dim queriesInsert(1) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " SELECT * FROM transformationunits WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "'"

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsUnidad As DataSet
            dsUnidad = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "'")

            Try

                If dsUnidad.Tables(0).Rows.Count > 0 Then

                    txtUnidadOrigen.Text = dsUnidad.Tables(0).Rows(0).Item("soriginunit")
                    txtUnidadDestino.Text = dsUnidad.Tables(0).Rows(0).Item("sdestinationunit")
                    txtFactorConversion.Text = dsUnidad.Tables(0).Rows(0).Item("factor")

                End If

            Catch ex As Exception

            End Try

            txtUnidadOrigen.Enabled = False
            txtUnidadDestino.Enabled = False

        End If

        If isUnit = True Then

            txtUnidadDestino.Visible = False
            txtFactorConversion.Visible = False

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Unidad de Transformación " & txtUnidadOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtUnidadDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Unidad', '" & sunit1 & "', '" & sunit2 & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        txtUnidadOrigen.Select()
        txtUnidadOrigen.Focus()
        txtUnidadOrigen.SelectionStart() = txtUnidadOrigen.Text.Length

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


    Private Sub txtUnidadOrigen_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUnidadOrigen.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtUnidadOrigen.Text.Contains(arrayCaractProhib(carp)) Then
                txtUnidadOrigen.Text = txtUnidadOrigen.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtUnidadOrigen.Select(txtUnidadOrigen.Text.Length, 0)
        End If

        txtUnidadOrigen.Text = txtUnidadOrigen.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtUnidadOrigen_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnidadOrigen.TextChanged

        If isUnit = True Then

            txtUnidadDestino.Text = txtUnidadOrigen.Text
            txtFactorConversion.Text = "1.00"

        End If

    End Sub


    Private Sub txtUnidadDestino_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUnidadDestino.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtUnidadDestino.Text.Contains(arrayCaractProhib(carp)) Then
                txtUnidadDestino.Text = txtUnidadDestino.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtUnidadDestino.Select(txtUnidadDestino.Text.Length, 0)
        End If

        txtUnidadDestino.Text = txtUnidadDestino.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtUnidadDestino_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUnidadDestino.TextChanged

        If txtUnidadOrigen.Text.Length > 0 And txtUnidadDestino.Text.Length > 0 Then

            lblAvailability.Visible = True

            If isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadOrigen.Text.Replace("--", "") & "' AND sdestinationunit = '" & txtUnidadDestino.Text.Replace("--", "") & "'") > 0 Then

                lblAvailability.Text = "No Disponible"
                lblAvailability.ForeColor = Color.Red

            ElseIf isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM transformationunits WHERE soriginunit = '" & txtUnidadOrigen.Text.Replace("--", "") & "' AND sdestinationunit = '" & txtUnidadDestino.Text.Replace("--", "") & "'") = 0 Then

                lblAvailability.Text = "Conversión Disponible"
                lblAvailability.ForeColor = Color.ForestGreen

            End If

        Else

            lblAvailability.Visible = False

        End If

    End Sub


    Private Sub txtFactorConversion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFactorConversion.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtFactorConversion.Text.Contains(arrayCaractProhib(carp)) Then
                txtFactorConversion.Text = txtFactorConversion.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtFactorConversion.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtFactorConversion.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtFactorConversion.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtFactorConversion.Text.Length

                    If longitud > (lugar + 1) Then
                        txtFactorConversion.Text = txtFactorConversion.Text.Substring(0, lugar) & txtFactorConversion.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtFactorConversion.Text = txtFactorConversion.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtFactorConversion.Select(txtFactorConversion.Text.Length, 0)
        End If

        txtFactorConversion.Text = txtFactorConversion.Text.Replace("--", "").Replace("'", "")
        txtFactorConversion.Text = txtFactorConversion.Text.Trim

    End Sub


    Private Function validaUnidad(ByVal silent As Boolean) As Boolean

        txtUnidadOrigen.Text = txtUnidadOrigen.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtUnidadDestino.Text = txtUnidadDestino.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtFactorConversion.Text = txtFactorConversion.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If txtUnidadOrigen.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una Unidad de Origen?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtUnidadOrigen.Select()
            txtUnidadOrigen.Focus()
            Return False

        End If

        If txtUnidadDestino.Text = "" Then

            If silent = False Then
                MsgBox("¿Podrías poner una Unidad de Destino?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtUnidadDestino.Select()
            txtUnidadDestino.Focus()
            Return False

        End If

        If isEdit = False And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM transformationunits WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "'") > 0 Then

            If silent = False Then
                MsgBox("Ya existe esta Conversión en el sistema, por lo que no puedo permitirte ingresarla nuevamente. Intenta editando la conversión directamente y no creando una nueva.", MsgBoxStyle.OkOnly, "Error")
            End If

            txtUnidadOrigen.Select()
            txtUnidadOrigen.Focus()
            Return False

        End If

        Dim cantidad As Double = 0.0

        Try
            cantidad = CDbl(txtFactorConversion.Text)
        Catch ex As Exception

        End Try

        If cantidad = 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías poner el factor de conversión? Es decir, cuantas unidades de Origen necesito para 1 unidad de Destino", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            txtFactorConversion.Select()
            txtFactorConversion.Focus()
            Return False

        End If

        Return True

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'sunit1 = ""
        'sunit2 = ""
        'dfactor = 0.0

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaUnidad(False) = False Then
            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim timesUnitIsOpen As Integer = 1

        timesUnitIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Unit" & sunit1 & "'")

        If timesUnitIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto la misma Unidad. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Unidad?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesUnitIsOpen > 1 And isEdit = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario esta ingresando esta misma Unidad o Conversión. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando la Unidad?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        If sunit1 = "" Then

            sunit1 = txtUnidadOrigen.Text
            sunit2 = txtUnidadDestino.Text

            Dim cantidad As Double = 1.0

            Try
                cantidad = CDbl(txtFactorConversion.Text)
            Catch ex As Exception

            End Try

            dfactor = cantidad

            Dim queriesCreation(3) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "UnitToUnit"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ( `soriginunit` varchar(150) CHARACTER SET latin1 NOT NULL, `sdestinationunit` varchar(150) CHARACTER SET latin1 NOT NULL, `factor` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`soriginunit`,`sdestinationunit`), KEY `originunit` (`soriginunit`), KEY `destinationunit` (`sdestinationunit`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"
            queriesCreation(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " VALUES ('" & txtUnidadOrigen.Text.Replace("--", "").Replace("'", "") & "', '" & txtUnidadDestino.Text.Replace("--", "").Replace("'", "") & "', " & txtFactorConversion.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " SET soriginunit = '" & txtUnidadOrigen.Text.Replace("--", "").Replace("'", "") & "', sdestinationunit = '" & txtUnidadDestino.Text.Replace("--", "").Replace("'", "") & ", factor = " & txtFactorConversion.Text & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "'")

            Dim cantidad As Double = 1.0

            Try
                cantidad = CDbl(txtFactorConversion.Text)
            Catch ex As Exception

            End Try

            dfactor = cantidad

        End If

        Dim queries(4) As String

        If isEdit = True Then

        Else

        End If

        queries(0) = "" & _
        "DELETE " & _
        "FROM transformationunits " & _
        "WHERE soriginunit = '" & sunit1 & "' AND sdestinationunit = '" & sunit2 & "' AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu WHERE transformationunits.soriginunit = ttu.soriginunit AND transformationunits.sdestinationunit = ttu.sdestinationunit) "

        queries(1) = "" & _
        "UPDATE transformationunits tu JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu ON ttu.soriginunit = tu.soriginunit AND ttu.sdestinationunit = tu.sdestinationunit SET tu.iupdatedate = ttu.iupdatedate, tu.supdatetime = ttu.supdatetime, tu.supdateusername = ttu.supdateusername, tu.soriginunit = ttu.soriginunit, tu.sdestinationunit = ttu.sdestinationunit WHERE STR_TO_DATE(CONCAT(ttu.iupdatedate, ' ', ttu.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(tu.iupdatedate, ' ', tu.supdatetime), '%Y%c%d %T') "

        queries(2) = "" & _
        "INSERT INTO transformationunits " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Unit" & sunit1 & "ToUnit" & sunit2 & " ttu " & _
        "WHERE NOT EXISTS (SELECT * FROM transformationunits tu WHERE tu.soriginunit = ttu.soriginunit AND tu.sdestinationunit = ttu.sdestinationunit AND tu.soriginunit = '" & sunit1 & "' AND tu.sdestinationunit = '" & sunit2 & "') "

        queries(3) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Unidad de Transformación " & txtUnidadOrigen.Text.Replace("'", "").Replace("--", "") & " a " & txtUnidadDestino.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub



End Class