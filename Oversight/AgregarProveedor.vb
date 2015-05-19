Public Class AgregarProveedor

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

    Private isFormReadyForAction As Boolean = False

    Public isupplierid As Integer = 0
    Public ssuppliername As String = ""
    Public ssupplierplace As String = ""

    Private iselectedphoneid As Integer = 0
    Private sselectedphone As String = ""
    Private sselectedphonetype As String = ""

    Private ipeopleid As Integer = 0
    Private sselectedpeoplefullname As String = ""

    Private WithEvents txtNumeroDgvTelefonos As TextBox
    Private WithEvents txtTipoNumeroDgvTelefonos As TextBox

    Private txtNumeroDgvTelefonos_OldText As String = ""
    Private txtTipoNumeroDgvTelefonos_OldText As String = ""

    Private WithEvents txtNombreDgvPersonas As TextBox
    Private txtNombreDgvPersonas_OldText As String = ""

    Private openPermission As Boolean = False

    Private addPhonePermission As Boolean = False
    Private modifyPhonePermission As Boolean = False
    Private deletePhonePermission As Boolean = False

    Private openPeoplePermission As Boolean = False
    Private addPeoplePermission As Boolean = False
    Private insertPeoplePermission As Boolean = False
    Private modifyPeoplePermission As Boolean = False
    Private deletePeoplePermission As Boolean = False

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

                If permission = "Abrir Persona" Then
                    btnCopiarPersona.Enabled = True
                End If

                If permission = "Copiar Datos" Then
                    btnCopiarDatos.Enabled = True
                End If

                If permission = "Ver Telefonos" Then
                    dgvTelefonos.Visible = True
                End If

                If permission = "Agregar Telefono" Then
                    addPhonePermission = True
                    btnNuevoTelefono.Enabled = True
                End If

                If permission = "Modificar Telefono" Then
                    modifyPhonePermission = True
                End If

                If permission = "Eliminar Telefono" Then
                    deletePhonePermission = True
                    btnEliminarTelefono.Enabled = True
                End If

                If permission = "Ver Personas" Then
                    dgvPersonas.Visible = True
                End If

                If permission = "Agregar Persona" Then
                    openPeoplePermission = True
                    addPeoplePermission = True
                    btnNuevaPersona.Enabled = True
                End If

                If permission = "Insertar Persona" Then
                    openPeoplePermission = True
                    insertPeoplePermission = True
                    btnInsertarPersona.Enabled = True
                End If

                If permission = "Eliminar Persona" Then
                    openPeoplePermission = True
                    deletePeoplePermission = True
                    btnEliminarPersona.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Proveedor', 'OK')")

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


    Private Sub AgregarProveedor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0
        Dim conteo7 As Integer = 0
        Dim conteo8 As Integer = 0
        Dim conteo9 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM suppliers " & _
        "WHERE isupplierid = " & isupplierid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts WHERE suppliers.isupplierid = ts.isupplierid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierphonenumbers " & _
        "WHERE isupplierid = " & isupplierid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn WHERE supplierphonenumbers.isupplierid = tspn.isupplierid AND supplierphonenumbers.isupplierphonenumberid = tspn.isupplierphonenumberid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM suppliercontacts " & _
        "WHERE isupplierid = " & isupplierid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc WHERE suppliercontacts.isupplierid = tsc.isupplierid AND suppliercontacts.ipeopleid = tsc.ipeopleid) ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(ts.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts JOIN suppliers s ON ts.isupplierid = s.isupplierid WHERE STR_TO_DATE(CONCAT(ts.iupdatedate, ' ', ts.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(s.iupdatedate, ' ', s.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tspn.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn JOIN supplierphonenumbers spn ON tspn.isupplierid = spn.isupplierid AND tspn.isupplierphonenumberid = spn.isupplierphonenumberid WHERE STR_TO_DATE(CONCAT(tspn.iupdatedate, ' ', tspn.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(spn.iupdatedate, ' ', spn.supdatetime), '%Y%c%d %T') ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc JOIN suppliercontacts sc ON tsc.isupplierid = sc.isupplierid AND tsc.ipeopleid = sc.ipeopleid WHERE STR_TO_DATE(CONCAT(tsc.iupdatedate, ' ', tsc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sc.iupdatedate, ' ', sc.supdatetime), '%Y%c%d %T') ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts " & _
        "WHERE NOT EXISTS (SELECT * FROM suppliers s WHERE s.isupplierid = ts.isupplierid AND s.isupplierid = " & isupplierid & ") ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierphonenumbers spn WHERE spn.ipeopleid = tspn.ipeopleid AND spn.isuppplierphonenumberid = tspn.isupplierphonenumberid AND spn.isupplierid = " & isupplierid & ") ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc " & _
        "WHERE NOT EXISTS (SELECT * FROM suppliercontacts sc WHERE sc.isupplierid = tsc.isupplierid AND sc.ipeopleid = tsc.ipeopleid AND sc.isupplierid = " & isupplierid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo9 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaProveedorCompleto(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Los datos de este Proveedor están incompletos. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then

            Dim timesSupplierIsOpen As Integer = 1

            timesSupplierIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Supplier" & isupplierid & "'")

            If timesSupplierIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Proveedor. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Proveedor?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesSupplierIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Supplier" & isupplierid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(6) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "PhoneNumbers"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "Contacts"
                queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & " SET isupplierid = " & isupplierid + newIdAddition & " WHERE isupplierid = " & isupplierid
                queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "PhoneNumbers SET isupplierid = " & isupplierid + newIdAddition & " WHERE isupplierid = " & isupplierid
                queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "Contacts SET isupplierid = " & isupplierid + newIdAddition & " WHERE isupplierid = " & isupplierid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierid = isupplierid + newIdAddition
                End If

            End If

            Dim queriesSave(10) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM suppliers " & _
            "WHERE isupplierid = " & isupplierid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts WHERE suppliers.isupplierid = ts.isupplierid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM supplierphonenumbers " & _
            "WHERE isupplierid = " & isupplierid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn WHERE supplierphonenumbers.isupplierid = tspn.isupplierid AND supplierphonenumbers.isupplierphonenumberid = tspn.isupplierphonenumberid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM suppliercontacts " & _
            "WHERE isupplierid = " & isupplierid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc WHERE suppliercontacts.isupplierid = tsc.isupplierid AND suppliercontacts.ipeopleid = tsc.ipeopleid) "

            queriesSave(3) = "" & _
            "UPDATE suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts ON ts.isupplierid = s.isupplierid SET s.iupdatedate = ts.iupdatedate, s.supdatetime = ts.supdatetime, s.supdateusername = ts.supdateusername, s.ssuppliername = ts.ssuppliername, s.ssupplierofficialname = ts.ssupplierofficialname, s.ssupplieraddress = ts.ssupplieraddress, s.ssupplierofficialaddress = ts.ssupplierofficialaddress, s.ssupplierrfc = ts.ssupplierrfc, s.ssupplieremail = ts.ssupplieremail, s.ssupplierobservations = ts.ssupplierobservations WHERE STR_TO_DATE(CONCAT(ts.iupdatedate, ' ', ts.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(s.iupdatedate, ' ', s.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE supplierphonenumbers spn JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn ON tspn.isupplierid = spn.isupplierid AND tspn.isupplierphonenumberid = spn.isupplierphonenumberid SET spn.iupdatedate = tspn.iupdatedate, spn.supdatetime = tspn.supdatetime, spn.supdateusername = tspn.supdateusername, spn.ssupplierphonenumber = tspn.ssupplierphonenumber WHERE STR_TO_DATE(CONCAT(tspn.iupdatedate, ' ', tspn.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(spn.iupdatedate, ' ', spn.supdatetime), '%Y%c%d %T') "

            queriesSave(5) = "" & _
            "UPDATE suppliercontacts sc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc ON tsc.isupplierid = sc.isupplierid AND tsc.ipeopleid = sc.ipeopleid SET sc.iupdatedate = tsc.iupdatedate, sc.supdatetime = tsc.supdatetime, sc.supdateusername = tsc.supdateusername, sc.ipeopleid = tsc.ipeopleid WHERE STR_TO_DATE(CONCAT(tsc.iupdatedate, ' ', tsc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sc.iupdatedate, ' ', sc.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO suppliers " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts " & _
            "WHERE NOT EXISTS (SELECT * FROM suppliers s WHERE s.isupplierid = ts.isupplierid AND s.isupplierid = " & isupplierid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO supplierphonenumbers " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierphonenumbers spn WHERE spn.ipeopleid = tspn.ipeopleid AND spn.isuppplierphonenumberid = tspn.isupplierphonenumberid AND spn.isupplierid = " & isupplierid & ") "

            queriesSave(8) = "" & _
            "INSERT INTO suppliercontacts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc " & _
            "WHERE NOT EXISTS (SELECT * FROM suppliercontacts sc WHERE sc.isupplierid = tsc.isupplierid AND sc.ipeopleid = tsc.ipeopleid AND sc.isupplierid = " & isupplierid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Proveedor " & isupplierid & ": " & txtNombreComercial.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
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

        Dim queriesDelete(5) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
        queriesDelete(3) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Proveedor " & isupplierid & ": " & txtNombreComercial.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        queriesDelete(4) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Supplier', 'Proveedor', '" & isupplierid & "', '" & txtNombreComercial.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarProveedor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarProveedor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesSupplierIsOpen As Integer = 0

        timesSupplierIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Supplier" & isupplierid & "'")

        If timesSupplierIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Proveedor. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Proveedor?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        If isRecover = False Then

            Dim queriesCreation(6) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If isEdit = False Then

            txtNombreComercial.Text = ""
            txtDireccion.Text = "Tuxtla Gutierrez, Chiapas"
            txtEmail.Text = ""
            txtObservaciones.Text = ""

            txtNombreFiscal.Text = ""
            txtDireccionFiscal.Text = "Tuxtla Gutierrez, Chiapas"
            txtRFC.Text = ""

            Dim queriesInsert(3) As String

            queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SELECT * FROM suppliers WHERE isupplierid = " & isupplierid
            queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers SELECT * FROM supplierphonenumbers WHERE isupplierid = " & isupplierid
            queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts SELECT * FROM suppliercontacts WHERE isupplierid = " & isupplierid

            executeTransactedSQLCommand(0, queriesInsert)

            'btnNuevoTelefono.Enabled = False
            'btnEliminarTelefono.Enabled = False

            'btnNuevaPersona.Enabled = False
            'btnInsertarPersona.Enabled = False
            'btnEliminarPersona.Enabled = False

        Else

            If isRecover = False Then

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SELECT * FROM suppliers WHERE isupplierid = " & isupplierid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers SELECT * FROM supplierphonenumbers WHERE isupplierid = " & isupplierid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts SELECT * FROM suppliercontacts WHERE isupplierid = " & isupplierid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsSupplier As DataSet
            dsSupplier = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " WHERE isupplierid = " & isupplierid)

            txtNombreComercial.Text = dsSupplier.Tables(0).Rows(0).Item("ssuppliername")
            txtDireccion.Text = dsSupplier.Tables(0).Rows(0).Item("ssupplieraddress")
            txtEmail.Text = dsSupplier.Tables(0).Rows(0).Item("ssupplieremail")
            txtObservaciones.Text = dsSupplier.Tables(0).Rows(0).Item("ssupplierobservations")

            txtNombreFiscal.Text = dsSupplier.Tables(0).Rows(0).Item("ssupplierofficialname")
            txtDireccionFiscal.Text = dsSupplier.Tables(0).Rows(0).Item("ssupplierofficialaddress")
            txtRFC.Text = dsSupplier.Tables(0).Rows(0).Item("ssupplierrfc")

        End If

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

        If isEdit = False Then
            dgvTelefonos.Enabled = False
        Else
            dgvTelefonos.Enabled = True
        End If


        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

        If isEdit = False Then
            dgvPersonas.Enabled = False
        Else
            dgvPersonas.Enabled = True
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Proveedor " & isupplierid & ": " & txtNombreComercial.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Supplier', 'Proveedor', '" & isupplierid & "', '" & txtNombreComercial.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        txtNombreComercial.Select()
        txtNombreComercial.Focus()
        txtNombreComercial.SelectionStart() = txtNombreComercial.Text.Length

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


    Private Sub txtNombreComercial_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreComercial.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreComercial.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreComercial.Text = txtNombreComercial.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNombreComercial.Select(txtNombreComercial.Text.Length, 0)
        End If

    End Sub


    Private Sub txtNombreComercial_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreComercial.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        'If isEdit = True Then

        '    Dim fecha As Integer = getMySQLDate()
        '    Dim hora As String = getAppTime()

        '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & txtNombreComercial.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

        'End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub txtNombreFiscal_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreFiscal.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreFiscal.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreFiscal.Text = txtNombreFiscal.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNombreFiscal.Select(txtNombreFiscal.Text.Length, 0)
        End If

    End Sub


    Private Sub txtNombreFiscal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreFiscal.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        'If isEdit = True Then

        '    Dim fecha As Integer = getMySQLDate()
        '    Dim hora As String = getAppTime()

        '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssupplierofficialname = '" & txtNombreFiscal.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

        'End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub txtDireccion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDireccion.KeyUp, txtDireccionFiscal.KeyUp

        Dim strcaracteresprohibidos As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDireccion.Text.Contains(arrayCaractProhib(carp)) Then
                txtDireccion.Text = txtDireccion.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDireccion.Select(txtDireccion.Text.Length, 0)
        End If

    End Sub


    Private Sub txtDireccion_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDireccion.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        'If isEdit = True Then

        '    Dim fecha As Integer = getMySQLDate()
        '    Dim hora As String = getAppTime()

        '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssupplieraddress = '" & txtDireccion.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

        'End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub txtDireccionFiscal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDireccionFiscal.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        'If isEdit = True Then

        '    Dim fecha As Integer = getMySQLDate()
        '    Dim hora As String = getAppTime()

        '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssupplierofficialaddress = '" & txtDireccionFiscal.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

        'End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub txtEmail_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmail.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]:;,{}+´¿'¬^`~\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtEmail.Text.Contains(arrayCaractProhib(carp)) Then
                txtEmail.Text = txtEmail.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtEmail.Select(txtEmail.Text.Length, 0)
        End If

    End Sub


    Private Sub txtEmail_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmail.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        'If isEdit = True Then

        '    Dim fecha As Integer = getMySQLDate()
        '    Dim hora As String = getAppTime()

        '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssupplieremail = '" & txtEmail.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

        'End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub txtObservaciones_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtObservaciones.KeyUp

        Dim strcaracteresprohibidos As String = "|°#$%&/()=¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtObservaciones.Text.Contains(arrayCaractProhib(carp)) Then
                txtObservaciones.Text = txtObservaciones.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtObservaciones.Select(txtObservaciones.Text.Length, 0)
        End If

    End Sub


    Private Sub txtObservaciones_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtObservaciones.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        'If isEdit = True Then

        '    Dim fecha As Integer = getMySQLDate()
        '    Dim hora As String = getAppTime()

        '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssupplierobservations = '" & txtObservaciones.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

        'End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub txtRFC_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRFC.KeyUp

        Dim strcaracteresprohibidos As String = "|°#$%&/()=¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtRFC.Text.Contains(arrayCaractProhib(carp)) Then
                txtRFC.Text = txtRFC.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtRFC.Select(txtRFC.Text.Length, 0)
        End If

    End Sub


    Private Sub txtRFC_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRFC.TextChanged

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Function validarDatosComerciales(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos3 As String = "|°#$%&/()=¡*¨[]_:;,-{}+´¿'¬^`~@\<>"

        txtNombreComercial.Text = txtNombreComercial.Text.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")
        txtDireccion.Text = txtDireccion.Text.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")
        txtObservaciones.Text = txtObservaciones.Text.Trim(strcaracteresprohibidos3.ToCharArray).Replace("--", "").Replace("'", "")

        If txtNombreComercial.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner el Nombre Comercial del Proveedor?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If save = True Then

            If txtDireccion.Text = "" Then
                If silent = False Then
                    MsgBox("¿Podrías poner la Dirección del Proveedor?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If

        End If

        Return True

    End Function


    Private Function validarDatosFiscales(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"

        txtNombreFiscal.Text = txtNombreFiscal.Text.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")
        txtDireccionFiscal.Text = txtDireccionFiscal.Text.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")
        txtRFC.Text = txtRFC.Text.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")

        If txtNombreFiscal.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner el Nombre Fiscal del Proveedor?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        'If txtRFC.Text = "" Then
        '    If silent = False Then
        '        MsgBox("¿Podrías poner el RFC del Proveedor?", MsgBoxStyle.OkOnly, "Dato Faltante")
        '    End If
        '    Return False
        'End If

        If save = True Then

            If txtDireccionFiscal.Text = "" Then
                If silent = False Then
                    MsgBox("¿Podrías poner la Dirección Fiscal del Proveedor?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If

        End If

        Return True

    End Function


    Private Sub btnCopiarPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopiarPersona.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPersonas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname

        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsPersona As DataSet

            dsPersona = getSQLQueryAsDataset(0, "SELECT ipeopleid, speoplefullname, speopleaddress, speoplemail, speopleobservations FROM people WHERE ipeopleid = " & bp.ipeopleid)

            txtNombreComercial.Text = dsPersona.Tables(0).Rows(0).Item("speoplefullname")
            txtDireccion.Text = dsPersona.Tables(0).Rows(0).Item("speopleaddress")
            txtEmail.Text = dsPersona.Tables(0).Rows(0).Item("speoplemail")
            txtObservaciones.Text = dsPersona.Tables(0).Rows(0).Item("speopleobservations")

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim queries(4) As String

            If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

                isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(9) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
                queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
                queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                dgvPersonas.Enabled = True
                dgvTelefonos.Enabled = True

            ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

                queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid

                dgvPersonas.Enabled = True
                dgvTelefonos.Enabled = True

            End If

            queries(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts VALUES (" & isupplierid & ", " & bp.ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"
            queries(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers SELECT " & isupplierid & ", ipeoplephonenumberid, speoplephonenumber, speoplephonetype, " & fecha & ", '" & hora & "', '" & susername & "' FROM peoplephonenumbers WHERE ipeopleid = " & bp.ipeopleid

            executeTransactedSQLCommand(0, queries)

            setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

            dgvPersonas.Columns(0).Visible = False
            dgvPersonas.Columns(1).Visible = False

            dgvPersonas.Columns(0).ReadOnly = True
            dgvPersonas.Columns(1).ReadOnly = True

            dgvPersonas.Columns(0).Width = 30
            dgvPersonas.Columns(1).Width = 30
            dgvPersonas.Columns(2).Width = 150

            dgvPersonas.Enabled = True


            setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

            dgvTelefonos.Columns(0).Visible = False
            dgvTelefonos.Columns(1).Visible = False

            dgvTelefonos.Columns(0).ReadOnly = True
            dgvTelefonos.Columns(1).ReadOnly = True

            dgvTelefonos.Columns(0).Width = 30
            dgvTelefonos.Columns(1).Width = 30
            dgvTelefonos.Columns(2).Width = 100

            dgvTelefonos.Enabled = True

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCopiarDatos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopiarDatos.Click

        txtNombreFiscal.Text = txtNombreComercial.Text
        txtDireccionFiscal.Text = txtDireccion.Text
        txtRFC.Focus()

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesCreation)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub btnDireccionFiscal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bd As New BuscaDirecciones
        bd.susername = susername
        bd.bactive = bactive
        bd.bonline = bonline
        bd.suserfullname = suserfullname

        bd.suseremail = suseremail
        bd.susersession = susersession
        bd.susermachinename = susermachinename
        bd.suserip = suserip

        bd.querystring = txtDireccionFiscal.Text

        If Me.WindowState = FormWindowState.Maximized Then
            bd.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bd.ShowDialog(Me)
        Me.Visible = True

        If bd.DialogResult = Windows.Forms.DialogResult.OK Then

            txtDireccionFiscal.Text = bd.sdireccion

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTelefonos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvTelefonos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedphoneid = CInt(dgvTelefonos.Rows(e.RowIndex).Cells(0).Value())
            sselectedphone = dgvTelefonos.Rows(e.RowIndex).Cells(2).Value()
            sselectedphonetype = dgvTelefonos.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            iselectedphoneid = 0
            sselectedphone = ""
            sselectedphonetype = ""

        End Try

    End Sub


    Private Sub dgvTelefonos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvTelefonos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedphoneid = CInt(dgvTelefonos.Rows(e.RowIndex).Cells(0).Value())
            sselectedphone = dgvTelefonos.Rows(e.RowIndex).Cells(2).Value()
            sselectedphonetype = dgvTelefonos.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            iselectedphoneid = 0
            sselectedphone = ""
            sselectedphonetype = ""

        End Try

    End Sub


    Private Sub dgvTelefonos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTelefonos.SelectionChanged

        Try

            If dgvTelefonos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedphoneid = CInt(dgvTelefonos.CurrentRow.Cells(0).Value())
            sselectedphone = dgvTelefonos.CurrentRow.Cells(2).Value()
            sselectedphonetype = dgvTelefonos.CurrentRow.Cells(3).Value()

        Catch ex As Exception

            iselectedphoneid = 0
            sselectedphone = ""
            sselectedphonetype = ""

        End Try

    End Sub


    Private Sub dgvTelefonos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTelefonos.CellEndEdit

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

    End Sub


    Private Sub dgvTelefonos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTelefonos.CellValueChanged

        If modifyPhonePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If e.ColumnIndex = 2 Then

            dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            If dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Teléfono?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Teléfono") = MsgBoxResult.Yes Then

                    If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid) = 1 Then

                        MsgBox("No puedo quedarme sin formas de contactar al Proveedor...", MsgBoxStyle.OkOnly, "Dato Faltante")
                        Exit Sub

                    Else

                        executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid & " AND isupplierphonenumberid = " & iselectedphoneid)

                    End If



                Else
                    'dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedphone
                End If

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers SET ssupplierphonenumber = '" & dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid & " AND isupplierphonenumberid = " & iselectedphoneid)

            End If


        ElseIf e.ColumnIndex = 3 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers SET ssupplierphonetype = '" & dgvTelefonos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid & " AND isupplierphonenumberid = " & iselectedphoneid)

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTelefonos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvTelefonos.EditingControlShowing

        If dgvTelefonos.CurrentCell.ColumnIndex = 2 Then

            txtNumeroDgvTelefonos = CType(e.Control, TextBox)
            txtNumeroDgvTelefonos_OldText = txtNumeroDgvTelefonos.Text

        ElseIf dgvTelefonos.CurrentCell.ColumnIndex = 3 Then

            txtTipoNumeroDgvTelefonos = CType(e.Control, TextBox)
            txtTipoNumeroDgvTelefonos_OldText = txtTipoNumeroDgvTelefonos.Text

        Else

            txtNumeroDgvTelefonos = Nothing
            txtTipoNumeroDgvTelefonos = Nothing
            txtNumeroDgvTelefonos_OldText = Nothing
            txtTipoNumeroDgvTelefonos_OldText = Nothing

        End If

    End Sub


    Private Sub txtNumeroDgvTelefonos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroDgvTelefonos.KeyUp

        txtNumeroDgvTelefonos.Text = txtNumeroDgvTelefonos.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!$%&/()=?¡¨[]_:;,{}´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNumeroDgvTelefonos.Text.Contains(arrayForbidden1(fc)) Then
                txtNumeroDgvTelefonos.Text = txtNumeroDgvTelefonos.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

        txtNumeroDgvTelefonos.Text = txtNumeroDgvTelefonos.Text.Replace("--", "").Replace("@", "").Replace("'", "")

    End Sub


    Private Sub dgvTelefonos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvTelefonos.UserAddedRow

        If addPhonePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedphoneid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierphonenumberid) + 1 IS NULL, 1, MAX(isupplierphonenumberid) + 1) AS isupplierphonenumberid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid)

        dgvTelefonos.CurrentRow.Cells(0).Value = iselectedphoneid
        dgvTelefonos.CurrentRow.Cells(1).Value = isupplierid
        dgvTelefonos.CurrentRow.Cells(2).Value = "1"
        dgvTelefonos.CurrentRow.Cells(3).Value = ""

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers VALUES (" & isupplierid & ", " & iselectedphoneid & ", '1', 'General', " & fecha & ", '" & hora & "', '" & susername & "')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvTelefonos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvTelefonos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePhonePermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar este Teléfono?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Teléfono") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedphoneid As Integer = 0
                Try
                    tmpselectedphoneid = CInt(dgvTelefonos.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid) = 1 Then

                    MsgBox("No puedo quedarme sin formas de contactar al Proveedor...", MsgBoxStyle.OkOnly, "Dato Faltante")
                    Exit Sub

                Else

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid & " AND isupplierphonenumberid = " & tmpselectedphoneid)

                End If

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvTelefonos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTelefonos.Click

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            If executeTransactedSQLCommand(0, queriesCreation) = True Then
                dgvPersonas.Enabled = True
                dgvTelefonos.Enabled = True
            End If

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub btnNuevoTelefono_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoTelefono.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim queries(4) As String

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        executeTransactedSQLCommand(0, queries)

        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

        dgvPersonas.Enabled = True

        dgvTelefonos.Enabled = True

        iselectedphoneid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierphonenumberid) + 1 IS NULL, 1, MAX(isupplierphonenumberid) + 1) AS isupplierphonenumberid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid)

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers VALUES (" & isupplierid & ", " & iselectedphoneid & ", '1', 'General', " & fecha & ", '" & hora & "', '" & susername & "')")

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarTelefono_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarTelefono.Click

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim queries(4) As String

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        executeTransactedSQLCommand(0, queries)

        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

        dgvPersonas.Enabled = True

        dgvTelefonos.Enabled = True


        If MsgBox("¿Está seguro que deseas eliminar este Teléfono?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Teléfono") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedphoneid As Integer = 0
            Try
                tmpselectedphoneid = CInt(dgvTelefonos.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid) = 1 Then

                MsgBox("No puedo quedarme sin formas de contactar al Proveedor...", MsgBoxStyle.OkOnly, "Dato Faltante")
                Exit Sub

            Else

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid & " AND isupplierphonenumberid = " & tmpselectedphoneid)

            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvPersonas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPersonas.CellClick

        Try

            If dgvPersonas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            ipeopleid = CInt(dgvPersonas.Rows(e.RowIndex).Cells(1).Value())
            sselectedpeoplefullname = dgvPersonas.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            ipeopleid = 0
            sselectedpeoplefullname = ""

        End Try

    End Sub


    Private Sub dgvPersonas_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPersonas.CellContentClick

        Try

            If dgvPersonas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            ipeopleid = CInt(dgvPersonas.Rows(e.RowIndex).Cells(1).Value())
            sselectedpeoplefullname = dgvPersonas.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            ipeopleid = 0
            sselectedpeoplefullname = ""

        End Try

    End Sub


    Private Sub dgvPersonas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvPersonas.SelectionChanged

        Try

            If dgvPersonas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            ipeopleid = CInt(dgvPersonas.CurrentRow.Cells(1).Value())
            sselectedpeoplefullname = dgvPersonas.CurrentRow.Cells(2).Value

        Catch ex As Exception

            ipeopleid = 0
            sselectedpeoplefullname = ""

        End Try

    End Sub


    Private Sub dgvPersonas_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPersonas.CellEndEdit

        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

    End Sub


    Private Sub dgvPersonas_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPersonas.CellValueChanged

        If modifyPeoplePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim dsBusquedaPersonaRepetida As DataSet

        If e.ColumnIndex = 2 Then

            dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            If dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar esta relación de la Persona con el Proveedor?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Persona Proveedor") = MsgBoxResult.Yes Then
                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts WHERE isupplierid = " & isupplierid & " AND ipeopleid = " & ipeopleid)
                Else
                    dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedpeoplefullname
                End If

            Else

                Dim bp As New BuscaPersonas

                bp.susername = susername
                bp.bactive = bactive
                bp.bonline = bonline
                bp.suserfullname = suserfullname

                bp.suseremail = suseremail
                bp.susersession = susersession
                bp.susermachinename = susermachinename
                bp.suserip = suserip


                bp.querystring = dgvPersonas.CurrentCell.EditedFormattedValue

                bp.isEdit = False

                If Me.WindowState = FormWindowState.Maximized Then
                    bp.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                bp.ShowDialog(Me)
                Me.Visible = True

                If bp.DialogResult = Windows.Forms.DialogResult.OK Then

                    dsBusquedaPersonaRepetida = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts WHERE isupplierid = " & isupplierid & " AND ipeopleid = " & bp.ipeopleid)

                    If dsBusquedaPersonaRepetida.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes a esa Persona relacionada con el Proveedor", MsgBoxStyle.OkOnly, "Persona Repetida")
                        Exit Sub

                    End If


                    If MsgBox("¿Estás seguro de que deseas reemplazar a la Persona " & dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & " por " & bp.speoplefullname & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Tarjeta") = MsgBoxResult.Yes Then

                        Dim queriesReplace(2) As String

                        queriesReplace(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts WHERE isupplierid = " & isupplierid & " AND ipeopleid = " & ipeopleid
                        queriesReplace(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts VALUES ( " & isupplierid & ", " & bp.ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                        executeTransactedSQLCommand(0, queriesReplace)

                    Else

                        'Si cancela el reemplazo de persona
                        dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedpeoplefullname

                    End If


                Else

                    'Si cancela el reemplazo de persona
                    dgvPersonas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedpeoplefullname


                End If


            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvPersonas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvPersonas.Click

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(10) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            If executeTransactedSQLCommand(0, queriesCreation) = True Then
                dgvPersonas.Enabled = True
                dgvTelefonos.Enabled = True
            End If

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        End If

    End Sub


    Private Sub dgvPersonas_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvPersonas.EditingControlShowing

        If dgvPersonas.CurrentCell.ColumnIndex = 2 Then

            txtNombreDgvPersonas = CType(e.Control, TextBox)
            txtNombreDgvPersonas_OldText = txtNombreDgvPersonas.Text

        Else

            txtNombreDgvPersonas = Nothing
            txtNombreDgvPersonas_OldText = Nothing

        End If

    End Sub


    Private Sub dgvPersonas_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvPersonas.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePeoplePermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas quitar esta relación de la Persona con el Proveedor?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Persona Proveedor") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedpeopleid As Integer = 0
                Try
                    tmpselectedpeopleid = CInt(dgvPersonas.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts WHERE isupplierid = " & isupplierid & " AND ipeopleid = " & tmpselectedpeopleid)

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvPersonas_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvPersonas.UserAddedRow

        If addPeoplePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPersonas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname

        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.querystring = dgvPersonas.CurrentCell.EditedFormattedValue

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts VALUES ( " & isupplierid & ", " & bp.ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')")

        End If

        dgvPersonas.EndEdit()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevaPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevaPersona.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim queries(4) As String

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        executeTransactedSQLCommand(0, queries)

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100


        dgvPersonas.Enabled = True

        dgvTelefonos.Enabled = True


        Dim ap As New AgregarPersona

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname

        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        If ap.DialogResult = Windows.Forms.DialogResult.OK Then

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts VALUES ( " & isupplierid & ", " & ap.ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')")

        End If

        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnInsertarPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarPersona.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim queries(4) As String

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        executeTransactedSQLCommand(0, queries)

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100


        dgvPersonas.Enabled = True

        dgvTelefonos.Enabled = True

        Dim bp As New BuscaPersonas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname

        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts VALUES ( " & isupplierid & ", " & bp.ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')")

        End If

        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarPersona.Click

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim queries(4) As String

        If (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = False And isupplierid = 0) Or (validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid = 0) Then

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(9) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
            queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
            queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            queries(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        ElseIf validarDatosComerciales(True, False) = True And validarDatosFiscales(True, False) = True And isupplierid > 0 Then

            queries(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid

            dgvPersonas.Enabled = True
            dgvTelefonos.Enabled = True

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        executeTransactedSQLCommand(0, queries)

        setDataGridView(dgvTelefonos, "SELECT isupplierphonenumberid, isupplierid, ssupplierphonenumber AS 'Telefono', ssupplierphonetype AS 'Tipo Telefono' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid, False)

        dgvTelefonos.Columns(0).Visible = False
        dgvTelefonos.Columns(1).Visible = False

        dgvTelefonos.Columns(0).ReadOnly = True
        dgvTelefonos.Columns(1).ReadOnly = True

        dgvTelefonos.Columns(0).Width = 30
        dgvTelefonos.Columns(1).Width = 30
        dgvTelefonos.Columns(2).Width = 100


        dgvPersonas.Enabled = True

        dgvTelefonos.Enabled = True

        If MsgBox("¿Está seguro que deseas quitar esta relación de la Persona con el Proveedor?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Persona Proveedor") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedpeopleid As Integer = 0
            Try
                tmpselectedpeopleid = CInt(dgvPersonas.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts WHERE isupplierid = " & isupplierid & " AND ipeopleid = " & tmpselectedpeopleid)

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

        setDataGridView(dgvPersonas, "SELECT sc.isupplierid, sc.ipeopleid, p.speoplefullname AS 'Nombre de Personas Relacionadas' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts sc JOIN people p ON sc.ipeopleid = p.ipeopleid WHERE sc.isupplierid = " & isupplierid, False)

        dgvPersonas.Columns(0).Visible = False
        dgvPersonas.Columns(1).Visible = False

        dgvPersonas.Columns(0).ReadOnly = True
        dgvPersonas.Columns(1).ReadOnly = True

        dgvPersonas.Columns(0).Width = 30
        dgvPersonas.Columns(1).Width = 30
        dgvPersonas.Columns(2).Width = 150

    End Sub


    Private Function validaFormasDeContacto(ByVal silent As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]:;,{}+´¿'¬^`~\<>"
        txtEmail.Text = txtEmail.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")

        If txtEmail.Text = "" And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers WHERE isupplierid = " & isupplierid) < 1 And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts WHERE isupplierid = " & isupplierid) < 1 Then
            If silent = False Then
                MsgBox("¿Podrías poner alguna forma de contactar al proveedor? Un email o un teléfono...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        Return True

    End Function


    Private Function validaProveedorCompleto(ByVal silent As Boolean) As Boolean

        If validarDatosComerciales(silent, True) = False Or validarDatosFiscales(silent, True) = False Or validaFormasDeContacto(silent) = False Then
            Return False
        Else
            Return True
        End If

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'isupplierid = 0
        'ssuppliername = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesSupplierIsOpen As Integer = 1

        timesSupplierIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Supplier" & isupplierid & "'")

        If timesSupplierIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Proveedor. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando el Proveedor?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesSupplierIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Supplier" & isupplierid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(6) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "PhoneNumbers"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "Contacts"
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & " SET isupplierid = " & isupplierid + newIdAddition & " WHERE isupplierid = " & isupplierid
            queriesNewId(4) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "PhoneNumbers SET isupplierid = " & isupplierid + newIdAddition & " WHERE isupplierid = " & isupplierid
            queriesNewId(5) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid + newIdAddition & "Contacts SET isupplierid = " & isupplierid + newIdAddition & " WHERE isupplierid = " & isupplierid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                isupplierid = isupplierid + newIdAddition
            End If

        End If


        If validaProveedorCompleto(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            ssuppliername = txtNombreComercial.Text
            ssupplierplace = txtDireccion.Text

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If isEdit = True Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

            Else

                Dim checkIfItsOnlyTextUpdate As Boolean = False

                checkIfItsOnlyTextUpdate = getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " WHERE isupplierid = " & isupplierid)

                If checkIfItsOnlyTextUpdate = True Then

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " SET ssuppliername = '" & ssuppliername & "', ssupplierofficialname = '" & txtNombreFiscal.Text & "', ssupplieraddress = '" & txtDireccion.Text & "', ssupplierofficialaddress = '" & txtDireccionFiscal.Text & "', ssupplieremail = '" & txtEmail.Text & "', ssupplierobservations = '" & txtObservaciones.Text & "', ssupplierrfc = '" & txtRFC.Text & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierid = " & isupplierid)

                Else

                    isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                    Dim queriesCreation(10) As String

                    queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0"
                    queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid
                    queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ( `isupplierid` int(10) NOT NULL AUTO_INCREMENT, `ssuppliername` varchar(400) CHARACTER SET latin1 NOT NULL, `ssupplierofficialname` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplieraddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierofficialaddress` varchar(1000) CHARACTER SET latin1 NOT NULL, `ssupplierrfc` varchar(15) CHARACTER SET latin1 NOT NULL, `ssupplieremail` varchar(500) CHARACTER SET latin1 NOT NULL, `ssupplierobservations` varchar(1000) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0PhoneNumbers"
                    queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers"
                    queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers ( `isupplierid` int(11) NOT NULL, `isupplierphonenumberid` int(11) NOT NULL, `ssupplierphonenumber` varchar(100) CHARACTER SET latin1 NOT NULL, `ssupplierphonetype` varchar(200) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`isupplierphonenumberid`), KEY `supplierid` (`isupplierid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier0Contacts"
                    queriesCreation(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts"
                    queriesCreation(8) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts ( `isupplierid` int(11) NOT NULL, `ipeopleid` int(11) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierid`,`ipeopleid`), KEY `supplierid` (`isupplierid`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesCreation(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " VALUES (" & isupplierid & ", '" & ssuppliername & "', '" & txtNombreFiscal.Text & "', '" & txtDireccion.Text & "', '" & txtDireccionFiscal.Text & "', '" & txtRFC.Text & "', '" & txtEmail.Text & "', '" & txtObservaciones.Text & "', " & fecha & ", '" & hora & "', '" & susername & "')"

                    executeTransactedSQLCommand(0, queriesCreation)

                End If

            End If


            Dim queriesSave(10) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM suppliers " & _
            "WHERE isupplierid = " & isupplierid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts WHERE suppliers.isupplierid = ts.isupplierid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM supplierphonenumbers " & _
            "WHERE isupplierid = " & isupplierid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn WHERE supplierphonenumbers.isupplierid = tspn.isupplierid AND supplierphonenumbers.isupplierphonenumberid = tspn.isupplierphonenumberid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM suppliercontacts " & _
            "WHERE isupplierid = " & isupplierid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc WHERE suppliercontacts.isupplierid = tsc.isupplierid AND suppliercontacts.ipeopleid = tsc.ipeopleid) "

            queriesSave(3) = "" & _
            "UPDATE suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts ON ts.isupplierid = s.isupplierid SET s.iupdatedate = ts.iupdatedate, s.supdatetime = ts.supdatetime, s.supdateusername = ts.supdateusername, s.ssuppliername = ts.ssuppliername, s.ssupplierofficialname = ts.ssupplierofficialname, s.ssupplieraddress = ts.ssupplieraddress, s.ssupplierofficialaddress = ts.ssupplierofficialaddress, s.ssupplierrfc = ts.ssupplierrfc, s.ssupplieremail = ts.ssupplieremail, s.ssupplierobservations = ts.ssupplierobservations WHERE STR_TO_DATE(CONCAT(ts.iupdatedate, ' ', ts.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(s.iupdatedate, ' ', s.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "UPDATE supplierphonenumbers spn JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn ON tspn.isupplierid = spn.isupplierid AND tspn.isupplierphonenumberid = spn.isupplierphonenumberid SET spn.iupdatedate = tspn.iupdatedate, spn.supdatetime = tspn.supdatetime, spn.supdateusername = tspn.supdateusername, spn.ssupplierphonenumber = tspn.ssupplierphonenumber WHERE STR_TO_DATE(CONCAT(tspn.iupdatedate, ' ', tspn.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(spn.iupdatedate, ' ', spn.supdatetime), '%Y%c%d %T') "

            queriesSave(5) = "" & _
            "UPDATE suppliercontacts sc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc ON tsc.isupplierid = sc.isupplierid AND tsc.ipeopleid = sc.ipeopleid SET sc.iupdatedate = tsc.iupdatedate, sc.supdatetime = tsc.supdatetime, sc.supdateusername = tsc.supdateusername, sc.ipeopleid = tsc.ipeopleid WHERE STR_TO_DATE(CONCAT(tsc.iupdatedate, ' ', tsc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sc.iupdatedate, ' ', sc.supdatetime), '%Y%c%d %T') "

            queriesSave(6) = "" & _
            "INSERT INTO suppliers " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & " ts " & _
            "WHERE NOT EXISTS (SELECT * FROM suppliers s WHERE s.isupplierid = ts.isupplierid AND s.isupplierid = " & isupplierid & ") "

            queriesSave(7) = "" & _
            "INSERT INTO supplierphonenumbers " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "PhoneNumbers tspn " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierphonenumbers spn WHERE spn.isupplierid = tspn.isupplierid AND spn.isupplierphonenumberid = tspn.isupplierphonenumberid AND spn.isupplierid = " & isupplierid & ") "

            queriesSave(8) = "" & _
            "INSERT INTO suppliercontacts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Supplier" & isupplierid & "Contacts tsc " & _
            "WHERE NOT EXISTS (SELECT * FROM suppliercontacts sc WHERE sc.isupplierid = tsc.isupplierid AND sc.ipeopleid = tsc.ipeopleid AND sc.isupplierid = " & isupplierid & ") "

            queriesSave(9) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Proveedor " & isupplierid & ": " & txtNombreComercial.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            wasCreated = True

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvPersonas_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPersonas.CellContentDoubleClick

        If openPeoplePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvPersonas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            ipeopleid = CInt(dgvPersonas.Rows(e.RowIndex).Cells(1).Value())

        Catch ex As Exception

            ipeopleid = 0

        End Try

        If dgvPersonas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim ap As New AgregarPersona

            ap.susername = susername
            ap.bactive = bactive
            ap.bonline = bonline
            ap.suserfullname = suserfullname
            ap.suseremail = suseremail
            ap.susersession = susersession
            ap.susermachinename = susermachinename
            ap.suserip = suserip

            ap.ipeopleid = ipeopleid

            ap.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ap.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ap.ShowDialog(Me)
            Me.Visible = True

            If ap.DialogResult = Windows.Forms.DialogResult.OK Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvPersonas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPersonas.CellDoubleClick

        If openPeoplePermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvPersonas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            ipeopleid = CInt(dgvPersonas.Rows(e.RowIndex).Cells(1).Value())

        Catch ex As Exception

            ipeopleid = 0

        End Try

        If dgvPersonas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim ap As New AgregarPersona

            ap.susername = susername
            ap.bactive = bactive
            ap.bonline = bonline
            ap.suserfullname = suserfullname
            ap.suseremail = suseremail
            ap.susersession = susersession
            ap.susermachinename = susermachinename
            ap.suserip = suserip

            ap.ipeopleid = ipeopleid

            ap.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                ap.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ap.ShowDialog(Me)
            Me.Visible = True

            If ap.DialogResult = Windows.Forms.DialogResult.OK Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class