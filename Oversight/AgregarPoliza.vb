Public Class AgregarPoliza

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public isRecover As Boolean = False
    Public isEdit As Boolean = False

    Public wasCreated As Boolean = False

    Public ipolicyid As Integer = 0
    Public spolicydescription As String = ""
    
    Private iselectedid As Integer = 0
    Private sselectedtypeid As String = ""

    Private isFormReadyForAction As Boolean = False

    Private openPermission As Boolean = False
    Private alreadyDone As Boolean = False

    Private addPolicyMemberPermission As Boolean = False
    Private modifyPolicyMemberPermission As Boolean = False
    Private deletePolicyMemberPermission As Boolean = False

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

                If permission = "Agregar Movimiento" Then
                    addPolicyMemberPermission = True
                    btnInsertar.Enabled = True
                End If

                If permission = "Modificar Movimiento" Then
                    modifyPolicyMemberPermission = True
                End If

                If permission = "Eliminar Movimiento" Then
                    deletePolicyMemberPermission = True
                    btnEliminar.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Póliza', 'OK')")

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


    Private Sub AgregarPoliza_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM policies " & _
        "WHERE ipolicyid = " & ipolicyid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp WHERE policies.ipolicyid = tp.ipolicyid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM policymovements " & _
        "WHERE ipolicyid = " & ipolicyid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp WHERE policymovements.ipolicyid = tp.ipolicyid AND policymovements.sdocumentname = tp.sdocumentname AND policymovements.iid = tp.iid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp JOIN policies p ON tp.ipolicyid = p.ipolicyid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
       "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp JOIN policymovements p ON tp.ipolicyid = p.ipolicyid AND tp.sdocumentname = p.sdocumentname AND tp.iid = p.iid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM policies p WHERE p.ipolicyid = tp.ipolicyid AND p.ipolicyid = " & ipolicyid & ") ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp " & _
        "WHERE NOT EXISTS (SELECT * FROM policymovements p WHERE p.ipolicyid = tp.ipolicyid AND p.sdocumentname = tp.sdocumentname AND p.iid = tp.iid AND p.ipolicyid = " & ipolicyid & ") ")


        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0


        If validaPoliza(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Los datos de esta Póliza están incompletos. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesPolicyMemberIsOpen As Integer = 1

            timesPolicyMemberIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid & "'")

            If timesPolicyMemberIsOpen > 1 Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Póliza. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Póliza?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            End If

            Dim queriesSave(7) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM policies " & _
            "WHERE ipolicyid = " & ipolicyid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp WHERE policies.ipolicyid = tp.ipolicyid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM policymovements " & _
            "WHERE ipolicyid = " & ipolicyid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp WHERE policymovements.ipolicyid = tp.ipolicyid AND policymovements.sdocumentname = tp.sdocumentname AND policymovements.iid = tp.iid) "

            queriesSave(2) = "" & _
            "UPDATE policies p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp ON tp.ipolicyid = p.ipolicyid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.ipolicydate = tp.ipolicydate, p.spolicytime = tp.spolicytime, p.spolicydescription = tp.spolicydescription WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(3) = "" & _
            "UPDATE policymovements p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp ON tp.ipolicyid = p.ipolicyid AND tp.sdocumentname = p.sdocumentname AND tp.iid = p.iid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sextranote = tp.sextranote WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "INSERT INTO policies " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp " & _
            "WHERE NOT EXISTS (SELECT * FROM policies p WHERE p.ipolicyid = tp.ipolicyid AND p.ipolicyid = " & ipolicyid & ") "

            queriesSave(5) = "" & _
            "INSERT INTO policymovements " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp " & _
            "WHERE NOT EXISTS (SELECT * FROM policymovements p WHERE p.ipolicyid = tp.ipolicyid AND p.sdocumentname = tp.sdocumentname AND p.iid = tp.iid AND p.ipolicyid = " & ipolicyid & ") "

            queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Póliza " & ipolicyid & "', 'OK')"

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

        Dim queriesDelete(4) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid
        queriesDelete(2) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Póliza " & ipolicyid & "', 'OK')"
        queriesDelete(3) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Policy', 'Póliza', '" & ipolicyid & "', 'Póliza', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarPoliza_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarPoliza_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesPolicyMemberIsOpen As Integer = 0

        timesPolicyMemberIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid & "'")

        If timesPolicyMemberIsOpen > 0 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Póliza. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo esta Póliza?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If


        If isRecover = False Then

            Dim queriesCreation(4) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " ( `ipolicyid` int(11) NOT NULL, `ipolicydate` int(11) NOT NULL, `spolicytime` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `spolicydescription` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `supdateusername` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, PRIMARY KEY (`ipolicyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=utf8"
            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements ( `ipolicyid` int(11) NOT NULL, `sdocumentname` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `iid` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `sextranote` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, `supdateusername` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL, PRIMARY KEY (`ipolicyid`,`sdocumentname`,`iid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=utf8"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        If isEdit = False Then

            txtConcepto.Text = ""
            txtTotal.Text = FormatCurrency(0.0, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Else

            If isRecover = False Then

                Dim queriesInsert(6) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " SELECT * FROM policies WHERE ipolicyid = " & ipolicyid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements SELECT * FROM policymovements WHERE ipolicyid = " & ipolicyid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim dsPolicy As DataSet

            dsPolicy = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " WHERE ipolicyid = " & ipolicyid)

            txtConcepto.Text = dsPolicy.Tables(0).Rows(0).Item("spolicydescription")
            spolicydescription = dsPolicy.Tables(0).Rows(0).Item("spolicydescription")

            dtFechaPoliza.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsPolicy.Tables(0).Rows(0).Item("ipolicydate"))

            Dim queryTotalPoliza As String = ""

            queryTotalPoliza = "" & _
            "SELECT FORMAT(SUM(tmpC.monto), 2) " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
            "JOIN ( " & _
            "SELECT * FROM ( " & _
            "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
            "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
            "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
            "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
            "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
            "FROM ( " & _
            "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
            "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
            "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
            "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
            "GROUP BY si.isupplierinvoiceid " & _
            "UNION " & _
            "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
            "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
            "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
            "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
            "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
            "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
            "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "GROUP BY si.isupplierinvoiceid " & _
            ") tmpA " & _
            "UNION " & _
            "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
            "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
            "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
            "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
            "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
            "FROM ( " & _
            "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
            "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
            "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
            "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
            "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
            "ORDER BY s.ssuppliername ASC)tmpA " & _
            "WHERE total > 0 " & _
            "GROUP BY 1 " & _
            "UNION " & _
            "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
            "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
            "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
            "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
            "FROM gasvouchers g " & _
            "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
            "JOIN assets a ON g.iassetid = a.iassetid " & _
            "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
            "UNION " & _
            "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
            "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
            "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
            "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
            "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
            "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
            "FROM payrolls pr " & _
            "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
            "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
            "GROUP BY 1 " & _
            "UNION " & _
            "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
            "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
            "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
            "FROM payments py " & _
            "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
            "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
            "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
            "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
            "UNION " & _
            "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
            "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
            "ic.dincomeamount AS 'monto', '', '', '', '' " & _
            "FROM incomes ic " & _
            "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
            "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
            "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
            "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
            "UNION " & _
            "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
            "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
            "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
            "FROM paytolls ic " & _
            "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
            "  ) tmpB " & _
            ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
            "WHERE p.ipolicyid = " & ipolicyid

            txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, queryTotalPoliza), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If



        Dim queryPolicyMovements As String = ""

        queryPolicyMovements = "" & _
        "SELECT pm.sdocumentname AS 'Documento', pm.iid, tmpC.sexpensedescription AS 'Descripcion', tmpC.fecha AS 'Fecha Movimiento', " & _
        "tmpC.tipo AS 'Tipo Movimiento', tmpC.ssuppliername AS 'Persona/Proveedor', tmpC.idadicional AS 'Folio/Identificador', " & _
        "FORMAT(tmpC.monto, 2) AS 'Total', CAST(tmpC.sexpenselocation AS CHAR) AS 'Lugar', tmpC.personafactura AS 'Responsable', tmpC.sprojectname AS 'Proyecto/Datos Adicionales', " & _
        "tmpC.cliente AS 'Cliente/Datos Adicionales', pm.sextranote AS 'Observaciones' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
        "JOIN ( " & _
        "SELECT * FROM ( " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
        "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
        "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
        "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
        "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
        "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
        "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
        "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "UNION " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
        "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
        "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
        "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
        "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
        "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
        "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
        "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
        "ORDER BY s.ssuppliername ASC)tmpA " & _
        "WHERE total > 0 " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
        "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
        "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
        "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
        "FROM gasvouchers g " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
        "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
        "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
        "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
        "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
        "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
        "FROM payrolls pr " & _
        "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
        "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
        "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
        "ic.dincomeamount AS 'monto', '', '', '', '' " & _
        "FROM incomes ic " & _
        "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
        "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
        "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
        "FROM paytolls ic " & _
        "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
        "  ) tmpB " & _
        ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
        "WHERE p.ipolicyid = " & ipolicyid & " ORDER BY 4 ASC, 3 ASC "

        setDataGridView(dgvMovimientos, queryPolicyMovements, False)

        dgvMovimientos.Columns(1).Visible = False

        dgvMovimientos.Columns(0).ReadOnly = True
        dgvMovimientos.Columns(1).ReadOnly = True
        dgvMovimientos.Columns(2).ReadOnly = True
        dgvMovimientos.Columns(3).ReadOnly = True
        dgvMovimientos.Columns(4).ReadOnly = True
        dgvMovimientos.Columns(5).ReadOnly = True
        dgvMovimientos.Columns(6).ReadOnly = True
        dgvMovimientos.Columns(7).ReadOnly = True
        dgvMovimientos.Columns(8).ReadOnly = True
        dgvMovimientos.Columns(9).ReadOnly = True
        dgvMovimientos.Columns(10).ReadOnly = True
        dgvMovimientos.Columns(11).ReadOnly = True
        dgvMovimientos.Columns(11).ReadOnly = False

        dgvMovimientos.Columns(0).Width = 130
        dgvMovimientos.Columns(2).Width = 200
        dgvMovimientos.Columns(3).Width = 130
        dgvMovimientos.Columns(4).Width = 120
        dgvMovimientos.Columns(5).Width = 200
        dgvMovimientos.Columns(6).Width = 70
        dgvMovimientos.Columns(7).Width = 70
        dgvMovimientos.Columns(8).Width = 150
        dgvMovimientos.Columns(9).Width = 150
        dgvMovimientos.Columns(10).Width = 250
        dgvMovimientos.Columns(11).Width = 250
        dgvMovimientos.Columns(12).Width = 200

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Póliza " & ipolicyid & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Policy', 'Póliza', '" & ipolicyid & "', 'Póliza', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True

        txtConcepto.Select()
        txtConcepto.Focus()

        txtConcepto.SelectionStart() = txtConcepto.Text.Length

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


    Private Function validaPoliza(ByVal silent As Boolean) As Boolean


        If txtConcepto.Text.Trim = "" Then
            If silent = False Then
                MsgBox("¿Podrías ponerle una descripcion a la Póliza?", MsgBoxStyle.OkOnly, "Dato faltante")
            End If
            Return False
        End If


        Dim queryTotalPoliza As String = ""

        queryTotalPoliza = "" & _
        "SELECT FORMAT(SUM(tmpC.monto), 2) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
        "JOIN ( " & _
        "SELECT * FROM ( " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
        "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
        "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
        "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
        "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
        "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
        "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
        "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "UNION " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
        "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
        "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
        "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
        "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
        "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
        "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
        "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
        "ORDER BY s.ssuppliername ASC)tmpA " & _
        "WHERE total > 0 " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
        "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
        "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
        "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
        "FROM gasvouchers g " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
        "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
        "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
        "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
        "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
        "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
        "FROM payrolls pr " & _
        "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
        "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
        "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
        "ic.dincomeamount AS 'monto', '', '', '', '' " & _
        "FROM incomes ic " & _
        "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
        "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
        "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
        "FROM paytolls ic " & _
        "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
        "  ) tmpB " & _
        ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
        "WHERE p.ipolicyid = " & ipolicyid

        If getSQLQueryAsDouble(0, queryTotalPoliza) = 0 Then
            If silent = False Then
                MsgBox("¿Podrías poner un movimiento en la Póliza? Está en ceros...", MsgBoxStyle.OkOnly, "Dato faltante")
            End If
            Return False
        End If

    End Function


    Private Sub dgvMovimientos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvMovimientos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            sselectedtypeid = dgvMovimientos.Rows(e.RowIndex).Cells(0).Value()
            iselectedid = CInt(dgvMovimientos.Rows(e.RowIndex).Cells(1).Value())

        Catch ex As Exception

            sselectedtypeid = ""
            iselectedid = 0

        End Try

    End Sub


    Private Sub dgvMovimientos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvMovimientos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            sselectedtypeid = dgvMovimientos.Rows(e.RowIndex).Cells(0).Value()
            iselectedid = CInt(dgvMovimientos.Rows(e.RowIndex).Cells(1).Value())

        Catch ex As Exception

            sselectedtypeid = ""
            iselectedid = 0

        End Try

    End Sub


    Private Sub dgvMovimientos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMovimientos.SelectionChanged

        Try

            If dgvMovimientos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            sselectedtypeid = dgvMovimientos.CurrentRow.Cells(0).Value()
            iselectedid = CInt(dgvMovimientos.CurrentRow.Cells(1).Value())

        Catch ex As Exception

            sselectedtypeid = ""
            iselectedid = 0

        End Try

    End Sub


    Private Sub dgvMovimientos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMovimientos.CellEndEdit

        Dim queryPolicyMovements As String = ""

        queryPolicyMovements = "" & _
        "SELECT pm.sdocumentname AS 'Documento', pm.iid, tmpC.sexpensedescription AS 'Descripcion', tmpC.fecha AS 'Fecha Movimiento', " & _
        "tmpC.tipo AS 'Tipo Movimiento', tmpC.ssuppliername AS 'Persona/Proveedor', tmpC.idadicional AS 'Folio/Identificador', " & _
        "FORMAT(tmpC.monto, 2) AS 'Total', CAST(tmpC.sexpenselocation AS CHAR) AS 'Lugar', tmpC.personafactura AS 'Responsable', tmpC.sprojectname AS 'Proyecto/Datos Adicionales', " & _
        "tmpC.cliente AS 'Cliente/Datos Adicionales', pm.sextranote AS 'Observaciones' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
        "JOIN ( " & _
        "SELECT * FROM ( " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
        "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
        "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
        "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
        "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
        "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
        "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
        "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "UNION " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
        "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
        "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
        "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
        "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
        "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
        "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
        "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
        "ORDER BY s.ssuppliername ASC)tmpA " & _
        "WHERE total > 0 " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
        "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
        "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
        "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
        "FROM gasvouchers g " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
        "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
        "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
        "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
        "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
        "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
        "FROM payrolls pr " & _
        "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
        "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
        "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
        "ic.dincomeamount AS 'monto', '', '', '', '' " & _
        "FROM incomes ic " & _
        "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
        "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
        "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
        "FROM paytolls ic " & _
        "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
        "  ) tmpB " & _
        ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
        "WHERE p.ipolicyid = " & ipolicyid & " ORDER BY 4 ASC, 3 ASC "

        setDataGridView(dgvMovimientos, queryPolicyMovements, False)

        dgvMovimientos.Columns(1).Visible = False

        dgvMovimientos.Columns(0).ReadOnly = True
        dgvMovimientos.Columns(1).ReadOnly = True
        dgvMovimientos.Columns(2).ReadOnly = True
        dgvMovimientos.Columns(3).ReadOnly = True
        dgvMovimientos.Columns(4).ReadOnly = True
        dgvMovimientos.Columns(5).ReadOnly = True
        dgvMovimientos.Columns(6).ReadOnly = True
        dgvMovimientos.Columns(7).ReadOnly = True
        dgvMovimientos.Columns(8).ReadOnly = True
        dgvMovimientos.Columns(9).ReadOnly = True
        dgvMovimientos.Columns(10).ReadOnly = True
        dgvMovimientos.Columns(11).ReadOnly = True
        dgvMovimientos.Columns(11).ReadOnly = False

        dgvMovimientos.Columns(0).Width = 130
        dgvMovimientos.Columns(2).Width = 200
        dgvMovimientos.Columns(3).Width = 130
        dgvMovimientos.Columns(4).Width = 120
        dgvMovimientos.Columns(5).Width = 200
        dgvMovimientos.Columns(6).Width = 70
        dgvMovimientos.Columns(7).Width = 70
        dgvMovimientos.Columns(8).Width = 150
        dgvMovimientos.Columns(9).Width = 150
        dgvMovimientos.Columns(10).Width = 250
        dgvMovimientos.Columns(11).Width = 250
        dgvMovimientos.Columns(12).Width = 200




        Dim queryTotalPoliza As String = ""

        queryTotalPoliza = "" & _
        "SELECT FORMAT(SUM(tmpC.monto), 2) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
        "JOIN ( " & _
        "SELECT * FROM ( " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
        "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
        "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
        "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
        "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
        "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
        "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
        "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "UNION " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
        "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
        "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
        "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
        "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
        "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
        "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
        "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
        "ORDER BY s.ssuppliername ASC)tmpA " & _
        "WHERE total > 0 " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
        "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
        "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
        "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
        "FROM gasvouchers g " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
        "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
        "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
        "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
        "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
        "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
        "FROM payrolls pr " & _
        "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
        "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
        "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
        "ic.dincomeamount AS 'monto', '', '', '', '' " & _
        "FROM incomes ic " & _
        "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
        "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
        "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
        "FROM paytolls ic " & _
        "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
        "  ) tmpB " & _
        ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
        "WHERE p.ipolicyid = " & ipolicyid

        txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, queryTotalPoliza), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


    End Sub


    Private Sub dgvMovimientos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMovimientos.CellValueChanged

        If modifyPolicyMemberPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim tmpsselectedtypeid As String = ""
        Dim tmpiselectedid As Integer = 0

        Try

            tmpsselectedtypeid = dgvMovimientos.CurrentRow.Cells(0).Value()
            tmpiselectedid = dgvMovimientos.CurrentRow.Cells(1).Value()

        Catch ex As Exception

            tmpsselectedtypeid = ""
            tmpiselectedid = 0

        End Try

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If e.ColumnIndex = 12 Then 'sextranote

            dgvMovimientos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvMovimientos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            If dgvMovimientos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements SET sextranote = '', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipolicyid = " & ipolicyid & " AND sdocumentname = '" & tmpsselectedtypeid & "' AND iid = " & tmpiselectedid)
            Else
                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements SET sextranote = '" & dgvMovimientos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipolicyid = " & ipolicyid & " AND sdocumentname = '" & tmpsselectedtypeid & "' AND iid = " & tmpiselectedid)
            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvMovimientos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvMovimientos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePolicyMemberPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar este movimiento de la Póliza?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Movimiento de la Póliza") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpsselectedtypeid As String = ""
                Dim tmpiselectedid As Integer = 0

                Try

                    tmpsselectedtypeid = dgvMovimientos.CurrentRow.Cells(0).Value()
                    tmpiselectedid = dgvMovimientos.CurrentRow.Cells(1).Value()

                Catch ex As Exception

                    tmpsselectedtypeid = ""
                    tmpiselectedid = 0

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " WHERE ipolicyid = " & ipolicyid & " AND sdocumentname = '" & tmpsselectedtypeid & "' AND iid = " & tmpiselectedid)




                Dim queryPolicyMovements As String = ""

                queryPolicyMovements = "" & _
                "SELECT pm.sdocumentname AS 'Documento', pm.iid, tmpC.sexpensedescription AS 'Descripcion', tmpC.fecha AS 'Fecha Movimiento', " & _
                "tmpC.tipo AS 'Tipo Movimiento', tmpC.ssuppliername AS 'Persona/Proveedor', tmpC.idadicional AS 'Folio/Identificador', " & _
                "FORMAT(tmpC.monto, 2) AS 'Total', CAST(tmpC.sexpenselocation AS CHAR) AS 'Lugar', tmpC.personafactura AS 'Responsable', tmpC.sprojectname AS 'Proyecto/Datos Adicionales', " & _
                "tmpC.cliente AS 'Cliente/Datos Adicionales', pm.sextranote AS 'Observaciones' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
                "JOIN ( " & _
                "SELECT * FROM ( " & _
                "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
                "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
                "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
                "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
                "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
                "FROM ( " & _
                "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
                "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
                "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
                "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
                "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
                "FROM supplierinvoices si " & _
                "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
                "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
                "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
                "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
                "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
                "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
                "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
                "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
                "GROUP BY si.isupplierinvoiceid " & _
                "UNION " & _
                "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
                "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
                "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
                "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
                "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
                "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
                "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
                "FROM supplierinvoices si " & _
                "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
                "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
                "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
                "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
                "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
                "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
                "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
                "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
                "GROUP BY si.isupplierinvoiceid " & _
                ") tmpA " & _
                "UNION " & _
                "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
                "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
                "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
                "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
                "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
                "FROM ( " & _
                "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
                "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
                "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
                "FROM supplierinvoices si " & _
                "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
                "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
                "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
                "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
                "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
                "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
                "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
                "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
                "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
                "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
                "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
                "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
                "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
                "ORDER BY s.ssuppliername ASC)tmpA " & _
                "WHERE total > 0 " & _
                "GROUP BY 1 " & _
                "UNION " & _
                "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
                "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
                "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
                "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
                "FROM gasvouchers g " & _
                "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
                "JOIN assets a ON g.iassetid = a.iassetid " & _
                "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
                "UNION " & _
                "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
                "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
                "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
                "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
                "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
                "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
                "FROM payrolls pr " & _
                "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
                "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
                "GROUP BY 1 " & _
                "UNION " & _
                "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
                "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
                "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
                "FROM payments py " & _
                "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
                "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
                "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
                "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
                "UNION " & _
                "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
                "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
                "ic.dincomeamount AS 'monto', '', '', '', '' " & _
                "FROM incomes ic " & _
                "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
                "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
                "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
                "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
                "UNION " & _
                "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
                "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
                "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
                "FROM paytolls ic " & _
                "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
                "  ) tmpB " & _
                ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
                "WHERE p.ipolicyid = " & ipolicyid & " ORDER BY 4 ASC, 3 ASC "

                setDataGridView(dgvMovimientos, queryPolicyMovements, False)

                dgvMovimientos.Columns(1).Visible = False

                dgvMovimientos.Columns(0).ReadOnly = True
                dgvMovimientos.Columns(1).ReadOnly = True
                dgvMovimientos.Columns(2).ReadOnly = True
                dgvMovimientos.Columns(3).ReadOnly = True
                dgvMovimientos.Columns(4).ReadOnly = True
                dgvMovimientos.Columns(5).ReadOnly = True
                dgvMovimientos.Columns(6).ReadOnly = True
                dgvMovimientos.Columns(7).ReadOnly = True
                dgvMovimientos.Columns(8).ReadOnly = True
                dgvMovimientos.Columns(9).ReadOnly = True
                dgvMovimientos.Columns(10).ReadOnly = True
                dgvMovimientos.Columns(11).ReadOnly = True
                dgvMovimientos.Columns(11).ReadOnly = False

                dgvMovimientos.Columns(0).Width = 130
                dgvMovimientos.Columns(2).Width = 200
                dgvMovimientos.Columns(3).Width = 130
                dgvMovimientos.Columns(4).Width = 120
                dgvMovimientos.Columns(5).Width = 200
                dgvMovimientos.Columns(6).Width = 70
                dgvMovimientos.Columns(7).Width = 70
                dgvMovimientos.Columns(8).Width = 150
                dgvMovimientos.Columns(9).Width = 150
                dgvMovimientos.Columns(10).Width = 250
                dgvMovimientos.Columns(11).Width = 250
                dgvMovimientos.Columns(12).Width = 200




                Dim queryTotalPoliza As String = ""

                queryTotalPoliza = "" & _
                "SELECT FORMAT(SUM(tmpC.monto), 2) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
                "JOIN ( " & _
                "SELECT * FROM ( " & _
                "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
                "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
                "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
                "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
                "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
                "FROM ( " & _
                "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
                "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
                "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
                "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
                "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
                "FROM supplierinvoices si " & _
                "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
                "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
                "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
                "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
                "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
                "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
                "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
                "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
                "GROUP BY si.isupplierinvoiceid " & _
                "UNION " & _
                "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
                "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
                "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
                "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
                "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
                "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
                "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
                "FROM supplierinvoices si " & _
                "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
                "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
                "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
                "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
                "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
                "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
                "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
                "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
                "GROUP BY si.isupplierinvoiceid " & _
                ") tmpA " & _
                "UNION " & _
                "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
                "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
                "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
                "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
                "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
                "FROM ( " & _
                "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
                "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
                "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
                "FROM supplierinvoices si " & _
                "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
                "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
                "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
                "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
                "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
                "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
                "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
                "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
                "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
                "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
                "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
                "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
                "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
                "ORDER BY s.ssuppliername ASC)tmpA " & _
                "WHERE total > 0 " & _
                "GROUP BY 1 " & _
                "UNION " & _
                "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
                "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
                "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
                "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
                "FROM gasvouchers g " & _
                "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
                "JOIN assets a ON g.iassetid = a.iassetid " & _
                "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
                "UNION " & _
                "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
                "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
                "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
                "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
                "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
                "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
                "FROM payrolls pr " & _
                "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
                "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
                "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
                "GROUP BY 1 " & _
                "UNION " & _
                "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
                "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
                "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
                "FROM payments py " & _
                "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
                "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
                "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
                "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
                "UNION " & _
                "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
                "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
                "ic.dincomeamount AS 'monto', '', '', '', '' " & _
                "FROM incomes ic " & _
                "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
                "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
                "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
                "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
                "UNION " & _
                "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
                "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
                "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
                "FROM paytolls ic " & _
                "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
                "  ) tmpB " & _
                ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
                "WHERE p.ipolicyid = " & ipolicyid

                txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, queryTotalPoliza), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub btnInsertar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertar.Click


        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If ipolicyid = 0 Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(12) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements"
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid

            If executeTransactedSQLCommand(0, queriesNewId) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " VALUES ( " & ipolicyid + newIdAddition & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtConcepto.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                ipolicyid = ipolicyid + newIdAddition

                btnInsertar.Enabled = True
                btnEliminar.Enabled = True

            End If

        ElseIf ipolicyid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " SET spolicydescription = '" & txtConcepto.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipolicyid = " & ipolicyid) = True Then
                btnInsertar.Enabled = True
                btnEliminar.Enabled = True
            End If

        End If


        'Empieza código de Insertar Movimiento

        If cmbTipo.SelectedItem.ToString = "Ingresos" Then

            Dim bp As New BuscaIngresos

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Ingreso', " & bp.iincomeid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        ElseIf cmbTipo.SelectedItem.ToString = "Facturas de Combustible (Vales)" Then

            Dim bp As New BuscaFacturasCombustible

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Factura de Combustible', " & bp.isupplierinvoiceid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        ElseIf cmbTipo.SelectedItem.ToString = "Facturas de Proveedores" Then

            Dim bp As New BuscaFacturasProveedor

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Factura de Proveedor', " & bp.isupplierinvoiceid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        ElseIf cmbTipo.SelectedItem.ToString = "Gastos por Casetas" Then

            Dim bp As New BuscaGastosPorCasetas

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Gasto por Caseta', " & bp.ipaytollid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        ElseIf cmbTipo.SelectedItem.ToString = "Vales" Then

            Dim bp As New BuscaVales

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Vale de Combustible', " & bp.igasvoucherid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        ElseIf cmbTipo.SelectedItem.ToString = "Nominas" Then

            Dim bp As New BuscaNominas

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Nomina', " & bp.ipayrollid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        ElseIf cmbTipo.SelectedItem.ToString = "Pagos" Then

            Dim bp As New BuscaPagos

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

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements VALUES (" & ipolicyid & ", 'Pago', " & bp.ipaymentid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor




        Dim queryPolicyMovements As String = ""

        queryPolicyMovements = "" & _
        "SELECT pm.sdocumentname AS 'Documento', pm.iid, tmpC.sexpensedescription AS 'Descripcion', tmpC.fecha AS 'Fecha Movimiento', " & _
        "tmpC.tipo AS 'Tipo Movimiento', tmpC.ssuppliername AS 'Persona/Proveedor', tmpC.idadicional AS 'Folio/Identificador', " & _
        "FORMAT(tmpC.monto, 2) AS 'Total', CAST(tmpC.sexpenselocation AS CHAR) AS 'Lugar', tmpC.personafactura AS 'Responsable', tmpC.sprojectname AS 'Proyecto/Datos Adicionales', " & _
        "tmpC.cliente AS 'Cliente/Datos Adicionales', pm.sextranote AS 'Observaciones' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
        "JOIN ( " & _
        "SELECT * FROM ( " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
        "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
        "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
        "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
        "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
        "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
        "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
        "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "UNION " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
        "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
        "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
        "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
        "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
        "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
        "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
        "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
        "ORDER BY s.ssuppliername ASC)tmpA " & _
        "WHERE total > 0 " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
        "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
        "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
        "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
        "FROM gasvouchers g " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
        "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
        "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
        "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
        "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
        "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
        "FROM payrolls pr " & _
        "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
        "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
        "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
        "ic.dincomeamount AS 'monto', '', '', '', '' " & _
        "FROM incomes ic " & _
        "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
        "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
        "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
        "FROM paytolls ic " & _
        "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
        "  ) tmpB " & _
        ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
        "WHERE p.ipolicyid = " & ipolicyid & " ORDER BY 4 ASC, 3 ASC "

        setDataGridView(dgvMovimientos, queryPolicyMovements, False)

        dgvMovimientos.Columns(1).Visible = False

        dgvMovimientos.Columns(0).ReadOnly = True
        dgvMovimientos.Columns(1).ReadOnly = True
        dgvMovimientos.Columns(2).ReadOnly = True
        dgvMovimientos.Columns(3).ReadOnly = True
        dgvMovimientos.Columns(4).ReadOnly = True
        dgvMovimientos.Columns(5).ReadOnly = True
        dgvMovimientos.Columns(6).ReadOnly = True
        dgvMovimientos.Columns(7).ReadOnly = True
        dgvMovimientos.Columns(8).ReadOnly = True
        dgvMovimientos.Columns(9).ReadOnly = True
        dgvMovimientos.Columns(10).ReadOnly = True
        dgvMovimientos.Columns(11).ReadOnly = True
        dgvMovimientos.Columns(11).ReadOnly = False

        dgvMovimientos.Columns(0).Width = 130
        dgvMovimientos.Columns(2).Width = 200
        dgvMovimientos.Columns(3).Width = 130
        dgvMovimientos.Columns(4).Width = 120
        dgvMovimientos.Columns(5).Width = 200
        dgvMovimientos.Columns(6).Width = 70
        dgvMovimientos.Columns(7).Width = 70
        dgvMovimientos.Columns(8).Width = 150
        dgvMovimientos.Columns(9).Width = 150
        dgvMovimientos.Columns(10).Width = 250
        dgvMovimientos.Columns(11).Width = 250
        dgvMovimientos.Columns(12).Width = 200




        Dim queryTotalPoliza As String = ""

        queryTotalPoliza = "" & _
        "SELECT FORMAT(SUM(tmpC.monto), 2) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
        "JOIN ( " & _
        "SELECT * FROM ( " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
        "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
        "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
        "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
        "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
        "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
        "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
        "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
        "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
        "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "UNION " & _
        "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
        "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
        "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
        "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
        "FROM ( " & _
        "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
        "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
        "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
        "FROM supplierinvoices si " & _
        "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
        "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
        "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
        "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
        "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
        "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
        "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
        "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
        "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
        "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
        "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
        "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
        "ORDER BY s.ssuppliername ASC)tmpA " & _
        "WHERE total > 0 " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
        "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
        "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
        "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
        "FROM gasvouchers g " & _
        "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
        "JOIN assets a ON g.iassetid = a.iassetid " & _
        "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
        "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
        "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
        "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
        "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
        "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
        "FROM payrolls pr " & _
        "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
        "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
        "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
        "GROUP BY 1 " & _
        "UNION " & _
        "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
        "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
        "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
        "FROM payments py " & _
        "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
        "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
        "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
        "ic.dincomeamount AS 'monto', '', '', '', '' " & _
        "FROM incomes ic " & _
        "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
        "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
        "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
        "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
        "UNION " & _
        "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
        "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
        "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
        "FROM paytolls ic " & _
        "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
        "  ) tmpB " & _
        ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
        "WHERE p.ipolicyid = " & ipolicyid

        txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, queryTotalPoliza), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")



        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If ipolicyid = 0 Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(12) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements"
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid

            If executeTransactedSQLCommand(0, queriesNewId) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " VALUES ( " & ipolicyid + newIdAddition & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtConcepto.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                ipolicyid = ipolicyid + newIdAddition

                btnInsertar.Enabled = True
                btnEliminar.Enabled = True

            End If

        ElseIf ipolicyid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " SET spolicydescription = '" & txtConcepto.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipolicyid = " & ipolicyid) = True Then
                btnInsertar.Enabled = True
                btnEliminar.Enabled = True
            End If

        End If

        'Empieza código de Eliminar Póliza

        If MsgBox("¿Está seguro que deseas eliminar este Movimiento de la Póliza?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Movimiento de la Póliza") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpsselectedtypeid As String = ""
            Dim tmpiselectedid As Integer = 0

            Try

                tmpsselectedtypeid = dgvMovimientos.CurrentRow.Cells(0).Value()
                tmpiselectedid = dgvMovimientos.CurrentRow.Cells(1).Value()

            Catch ex As Exception

                tmpsselectedtypeid = ""
                tmpiselectedid = 0

            End Try

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements WHERE ipolicyid = " & ipolicyid & " AND sdocumentname = '" & tmpsselectedtypeid & "' AND iid = " & tmpiselectedid)




            Dim queryPolicyMovements As String = ""

            queryPolicyMovements = "" & _
            "SELECT pm.sdocumentname AS 'Documento', pm.iid, tmpC.sexpensedescription AS 'Descripcion', tmpC.fecha AS 'Fecha Movimiento', " & _
            "tmpC.tipo AS 'Tipo Movimiento', tmpC.ssuppliername AS 'Persona/Proveedor', tmpC.idadicional AS 'Folio/Identificador', " & _
            "FORMAT(tmpC.monto, 2) AS 'Total', CAST(tmpC.sexpenselocation AS CHAR) AS 'Lugar', tmpC.personafactura AS 'Responsable', tmpC.sprojectname AS 'Proyecto/Datos Adicionales', " & _
            "tmpC.cliente AS 'Cliente/Datos Adicionales', pm.sextranote AS 'Observaciones' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
            "JOIN ( " & _
            "SELECT * FROM ( " & _
            "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
            "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
            "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
            "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
            "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
            "FROM ( " & _
            "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
            "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
            "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
            "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
            "GROUP BY si.isupplierinvoiceid " & _
            "UNION " & _
            "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
            "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
            "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
            "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
            "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
            "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
            "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "GROUP BY si.isupplierinvoiceid " & _
            ") tmpA " & _
            "UNION " & _
            "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
            "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
            "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
            "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
            "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
            "FROM ( " & _
            "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
            "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
            "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
            "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
            "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
            "ORDER BY s.ssuppliername ASC)tmpA " & _
            "WHERE total > 0 " & _
            "GROUP BY 1 " & _
            "UNION " & _
            "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
            "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
            "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
            "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
            "FROM gasvouchers g " & _
            "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
            "JOIN assets a ON g.iassetid = a.iassetid " & _
            "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
            "UNION " & _
            "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
            "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
            "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
            "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
            "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
            "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
            "FROM payrolls pr " & _
            "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
            "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
            "GROUP BY 1 " & _
            "UNION " & _
            "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
            "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
            "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
            "FROM payments py " & _
            "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
            "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
            "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
            "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
            "UNION " & _
            "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
            "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
            "ic.dincomeamount AS 'monto', '', '', '', '' " & _
            "FROM incomes ic " & _
            "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
            "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
            "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
            "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
            "UNION " & _
            "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
            "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
            "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
            "FROM paytolls ic " & _
            "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
            "  ) tmpB " & _
            ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
            "WHERE p.ipolicyid = " & ipolicyid & " ORDER BY 4 ASC, 3 ASC "

            setDataGridView(dgvMovimientos, queryPolicyMovements, False)

            dgvMovimientos.Columns(1).Visible = False

            dgvMovimientos.Columns(0).ReadOnly = True
            dgvMovimientos.Columns(1).ReadOnly = True
            dgvMovimientos.Columns(2).ReadOnly = True
            dgvMovimientos.Columns(3).ReadOnly = True
            dgvMovimientos.Columns(4).ReadOnly = True
            dgvMovimientos.Columns(5).ReadOnly = True
            dgvMovimientos.Columns(6).ReadOnly = True
            dgvMovimientos.Columns(7).ReadOnly = True
            dgvMovimientos.Columns(8).ReadOnly = True
            dgvMovimientos.Columns(9).ReadOnly = True
            dgvMovimientos.Columns(10).ReadOnly = True
            dgvMovimientos.Columns(11).ReadOnly = True
            dgvMovimientos.Columns(11).ReadOnly = False

            dgvMovimientos.Columns(0).Width = 130
            dgvMovimientos.Columns(2).Width = 200
            dgvMovimientos.Columns(3).Width = 130
            dgvMovimientos.Columns(4).Width = 120
            dgvMovimientos.Columns(5).Width = 200
            dgvMovimientos.Columns(6).Width = 70
            dgvMovimientos.Columns(7).Width = 70
            dgvMovimientos.Columns(8).Width = 150
            dgvMovimientos.Columns(9).Width = 150
            dgvMovimientos.Columns(10).Width = 250
            dgvMovimientos.Columns(11).Width = 250
            dgvMovimientos.Columns(12).Width = 200




            Dim queryTotalPoliza As String = ""

            queryTotalPoliza = "" & _
            "SELECT FORMAT(SUM(tmpC.monto), 2) " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements pm " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " p ON pm.ipolicyid = p.ipolicyid " & _
            "JOIN ( " & _
            "SELECT * FROM ( " & _
            "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Proveedor' AS sdocumentname, " & _
            "tmpA.sexpensedescription, STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', " & _
            "tmpA.ssupplierinvoicetypedescription AS 'tipo', tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', " & _
            "tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, tmpA.speoplefullname AS 'personafactura', " & _
            "IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
            "FROM ( " & _
            "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
            "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
            "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, " & _
            "si.iupdatedate, si.supdatetime, SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
            "GROUP BY si.isupplierinvoiceid " & _
            "UNION " & _
            "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
            "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, " & _
            "sit.ssupplierinvoicetypedescription, si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, " & _
            "si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
            "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, " & _
            "IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN supplierinvoicediscounts sid ON sid.isupplierinvoiceid = si.isupplierinvoiceid " & _
            "JOIN supplierinvoicetypes sit ON si.isupplierinvoicetypeid = sit.isupplierinvoicetypeid " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.isupplierinvoiceid = si.isupplierinvoiceid AND sii.iinputid = sip.iinputid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "GROUP BY si.isupplierinvoiceid " & _
            ") tmpA " & _
            "UNION " & _
            "SELECT tmpA.isupplierinvoiceid AS idfactura, 'Factura de Combustible' AS sdocumentname, tmpA.sexpensedescription, " & _
            "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'fecha', 'Factura de Combustible', " & _
            "tmpA.ssuppliername, tmpA.ssupplierinvoicefolio AS 'idadicional', tmpA.total*-1 AS 'monto', tmpA.sexpenselocation, " & _
            "tmpA.speoplefullname AS 'personafactura', IF(tmpA.sprojectname IS NULL, '', tmpA.sprojectname) AS sprojectname, " & _
            "IF(tmpA.cliente IS NULL, '', tmpA.cliente) AS cliente " & _
            "FROM ( " & _
            "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, si.sexpensedescription, si.ssupplierinvoicefolio, si.sexpenselocation, " & _
            "pe1.speoplefullname, si.iinvoicedate, si.sinvoicetime, IF(SUM(gv.dgasvoucheramount) IS NULL, 0, SUM(gv.dgasvoucheramount)) AS total, " & _
            "IF(p.sprojectname IS NULL, '', CONCAT(p.sprojectname, ', ', p.sterrainlocation)) AS sprojectname, IF(pe4.speoplefullname IS NULL, '', pe4.speoplefullname) AS cliente " & _
            "FROM supplierinvoices si " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoicegasvouchers sigv ON si.isupplierinvoiceid = sigv.isupplierinvoiceid " & _
            "LEFT JOIN gasvouchers gv ON sigv.igasvoucherid = gv.igasvoucherid " & _
            "LEFT JOIN supplierinvoicepayments sipy ON si.isupplierinvoiceid = sipy.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sipy.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN gasvoucherprojects gvp ON gv.igasvoucherid = gvp.igasvoucherid " & _
            "LEFT JOIN projects p ON gvp.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN assets a ON a.iassetid = gv.iassetid " & _
            "GROUP BY si.isupplierinvoiceid, spn.ssupplierphonenumber, ppn.speoplephonenumber " & _
            "ORDER BY s.ssuppliername ASC)tmpA " & _
            "WHERE total > 0 " & _
            "GROUP BY 1 " & _
            "UNION " & _
            "SELECT g.igasvoucherid, 'Vale de Combustible' AS sdocumentname, a.sassetdescription, " & _
            "STR_TO_DATE(CONCAT(g.igasdate, ' ', g.sgastime), '%Y%c%d %T') AS fecha, gt.sgastype AS 'tipogas', " & _
            "p.speoplefullname AS sexpenselocation, CONCAT(FORMAT(g.dlitersdispensed, 3), ' lts') AS 'litros', g.dgasvoucheramount*-1 AS 'montovale', " & _
            "g.scarorigindestination, CONCAT('Kilometraje : ', g.scarmileageatrequest) AS kms, CONCAT('Comentarios : ', g.svouchercomment), '' " & _
            "FROM gasvouchers g " & _
            "JOIN gastypes gt ON g.igastypeid = gt.igastypeid " & _
            "JOIN assets a ON g.iassetid = a.iassetid " & _
            "LEFT JOIN people p ON g.ipeopleid = p.ipeopleid " & _
            "UNION " & _
            "SELECT pr.ipayrollid,'Nomina' AS sdocumentname, IF(pr.spayrolldescription IS NULL, '', pr.spayrolldescription) AS spayrolldescription, " & _
            "STR_TO_DATE(CONCAT(pr.ipayrolldate, ' ', pr.spayrolltime), '%Y%c%d %T') AS 'fecha', pr.spayrolltype AS 'tiponomina', p.sprojectname AS 'proyecto', " & _
            "'', IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount))*-1 AS 'total', " & _
            "CONCAT('Sin descuentos de Nomina : ', FORMAT(IF(SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount - prpe.ddiscountamount) IS NULL, 0, SUM((prpe.ddaysworked * prpe.ddaysalary) + (prpe.dextrahours * prpe.dextrahoursalary) + prpe.dextraincomeamount))*-1, 2)) AS 'totalsindescuentos', " & _
            "IF(pe.speoplefullname IS NULL, '', pe.speoplefullname) AS responsable, CONCAT('Frecuencia : ', pr.spayrollfrequency) AS 'frecuencia', " & _
            "CONCAT('Desde ', STR_TO_DATE(CONCAT(pr.ipayrollstartdate, ' ', pr.spayrollstarttime), '%Y%c%d %T'), ' hasta ', STR_TO_DATE(CONCAT(pr.ipayrollenddate, ' ', pr.spayrollendtime), '%Y%c%d %T')) AS duracion " & _
            "FROM payrolls pr " & _
            "JOIN payrollpeople prpe ON pr.ipayrollid = prpe.ipayrollid " & _
            "LEFT JOIN projects p ON pr.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe ON pr.ipeopleid = pe.ipeopleid " & _
            "GROUP BY 1 " & _
            "UNION " & _
            "SELECT py.ipaymentid, 'Pago' AS sdocumentname, py.spaymentdescription, STR_TO_DATE(CONCAT(py.ipaymentdate, ' ', py.spaymenttime), '%Y%c%d %T') AS 'fecha', " & _
            "pyty.spaymenttypedescription AS 'tipopago', p.speoplefullname AS sexpenselocation, '', " & _
            "py.dpaymentamount*-1 AS 'monto', '', '', CONCAT('De Banco ', ba.sbankname, ' Cuenta ', ca.scompanyaccountname, IF (py.sdestinationaccount = '' OR py.sdestinationreference = '', '', CONCAT(' a Cuenta ', py.sdestinationaccount, ' con Referencia ', py.sdestinationreference))) AS 'movimiento', '' " & _
            "FROM payments py " & _
            "JOIN paymenttypes pyty ON py.ipaymenttypeid = pyty.ipaymenttypeid " & _
            "LEFT JOIN banks ba ON py.idestinationbankid = ba.ibankid " & _
            "LEFT JOIN companyaccounts ca ON py.ioriginaccountid = ca.iaccountid " & _
            "LEFT JOIN people p ON py.ipeopleid = p.ipeopleid " & _
            "UNION " & _
            "SELECT ic.iincomeid, 'Ingreso' AS sdocumentname, ic.sincomedescription, STR_TO_DATE(CONCAT(ic.iincomedate, ' ', ic.sincometime), '%Y%c%d %T') AS 'fecha', " & _
            "icty.sincometypedescription AS 'tipoingreso', p.speoplefullname AS sexpenselocation, IF(CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname) IS NULL, '', CONCAT('De ', ba.sbankname, ' Cuenta ', ic.soriginaccount, ' Referencia ', ic.soriginreference, ' a Cuenta ', ca.scompanyaccountname)) AS 'movimiento', " & _
            "ic.dincomeamount AS 'monto', '', '', '', '' " & _
            "FROM incomes ic " & _
            "JOIN incometypes icty ON ic.iincometypeid = icty.iincometypeid " & _
            "LEFT JOIN banks ba ON ic.ioriginbankid = ba.ibankid " & _
            "LEFT JOIN companyaccounts ca ON ic.idestinationaccountid = ca.iaccountid " & _
            "LEFT JOIN people p ON ic.ireceiverid = p.ipeopleid " & _
            "UNION " & _
            "SELECT ic.ipaytollid, 'Gasto por Caseta' AS sdocumentname, CONCAT(ic.sorigin, ' ', ic.sdestination) AS 'caseta', " & _
            "STR_TO_DATE(CONCAT(ic.ipaytolldate, ' ', ic.spaytolltime), '%Y%c%d %T') AS 'fecha', 'Salida en Efectivo', " & _
            "p.speoplefullname AS sexpenselocation, '', ic.dpaytollamount*-1 AS 'monto', '', '', '', '' " & _
            "FROM paytolls ic " & _
            "LEFT JOIN people p ON ic.ipeopleid = p.ipeopleid " & _
            "  ) tmpB " & _
            ") tmpC ON pm.ipolicyid = p.ipolicyid AND pm.sdocumentname = tmpC.sdocumentname AND pm.iid = tmpC.idfactura " & _
            "WHERE p.ipolicyid = " & ipolicyid

            txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, queryTotalPoliza), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")



            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesPolicyMemberIsOpen As Integer = 1

        timesPolicyMemberIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid & "'")

        If timesPolicyMemberIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Póliza. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Póliza?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesPolicyMemberIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(12) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements"
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                ipolicyid = ipolicyid + newIdAddition
            End If

        End If


        Dim queriesSave(7) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM policies " & _
        "WHERE ipolicyid = " & ipolicyid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp WHERE policies.ipolicyid = tp.ipolicyid) "

        queriesSave(1) = "" & _
        "DELETE " & _
        "FROM policymovements " & _
        "WHERE ipolicyid = " & ipolicyid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp WHERE policymovements.ipolicyid = tp.ipolicyid AND policymovements.sdocumentname = tp.sdocumentname AND policymovements.iid = tp.iid) "

        queriesSave(2) = "" & _
        "UPDATE policies p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp ON tp.ipolicyid = p.ipolicyid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.ipolicydate = tp.ipolicydate, p.spolicytime = tp.spolicytime, p.spolicydescription = tp.spolicydescription WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(3) = "" & _
        "UPDATE policymovements p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp ON tp.ipolicyid = p.ipolicyid AND tp.sdocumentname = p.sdocumentname AND tp.iid = p.iid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sextranote = tp.sextranote WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(4) = "" & _
        "INSERT INTO policies " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM policies p WHERE p.ipolicyid = tp.ipolicyid AND p.ipolicyid = " & ipolicyid & ") "

        queriesSave(5) = "" & _
        "INSERT INTO policymovements " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements tp " & _
        "WHERE NOT EXISTS (SELECT * FROM policymovements p WHERE p.ipolicyid = tp.ipolicyid AND p.sdocumentname = tp.sdocumentname AND p.iid = tp.iid AND p.ipolicyid = " & ipolicyid & ") "

        queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Póliza " & ipolicyid & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then
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


    Private Sub txtConcepto_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtConcepto.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtConcepto.Text.Contains(arrayCaractProhib(carp)) Then
                txtConcepto.Text = txtConcepto.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtConcepto.Select(txtConcepto.Text.Length, 0)
        End If

    End Sub


    Private Sub txtConcepto_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConcepto.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If ipolicyid = 0 Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(12) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements"
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid

            If executeTransactedSQLCommand(0, queriesNewId) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " VALUES ( " & ipolicyid + newIdAddition & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtConcepto.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                ipolicyid = ipolicyid + newIdAddition
                spolicydescription = txtConcepto.Text.Replace("'", "").Replace("@", "").Replace("--", "")
                btnInsertar.Enabled = True
                btnEliminar.Enabled = True

            End If

        ElseIf ipolicyid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " SET spolicydescription = '" & txtConcepto.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipolicyid = " & ipolicyid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió la descripcion de la Poliza a " & preventSQLInjection(txtConcepto.Text.Replace("--", "").Replace("'", "")) & " en la Poliza " & ipolicyid & "', 'OK')")
                spolicydescription = txtConcepto.Text.Replace("'", "").Replace("@", "").Replace("--", "")
                btnInsertar.Enabled = True
                btnEliminar.Enabled = True
            End If

        End If

    End Sub


    Private Sub dtFechaPoliza_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtFechaPoliza.ValueChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If ipolicyid = 0 Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Policy" & ipolicyid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(12) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & "Movements RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements"
            queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid
            queriesNewId(3) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & "Movements SET ipolicyid = " & ipolicyid + newIdAddition & " WHERE ipolicyid = " & ipolicyid

            If executeTransactedSQLCommand(0, queriesNewId) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid + newIdAddition & " VALUES ( " & ipolicyid + newIdAddition & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(10).Trim.Replace(".000", "") & "', '" & txtConcepto.Text.Replace("'", "").Replace("@", "").Replace("--", "") & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                ipolicyid = ipolicyid + newIdAddition

                btnInsertar.Enabled = True
                btnEliminar.Enabled = True

            End If

        ElseIf ipolicyid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Policy" & ipolicyid & " SET ipolicydate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", spolicytime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(10).Trim.Replace(".000", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE ipolicyid = " & ipolicyid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió la Fecha de la Poliza a a " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaPoliza.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & " en la Poliza " & ipolicyid & "', 'OK')")
                btnInsertar.Enabled = True
                btnEliminar.Enabled = True
            End If

        End If

    End Sub


End Class