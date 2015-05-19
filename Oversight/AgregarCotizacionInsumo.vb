Public Class AgregarCotizacionInsumo

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
    Public isOpeningFromRecents As Boolean = False

    Private isFormReadyForAction As Boolean = False

    Public wasCreated As Boolean = False

    Public iestimationprojectid As Integer = 0
    Public iestimationid As Integer = 0
    Public iprojectid As Integer = 0
    Public iinputid As Integer = 0

    Public isupplierid As Integer = 0

    Private iselectedestimationid As Integer = 0
    Private iselectedsupplierid As Integer = 0
    Private dselectedinputqty As Double = 0.0
    Private iselectedpeopleid As Integer = 0
    Private sselectedextranote As String = ""
    Private sselecteddeadline As String = ""

    Private WithEvents txtNumeroDgvCotizacionesInsumo As TextBox
    Private WithEvents txtNotaDgvCotizacionesInsumo As TextBox
    Private WithEvents txtFechaDgvCotizacionesInsumo As TextBox
    Private WithEvents txtNothingDgvCotizacionesInsumo As TextBox
    Private txtNotaDgvCotizacionesInsumo_OldText As String = ""
    Private txtNumeroDgvCotizacionesInsumo_OldText As String = ""
    Private txtFechaDgvCotizacionesInsumo_OldText As String = ""
    Private txtNothingDgvCotizacionesInsumo_OldText As String = ""

    Private openPermission As Boolean = False
    Private alreadyDone As Boolean = False

    Private addEstimationPermission As Boolean = False
    Private modifyEstimationPermission As Boolean = False
    Private deleteEstimationPermission As Boolean = False
    Private selectEstimationPermission As Boolean = False

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

                If permission = "Agregar Cotizacion de Insumo" Then
                    addEstimationPermission = True
                    btnNuevaCotizacion.Enabled = True
                End If

                If permission = "Modificar Cotizacion de Insumo" Then
                    modifyEstimationPermission = True
                End If

                If permission = "Eliminar Cotizacion de Insumo" Then
                    deleteEstimationPermission = True
                    btnEliminarCotizacion.Enabled = True
                End If

                If permission = "Elegir Cotizacion de Insumo" Then
                    selectEstimationPermission = True
                    btnElegirCotizacionGanadora.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Cotizacion de Insumo', 'OK')")

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


    Private Sub AgregarCotizacionInsumo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierestimations " & _
        "WHERE iestimationprojectid = " & iestimationprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp WHERE supplierestimations.iestimationprojectid = tp.iestimationprojectid AND supplierestimations.iestimationid = tp.iestimationid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp JOIN supplierestimations p ON tp.iestimationprojectid = p.iestimationprojectid AND tp.iestimationid = p.iestimationid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierestimations p WHERE p.iestimationprojectid = tp.iestimationprojectid AND p.iestimationid = tp.iestimationid) ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0


        If validaCotizacionInsumo(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then
            incomplete = True
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Los datos de esta Cotizacion están incompletos. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then


            Dim timesEstimationIsOpen As Integer = 1

            timesEstimationIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierEstimation" & iestimationprojectid & "'")

            If timesEstimationIsOpen > 1 Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Cotizacion de Insumo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Cotizacion?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            End If

            Dim queriesSave(7) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM supplierestimations " & _
            "WHERE iestimationprojectid = " & iestimationprojectid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp WHERE supplierestimations.iestimationprojectid = tp.iestimationprojectid AND supplierestimations.iestimationid = tp.iestimationid) "

            queriesSave(2) = "" & _
            "UPDATE supplierestimations p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp ON tp.iestimationprojectid = p.iestimationprojectid AND tp.iestimationid = p.iestimationid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.iprojectid = tp.iprojectid, p.iinputid = tp.iinputid, p.dinputqty = tp.dinputqty, p.isupplierid = tp.isupplierid, p.sestimationextranote = tp.sestimationextranote, p.ipeopleid = tp.ipeopleid, p.dinputunitprice = tp.dinputunitprice, p.dinputtotalprice = tp.dinputtotalprice, p.ideadlinedate = tp.ideadlinedate, p.sdeadlinetime = tp.sdeadlinetime, p.iselected = tp.iselected WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(4) = "" & _
            "INSERT INTO supplierestimations " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierestimations p WHERE p.iestimationprojectid = tp.iestimationprojectid AND p.iestimationid = tp.iestimationid AND p.iestimationprojectid = " & iestimationprojectid & ") "

            queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Cotizacion " & iestimationprojectid & "', 'OK')"

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

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid
        queriesDelete(2) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Cotizacion " & iestimationprojectid & "', 'OK')"
        queriesDelete(3) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Estimation', 'Cotizacion de Insumo', '" & iestimationprojectid & "', 'Cotizacion de Materiales de Proyecto', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarCotizacionInsumo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarCotizacionInsumo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesEstimationIsOpen As Integer = 0

        timesEstimationIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierEstimation" & iestimationprojectid & "'")

        If timesEstimationIsOpen > 0 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Cotizacion de Insumo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo esta Cotizacion?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

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

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " ( `iestimationprojectid` int(11) NOT NULL, `iestimationid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `isupplierid` int(11) NOT NULL, `sestimationextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `ipeopleid` int(11) NOT NULL, `dinputunitprice` decimal(20,5) DEFAULT NULL, `dinputtotalprice` decimal(20,5) DEFAULT NULL, `ideadlinedate` int(11) NOT NULL, `sdeadlinetime` varchar(11) CHARACTER SET latin1 NOT NULL, `iselected` int(1) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`iestimationprojectid`, `iestimationid`), KEY `project` (`iprojectid`), KEY `iinputid` (`iinputid`), KEY `supplier` (`isupplierid`), KEY `people` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        Me.Text = "Cotizacion de Insumo - Proyecto " & getSQLQueryAsString(0, "SELECT sprojectname FROM projects WHERE iprojectid = " & iprojectid)

        If isEdit = False Then

            txtInsumo.Text = getSQLQueryAsString(0, "SELECT sinputdescription FROM inputs WHERE iinputid = " & iinputid)
            txtCantidad.Text = FormatCurrency("1.00", 3, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Else

            If isRecover = False Then

                Dim queriesInsert(6) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SELECT * FROM supplierestimations WHERE iestimationprojectid = " & iestimationprojectid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            Dim dsCotizacion As DataSet
            dsCotizacion = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid)

            Try

                If dsCotizacion.Tables(0).Rows.Count > 0 Then

                    txtInsumo.Text = getSQLQueryAsString(0, "SELECT sinputdescription FROM inputs WHERE iinputid = " & dsCotizacion.Tables(0).Rows(0).Item("iinputid"))
                    txtCantidad.Text = FormatCurrency(dsCotizacion.Tables(0).Rows(0).Item("dinputqty"), 3, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    If getSQLQueryAsBoolean(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid & " AND iselected = 1") = True Then
                        alreadyDone = True
                    End If

                    If alreadyDone = True Then

                        txtCantidad.Enabled = False
                        btnNuevaCotizacion.Enabled = False
                        btnEliminarCotizacion.Enabled = False
                        btnElegirCotizacionGanadora.Enabled = False

                        lblMejorSugerencia.Text = "PROVEEDOR GANADOR:"
                        lblMejorSugerencia.ForeColor = Color.Red
                        txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iselected = 1 ORDER BY se.iupdatedate DESC, se.supdatetime DESC LIMIT 1")
                        txtMejorSugerencia.ForeColor = Color.Red

                    Else
                        txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")
                    End If


                End If

            Catch ex As Exception

            End Try

        End If


        Dim queryEstimations As String = ""

        queryEstimations = "" & _
        "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, " & _
        "s.ssuppliername AS 'Proveedor', se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
        "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
        "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
        "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
        "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
        "WHERE se.iestimationprojectid = " & iestimationprojectid

        If alreadyDone = True Then
            setDataGridView(dgvCotizacionesInsumo, queryEstimations, True)
        Else
            setDataGridView(dgvCotizacionesInsumo, queryEstimations, False)
        End If

        dgvCotizacionesInsumo.Columns(0).Visible = False
        dgvCotizacionesInsumo.Columns(1).Visible = False
        dgvCotizacionesInsumo.Columns(2).Visible = False
        dgvCotizacionesInsumo.Columns(3).Visible = False
        dgvCotizacionesInsumo.Columns(4).Visible = False
        dgvCotizacionesInsumo.Columns(5).Visible = False
        dgvCotizacionesInsumo.Columns(8).Visible = False

        dgvCotizacionesInsumo.Columns(13).ReadOnly = True

        dgvCotizacionesInsumo.Columns(6).Width = 150
        dgvCotizacionesInsumo.Columns(7).Width = 100
        dgvCotizacionesInsumo.Columns(9).Width = 150
        dgvCotizacionesInsumo.Columns(10).Width = 70
        dgvCotizacionesInsumo.Columns(11).Width = 70
        dgvCotizacionesInsumo.Columns(12).Width = 120

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Cotizacion " & iestimationprojectid & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Estimation', 'Cotizacion de Insumo', '" & iestimationprojectid & "', 'Cotizacion de Materiales de Proyecto', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        txtInsumo.Select()
        txtInsumo.Focus()

        txtInsumo.SelectionStart() = txtInsumo.Text.Length

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


    Private Sub txtCantidad_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidad.KeyUp

        If Not IsNumeric(txtCantidad.Text) Then

            Dim strForbidden9 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden9 As Char() = strForbidden9.ToCharArray

            For cp9 = 0 To arrayForbidden9.Length - 1

                If txtCantidad.Text.Contains(arrayForbidden9(cp9)) Then
                    txtCantidad.Text = txtCantidad.Text.Replace(arrayForbidden9(cp9), "")
                End If

            Next cp9

            If txtCantidad.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidad.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidad.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidad.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidad.Text = txtCantidad.Text.Substring(0, lugar) & txtCantidad.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidad.Text = txtCantidad.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

        End If

        txtCantidad.Text = txtCantidad.Text.Replace("--", "").Replace("'", "")
        txtCantidad.Text = txtCantidad.Text.Trim

    End Sub


    Private Function validaCotizacionInsumo(ByVal silent As Boolean) As Boolean

        Return True

    End Function


    Private Sub dgvCotizacionesInsumo_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvCotizacionesInsumo.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedestimationid = CInt(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(1).Value())
            iselectedsupplierid = CInt(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(5).Value())
            sselectedextranote = dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(7).Value()
            iselectedpeopleid = CInt(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(8).Value())
            sselecteddeadline = dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(12).Value()

        Catch ex As Exception

            iselectedestimationid = 0
            iselectedsupplierid = 0
            sselectedextranote = ""
            iselectedpeopleid = 0
            sselecteddeadline = ""

        End Try

    End Sub


    Private Sub dgvCotizacionesInsumo_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try

            If dgvCotizacionesInsumo.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedestimationid = CInt(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(1).Value())
            iselectedsupplierid = CInt(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(5).Value())
            sselectedextranote = dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(7).Value()
            iselectedpeopleid = CInt(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(8).Value())
            sselecteddeadline = dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(12).Value()

        Catch ex As Exception

            iselectedestimationid = 0
            iselectedsupplierid = 0
            sselectedextranote = ""
            iselectedpeopleid = 0
            sselecteddeadline = ""

        End Try

    End Sub


    Private Sub dgvCotizacionesInsumo_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvCotizacionesInsumo.SelectionChanged

        Try

            If dgvCotizacionesInsumo.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedestimationid = CInt(dgvCotizacionesInsumo.CurrentRow.Cells(1).Value())
            iselectedsupplierid = CInt(dgvCotizacionesInsumo.CurrentRow.Cells(5).Value())
            sselectedextranote = dgvCotizacionesInsumo.CurrentRow.Cells(7).Value()
            iselectedpeopleid = CInt(dgvCotizacionesInsumo.CurrentRow.Cells(8).Value())
            sselecteddeadline = dgvCotizacionesInsumo.CurrentRow.Cells(12).Value()

        Catch ex As Exception

            iselectedestimationid = 0
            iselectedsupplierid = 0
            sselectedextranote = ""
            iselectedpeopleid = 0
            sselecteddeadline = ""

        End Try

    End Sub


    Private Sub dgvCotizacionesInsumo_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCotizacionesInsumo.CellEndEdit

        Dim queryEstimations As String = ""

        queryEstimations = "" & _
        "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, s.ssuppliername AS 'Proveedor', " & _
        "se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
        "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
        "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
        "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
        "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
        "WHERE se.iestimationprojectid = " & iestimationprojectid

        setDataGridView(dgvCotizacionesInsumo, queryEstimations, False)

        dgvCotizacionesInsumo.Columns(0).Visible = False
        dgvCotizacionesInsumo.Columns(1).Visible = False
        dgvCotizacionesInsumo.Columns(2).Visible = False
        dgvCotizacionesInsumo.Columns(3).Visible = False
        dgvCotizacionesInsumo.Columns(4).Visible = False
        dgvCotizacionesInsumo.Columns(5).Visible = False
        dgvCotizacionesInsumo.Columns(8).Visible = False

        dgvCotizacionesInsumo.Columns(13).ReadOnly = True

        dgvCotizacionesInsumo.Columns(6).Width = 150
        dgvCotizacionesInsumo.Columns(7).Width = 100
        dgvCotizacionesInsumo.Columns(9).Width = 150
        dgvCotizacionesInsumo.Columns(10).Width = 70
        dgvCotizacionesInsumo.Columns(11).Width = 70
        dgvCotizacionesInsumo.Columns(12).Width = 120

        txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")

    End Sub


    Private Sub dgvCotizacionesInsumo_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCotizacionesInsumo.CellValueChanged

        If modifyEstimationPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If e.ColumnIndex = 6 Then 'ssuppliername


            Dim bp As New BuscaProveedores

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

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET isupplierid = " & bp.isupplierid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            End If


        ElseIf e.ColumnIndex = 7 Then 'Nota


            dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            If dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                'Nothing

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET sestimationextranote = '" & dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            End If


        ElseIf e.ColumnIndex = 9 Then 'speoplefullname


            Dim bpe As New BuscaPersonas

            bpe.susername = susername
            bpe.bactive = bactive
            bpe.bonline = bonline
            bpe.suserfullname = suserfullname

            bpe.suseremail = suseremail
            bpe.susersession = susersession
            bpe.susermachinename = susermachinename
            bpe.suserip = suserip

            bpe.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                bpe.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            bpe.ShowDialog(Me)
            Me.Visible = True

            If bpe.DialogResult = Windows.Forms.DialogResult.OK Then

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET ipeopleid = " & bpe.ipeopleid & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            End If


        ElseIf e.ColumnIndex = 10 Then 'dinputunitprice


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            Dim dinputqty As Double = 1.0
            Try
                dinputqty = txtCantidad.Text.Replace("--", "").Replace("'", "")
            Catch ex As Exception
                dinputqty = 1.0
            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET dinputunitprice = " & tmpValor & ", dinputtotalprice = " & tmpValor * dinputqty & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")


        ElseIf e.ColumnIndex = 11 Then 'dinputtotalprice


            Dim tmpValor As Double = 0.0

            Try

                tmpValor = CDbl(dgvCotizacionesInsumo.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Catch ex As Exception

            End Try

            Dim dinputqty As Double = 1.0
            Try
                dinputqty = txtCantidad.Text.Replace("--", "").Replace("'", "")
            Catch ex As Exception
                dinputqty = 1.0
            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET dinputtotalprice = " & tmpValor & ", dinputunitprice = " & tmpValor / dinputqty & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")


        ElseIf e.ColumnIndex = 12 Then 'vigencia


            'Dim bf As New BuscaFecha

            'Me.Visible = False
            'bf.ShowDialog(Me)
            'Me.Visible = True

            'If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            '    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET ideadlinedate = " & bf.fecha & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            'End If


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvCotizacionesInsumo_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvCotizacionesInsumo.EditingControlShowing

        'If dgvCotizacionesInsumo.CurrentCell.ColumnIndex = 6 Or dgvCotizacionesInsumo.CurrentCell.ColumnIndex = 9 Then

        '    txtNothingdgvCotizacionesInsumo = CType(e.Control, TextBox)
        '    txtNothingdgvCotizacionesInsumo_OldText = txtNothingdgvCotizacionesInsumo.Text

        'If dgvCotizacionesInsumo.CurrentCell.ColumnIndex = 7 Then

        '    txtNotaDgvCotizacionesInsumo = CType(e.Control, TextBox)
        '    txtNotaDgvCotizacionesInsumo_OldText = txtNotaDgvCotizacionesInsumo.Text

        'Else
        If dgvCotizacionesInsumo.CurrentCell.ColumnIndex = 11 Then

            txtNumeroDgvCotizacionesInsumo = CType(e.Control, TextBox)
            txtNumeroDgvCotizacionesInsumo_OldText = txtNumeroDgvCotizacionesInsumo.Text

        ElseIf dgvCotizacionesInsumo.CurrentCell.ColumnIndex = 12 Then

            txtFechaDgvCotizacionesInsumo = CType(e.Control, TextBox)
            txtFechaDgvCotizacionesInsumo_OldText = txtFechaDgvCotizacionesInsumo.Text

        Else

            txtNumeroDgvCotizacionesInsumo = Nothing
            txtNumeroDgvCotizacionesInsumo_OldText = Nothing
            txtNotaDgvCotizacionesInsumo = Nothing
            txtNotaDgvCotizacionesInsumo_OldText = Nothing
            'txtNothingdgvCotizacionesInsumo = Nothing
            'txtNothingdgvCotizacionesInsumo_OldText = Nothing
            txtFechaDgvCotizacionesInsumo = Nothing
            txtFechaDgvCotizacionesInsumo_OldText = Nothing

        End If

    End Sub


    Private Sub txtNumeroDgvCotizacionesInsumo_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroDgvCotizacionesInsumo.KeyUp

        If Not IsNumeric(txtNumeroDgvCotizacionesInsumo.Text) Then

            Dim strForbidden3 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden3 As Char() = strForbidden3.ToCharArray

            For cp = 0 To arrayForbidden3.Length - 1

                If txtNumeroDgvCotizacionesInsumo.Text.Contains(arrayForbidden3(cp)) Then
                    txtNumeroDgvCotizacionesInsumo.Text = txtNumeroDgvCotizacionesInsumo.Text.Replace(arrayForbidden3(cp), "")
                End If

            Next cp

            If txtNumeroDgvCotizacionesInsumo.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtNumeroDgvCotizacionesInsumo.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtNumeroDgvCotizacionesInsumo.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtNumeroDgvCotizacionesInsumo.Text.Length

                        If longitud > (lugar + 1) Then
                            txtNumeroDgvCotizacionesInsumo.Text = txtNumeroDgvCotizacionesInsumo.Text.Substring(0, lugar) & txtNumeroDgvCotizacionesInsumo.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtNumeroDgvCotizacionesInsumo.Text = txtNumeroDgvCotizacionesInsumo.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtNumeroDgvCotizacionesInsumo.Text = txtNumeroDgvCotizacionesInsumo.Text.Replace("--", "").Replace("'", "")
            txtNumeroDgvCotizacionesInsumo.Text = txtNumeroDgvCotizacionesInsumo.Text.Trim

        Else
            txtNumeroDgvCotizacionesInsumo_OldText = txtNumeroDgvCotizacionesInsumo.Text
        End If

    End Sub


    Private Sub txtNotaDgvCotizacionesInsumo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNotaDgvCotizacionesInsumo.KeyUp

        Dim strcaracteresprohibidos As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNotaDgvCotizacionesInsumo.Text.Contains(arrayCaractProhib(carp)) Then
                txtNotaDgvCotizacionesInsumo.Text = txtNotaDgvCotizacionesInsumo.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNotaDgvCotizacionesInsumo.Select(txtNotaDgvCotizacionesInsumo.Text.Length, 0)
        End If

    End Sub


    Private Sub txtFechaDgvCotizacionesInsumo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFechaDgvCotizacionesInsumo.KeyUp

        Dim bf As New BuscaFecha

        Me.Visible = False
        bf.ShowDialog(Me)
        Me.Visible = True

        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET ideadlinedate = " & bf.fecha & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & iselectedestimationid)

            txtFechaDgvCotizacionesInsumo.Text = ""

            dgvCotizacionesInsumo.EndEdit()

        End If

    End Sub


    'Private Sub txtNothingdgvCotizacionesInsumo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNothingdgvCotizacionesInsumo.KeyUp

    '    Dim fecha As Integer = 0
    '    Dim hora As String = ""

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    Dim bp As New BuscaProveedores

    '    bp.susername = susername
    '    bp.bactive = bactive
    '    bp.bonline = bonline
    '    bp.suserfullname = suserfullname
    '    bp.suseremail = suseremail
    '    bp.susersession = susersession
    '    bp.susermachinename = susermachinename
    '    bp.suserip = suserip

    '    bp.querystring = txtNothingdgvCotizacionesInsumo.Text

    '    txtNothingdgvCotizacionesInsumo.Text = ""

    '    bp.isEdit = False

    'If Me.WindowState = FormWindowState.Maximized Then
    '    bp.WindowState = FormWindowState.Maximized
    'End If

    '    Me.Visible = False
    '    bp.ShowDialog(Me)
    '    Me.Visible = True

    '    If bp.DialogResult = Windows.Forms.DialogResult.OK Then

    '        MsgBox("Ahora selecciona/inserta la persona que te atendió", MsgBoxStyle.OkOnly, "")

    '        Dim bpe As New BuscaPersonas

    '        bpe.susername = susername
    '        bpe.bactive = bactive
    '        bpe.bonline = bonline
    '        bpe.suserfullname = suserfullname

    '        bpe.suseremail = suseremail
    '        bpe.susersession = susersession
    '        bpe.susermachinename = susermachinename
    '        bpe.suserip = suserip

    '        bpe.isEdit = False

    'If Me.WindowState = FormWindowState.Maximized Then
    '        bpe.WindowState = FormWindowState.Maximized
    '    End If

    '        Me.Visible = False
    '        bpe.ShowDialog(Me)
    '        Me.Visible = True

    '        If bpe.DialogResult = Windows.Forms.DialogResult.OK Then

    '            iselectedestimationid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iestimationid) + 1 IS NULL, 1, MAX(iestimationid) + 1) AS iestimationid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid)

    '            executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " VALUES (" & iestimationprojectid & ", " & iselectedestimationid & ", " & iprojectid & ", " & iinputid & ", " & dinputqty & ", " & bp.isupplierid & ", '', " & bpe.ipeopleid & ", 0.00, 0.00, " & fecha & ", '" & hora & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')")

    '        End If

    '    End If

    'End Sub


    'Private Sub dgvCotizacionesInsumo_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvCotizacionesInsumo.UserAddedRow

    '    If addEstimationPermission = False Then
    '        Exit Sub
    '    End If

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    iselectedestimationid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iestimationid) + 1 IS NULL, 1, MAX(iestimationid) + 1) AS iestimationid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid)

    '    dgvCotizacionesInsumo.CurrentRow.Cells(0).Value = iprojectestimationid
    '    dgvCotizacionesInsumo.CurrentRow.Cells(1).Value = iselectedestimationid
    '    dgvCotizacionesInsumo.CurrentRow.Cells(2).Value = iestimationid
    '    dgvCotizacionesInsumo.CurrentRow.Cells(3).Value = "1"
    '    dgvCotizacionesInsumo.CurrentRow.Cells(4).Value = "General"

    '    executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " VALUES (" & iestimationprojectid & ", " & iselectedestimationid & ", '1', 'General', " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    Private Sub dgvCotizacionesInsumo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvCotizacionesInsumo.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteEstimationPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar esta Cotizacion?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Cotizacion") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedestimationid As Integer = 0
                Try
                    tmpselectedestimationid = CInt(dgvCotizacionesInsumo.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & tmpselectedestimationid)

                Dim queryEstimations As String = ""

                queryEstimations = "" & _
                "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, s.ssuppliername AS 'Proveedor', " & _
                "se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
                "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
                "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
                "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
                "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
                "WHERE se.iestimationprojectid = " & iestimationprojectid

                setDataGridView(dgvCotizacionesInsumo, queryEstimations, False)

                dgvCotizacionesInsumo.Columns(0).Visible = False
                dgvCotizacionesInsumo.Columns(1).Visible = False
                dgvCotizacionesInsumo.Columns(2).Visible = False
                dgvCotizacionesInsumo.Columns(3).Visible = False
                dgvCotizacionesInsumo.Columns(4).Visible = False
                dgvCotizacionesInsumo.Columns(5).Visible = False
                dgvCotizacionesInsumo.Columns(8).Visible = False

                dgvCotizacionesInsumo.Columns(13).ReadOnly = True

                dgvCotizacionesInsumo.Columns(6).Width = 150
                dgvCotizacionesInsumo.Columns(7).Width = 100
                dgvCotizacionesInsumo.Columns(9).Width = 150
                dgvCotizacionesInsumo.Columns(10).Width = 70
                dgvCotizacionesInsumo.Columns(11).Width = 70
                dgvCotizacionesInsumo.Columns(12).Width = 120

                txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub btnNuevaCotizacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevaCotizacion.Click

        If alreadyDone = True Then
            MsgBox("Esta es una cotización ya terminada! Intenta crear una nueva si gustas.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        'Empieza código de Nueva Cotizacion

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaProveedores

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

            MsgBox("Ahora selecciona/inserta la persona que te atendió", MsgBoxStyle.OkOnly, "")

            Dim bpe As New BuscaPersonas

            bpe.susername = susername
            bpe.bactive = bactive
            bpe.bonline = bonline
            bpe.suserfullname = suserfullname

            bpe.suseremail = suseremail
            bpe.susersession = susersession
            bpe.susermachinename = susermachinename
            bpe.suserip = suserip

            bpe.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                bpe.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            bpe.ShowDialog(Me)
            Me.Visible = True

            If bpe.DialogResult = Windows.Forms.DialogResult.OK Then

                iselectedestimationid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iestimationid) + 1 IS NULL, 1, MAX(iestimationid) + 1) AS iestimationid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid)

                Dim dinputqty As Double = 0.0
                Try
                    dinputqty = txtCantidad.Text.Replace("--", "").Replace("'", "")
                Catch ex As Exception
                    dinputqty = 0.0
                End Try

                executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " VALUES (" & iestimationprojectid & ", " & iselectedestimationid & ", " & iprojectid & ", " & iinputid & ", " & dinputqty & ", " & bp.isupplierid & ", '', " & bpe.ipeopleid & ", 0.00, 0.00, " & fecha & ", '" & hora & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')")

            End If

        End If


        Dim queryEstimations As String = ""

        queryEstimations = "" & _
        "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, s.ssuppliername AS 'Proveedor', " & _
        "se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
        "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
        "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
        "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
        "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
        "WHERE se.iestimationprojectid = " & iestimationprojectid

        setDataGridView(dgvCotizacionesInsumo, queryEstimations, False)

        dgvCotizacionesInsumo.Columns(0).Visible = False
        dgvCotizacionesInsumo.Columns(1).Visible = False
        dgvCotizacionesInsumo.Columns(2).Visible = False
        dgvCotizacionesInsumo.Columns(3).Visible = False
        dgvCotizacionesInsumo.Columns(4).Visible = False
        dgvCotizacionesInsumo.Columns(5).Visible = False
        dgvCotizacionesInsumo.Columns(8).Visible = False

        dgvCotizacionesInsumo.Columns(13).ReadOnly = True

        dgvCotizacionesInsumo.Columns(6).Width = 150
        dgvCotizacionesInsumo.Columns(7).Width = 100
        dgvCotizacionesInsumo.Columns(9).Width = 150
        dgvCotizacionesInsumo.Columns(10).Width = 70
        dgvCotizacionesInsumo.Columns(11).Width = 70
        dgvCotizacionesInsumo.Columns(12).Width = 120

        txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarCotizacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarCotizacion.Click

        If alreadyDone = True Then
            MsgBox("Esta es una cotización ya terminada! Intenta crear una nueva si gustas.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        'Empieza código de Eliminar Cotizacion

        If MsgBox("¿Está seguro que deseas eliminar esta Cotizacion?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Cotizacion") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedestimationid As Integer = 0
            Try
                tmpselectedestimationid = CInt(dgvCotizacionesInsumo.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & tmpselectedestimationid)

            Dim queryEstimations As String = ""

            queryEstimations = "" & _
            "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, s.ssuppliername AS 'Proveedor', " & _
            "se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
            "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
            "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
            "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
            "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
            "WHERE se.iestimationprojectid = " & iestimationprojectid

            setDataGridView(dgvCotizacionesInsumo, queryEstimations, False)

            dgvCotizacionesInsumo.Columns(0).Visible = False
            dgvCotizacionesInsumo.Columns(1).Visible = False
            dgvCotizacionesInsumo.Columns(2).Visible = False
            dgvCotizacionesInsumo.Columns(3).Visible = False
            dgvCotizacionesInsumo.Columns(4).Visible = False
            dgvCotizacionesInsumo.Columns(5).Visible = False
            dgvCotizacionesInsumo.Columns(8).Visible = False

            dgvCotizacionesInsumo.Columns(13).ReadOnly = True

            dgvCotizacionesInsumo.Columns(6).Width = 150
            dgvCotizacionesInsumo.Columns(7).Width = 100
            dgvCotizacionesInsumo.Columns(9).Width = 150
            dgvCotizacionesInsumo.Columns(10).Width = 70
            dgvCotizacionesInsumo.Columns(11).Width = 70
            dgvCotizacionesInsumo.Columns(12).Width = 120

            txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesEstimationIsOpen As Integer = 1

        timesEstimationIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierEstimation" & iestimationprojectid & "'")

        If timesEstimationIsOpen > 1 Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierta la misma Cotizacion de Insumo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir guardando esta Cotizacion?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        If iestimationprojectid = 0 Then

            Dim queriesCreation(7) As String

            iestimationprojectid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iestimationprojectid) + 1 IS NULL, 1, MAX(iestimationprojectid) + 1) AS iestimationprojectid FROM supplierestimations ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            queriesCreation(0) = "RENAME TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation0 TO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid
            queriesCreation(1) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET iestimationprojectid = " & iestimationprojectid & " WHERE iestimationprojectid = 0"

            executeTransactedSQLCommand(0, queriesCreation)

        End If


        Dim queriesSave(7) As String

        queriesSave(0) = "" & _
        "DELETE " & _
        "FROM supplierestimations " & _
        "WHERE iestimationprojectid = " & iestimationprojectid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp WHERE supplierestimations.iestimationprojectid = tp.iestimationprojectid AND supplierestimations.iestimationid = tp.iestimationid) "

        queriesSave(2) = "" & _
        "UPDATE supplierestimations p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp ON tp.iestimationprojectid = p.iestimationprojectid AND tp.iestimationid = p.iestimationid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.iprojectid = tp.iprojectid, p.iinputid = tp.iinputid, p.dinputqty = tp.dinputqty, p.isupplierid = tp.isupplierid, p.sestimationextranote = tp.sestimationextranote, p.ipeopleid = tp.ipeopleid, p.dinputunitprice = tp.dinputunitprice, p.dinputtotalprice = tp.dinputtotalprice, p.ideadlinedate = tp.ideadlinedate, p.sdeadlinetime = tp.sdeadlinetime, p.iselected = tp.iselected WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queriesSave(4) = "" & _
        "INSERT INTO supplierestimations " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierestimations p WHERE p.iestimationprojectid = tp.iestimationprojectid AND p.iestimationid = tp.iestimationid AND p.iestimationprojectid = " & iestimationprojectid & ") "

        queriesSave(6) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a la Cotizacion " & iestimationprojectid & "', 'OK')"

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


    Private Sub txtCantidad_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""
        Dim cantidad As Double = 0.0

        fecha = getMySQLDate()
        hora = getAppTime()

        Try
            cantidad = txtCantidad.Text.Replace("--", "").Replace("'", "")
        Catch ex As Exception

        End Try

        Dim queriesNewId(6) As String

        queriesNewId(0) = "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET dinputqty = " & cantidad & ", dinputtotalprice = " & cantidad & "*dinputunitprice, iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid

        If executeTransactedSQLCommand(0, queriesNewId) = True Then

            Dim queryEstimations As String = ""

            queryEstimations = "" & _
            "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, s.ssuppliername AS 'Proveedor', " & _
            "se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
            "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
            "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
            "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
            "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
            "WHERE se.iestimationprojectid = " & iestimationprojectid

            setDataGridView(dgvCotizacionesInsumo, queryEstimations, False)

            dgvCotizacionesInsumo.Columns(0).Visible = False
            dgvCotizacionesInsumo.Columns(1).Visible = False
            dgvCotizacionesInsumo.Columns(2).Visible = False
            dgvCotizacionesInsumo.Columns(3).Visible = False
            dgvCotizacionesInsumo.Columns(4).Visible = False
            dgvCotizacionesInsumo.Columns(5).Visible = False
            dgvCotizacionesInsumo.Columns(8).Visible = False

            dgvCotizacionesInsumo.Columns(13).ReadOnly = True

            dgvCotizacionesInsumo.Columns(6).Width = 150
            dgvCotizacionesInsumo.Columns(7).Width = 100
            dgvCotizacionesInsumo.Columns(9).Width = 150
            dgvCotizacionesInsumo.Columns(10).Width = 70
            dgvCotizacionesInsumo.Columns(11).Width = 70
            dgvCotizacionesInsumo.Columns(12).Width = 120

            txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iestimationprojectid = " & iestimationprojectid & " AND dinputunitprice > 0 ORDER BY dinputtotalprice ASC LIMIT 1")

        End If

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnElegirCotizacionGanadora_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElegirCotizacionGanadora.Click

        If alreadyDone = True Then
            MsgBox("Esta es una cotización ya terminada! Intenta crear una nueva si gustas.", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        'Empieza código de Eliminar Cotizacion

        If MsgBox("¿Está seguro que deseas marcar esta Cotizacion como la Ganadora?", MsgBoxStyle.YesNo, "Confirmación de Asignacion Afirmativa en Cotizacion") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedestimationid As Integer = 0
            Try
                tmpselectedestimationid = CInt(dgvCotizacionesInsumo.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " SET iselected = 1, iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE iestimationprojectid = " & iestimationprojectid & " AND iestimationid = " & tmpselectedestimationid)

            Dim queryEstimations As String = ""

            queryEstimations = "" & _
            "SELECT se.iestimationprojectid, se.iestimationid, se.iprojectid, se.iinputid, se.dinputqty, se.isupplierid, s.ssuppliername AS 'Proveedor', " & _
            "se.sestimationextranote AS 'Nota', se.ipeopleid, p.speoplefullname AS 'Persona que atendio', " & _
            "FORMAT(se.dinputunitprice, 3) AS 'Precio Unitario', FORMAT(se.dinputtotalprice, 3) AS 'Precio Total', " & _
            "STR_TO_DATE(CONCAT(se.ideadlinedate, ' ', se.sdeadlinetime), '%Y%c%d %T') AS 'Vigencia', IF(se.iselected = 1, 'Ganador seleccionado', '') AS 'Ganador Seleccionado' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se " & _
            "JOIN suppliers s ON s.isupplierid = se.isupplierid " & _
            "LEFT JOIN people p ON p.ipeopleid = se.ipeopleid " & _
            "WHERE se.iestimationprojectid = " & iestimationprojectid

            setDataGridView(dgvCotizacionesInsumo, queryEstimations, True)

            dgvCotizacionesInsumo.Columns(0).Visible = False
            dgvCotizacionesInsumo.Columns(1).Visible = False
            dgvCotizacionesInsumo.Columns(2).Visible = False
            dgvCotizacionesInsumo.Columns(3).Visible = False
            dgvCotizacionesInsumo.Columns(4).Visible = False
            dgvCotizacionesInsumo.Columns(5).Visible = False
            dgvCotizacionesInsumo.Columns(8).Visible = False

            dgvCotizacionesInsumo.Columns(13).ReadOnly = True

            dgvCotizacionesInsumo.Columns(6).Width = 150
            dgvCotizacionesInsumo.Columns(7).Width = 100
            dgvCotizacionesInsumo.Columns(9).Width = 150
            dgvCotizacionesInsumo.Columns(10).Width = 70
            dgvCotizacionesInsumo.Columns(11).Width = 70
            dgvCotizacionesInsumo.Columns(12).Width = 120

            txtCantidad.Enabled = False
            btnNuevaCotizacion.Enabled = False
            btnEliminarCotizacion.Enabled = False
            btnElegirCotizacionGanadora.Enabled = False

            lblMejorSugerencia.Text = "PROVEEDOR GANADOR:"
            lblMejorSugerencia.ForeColor = Color.Red
            txtMejorSugerencia.Text = getSQLQueryAsString(0, "SELECT s.ssuppliername FROM suppliers s JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierEstimation" & iestimationprojectid & " se ON s.isupplierid = se.isupplierid WHERE se.iselected = 1 ORDER BY se.iupdatedate DESC, se.supdatetime DESC LIMIT 1")
            txtMejorSugerencia.ForeColor = Color.Red

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If


    End Sub


End Class