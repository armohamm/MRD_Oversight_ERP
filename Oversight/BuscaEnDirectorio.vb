Public Class BuscaEnDirectorio

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

    Public stype As String = ""

    Public ipeopleid As Integer = 0
    Public speoplefullname As String = ""

    Public isupplierid As Integer = 0
    Public ssuppliername As String = ""
    Public ssupplierplace As String = ""

    Public isEdit As Boolean = False

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False
    Private viewObservations As Boolean = False

    Private viewPersonDebts As Boolean = False
    Private viewCompanyDebts As Boolean = False

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

                If permission = "Ver Observaciones" Then
                    viewObservations = True
                End If

                If permission = "Ver Debe" Then
                    viewPersonDebts = True
                End If

                If permission = "Ver Debemos" Then
                    viewCompanyDebts = True
                End If

                If permission = "Transformar" Then
                    btnTransformar.Visible = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Personas', 'OK')")

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


    Private Sub BuscaEnDirectorio_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Persona/Proveedor con la persona " & ipeopleid & " : " & speoplefullname & " seleccionada', 'OK')")

    End Sub


    Private Sub BuscaEnDirectorio_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaEnDirectorio_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        Me.AcceptButton = btnAbrir
        Me.CancelButton = btnCancelar

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        txtBuscar.Text = querystring

        Dim queryBusqueda As String = ""

        If chkInfoAdicional.Checked = False Then

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT pe.ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "pe.speoplefullname AS 'Nombre Completo', pe.speopleaddress AS 'Direccion', pe.speoplemail AS 'Email', " & _
            "pe.speopleobservations AS 'Observaciones', '' AS 'Debe $', '' AS 'Debemos $' " & _
            "FROM people pe " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN projects p ON p.ipeopleid = pe.ipeopleid " & _
            "WHERE " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY pe.ipeopleid " & _
            "UNION " & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "


            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70



        Else

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "speoplefullname AS 'Nombre Completo', speopleaddress AS 'Direccion', speoplemail AS 'Email', " & _
            "speopleobservations AS 'Observaciones', FORMAT(SUM(debe),3) AS 'Debe $', FORMAT(SUM(debemos),3) AS 'Debemos $' " & _
            "FROM ( " & _
            "SELECT p.iprojectid, pe.ipeopleid, pe.speoplefullname, pe.speopleaddress, pe.speoplemail, pe.speopleobservations, " & _
            "ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(1+p.dprojectIVApercentage))-IF(entradas.cash IS NULL, 0, entradas.cash) AS debe, " & _
            "IF(sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage) IS NULL, 0, sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage)) AS debemos " & _
            "FROM projects p " & _
            "JOIN projectcards ptf ON ptf.iprojectid = p.iprojectid " & _
            "JOIN cardlegacycategories ptflc ON ptflc.scardlegacycategoryid = ptf.scardlegacycategoryid " & _
            "JOIN people pe ON pe.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN (SELECT icp.iprojectid, SUM(ic.dincomeamount) AS cash FROM incomes ic JOIN incomeprojects icp ON icp.iincomeid = ic.iincomeid GROUP BY icp.iprojectid) AS entradas ON p.iprojectid = entradas.iprojectid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM projectcardinputs ptfi JOIN projectcards ptf ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.iprojectid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.iprojectid) AS costoEQ ON ptf.iprojectid = costoEQ.iprojectid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid) AS costoMO ON ptf.iprojectid = costoMO.iprojectid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM projectcardinputs ptfi JOIN projectcardcompoundinputs ptfci ON ptfci.iprojectid = ptfi.iprojectid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.iprojectid, ptfi.icardid ORDER BY ptfi.iprojectid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.iprojectid = costoMAT.iprojectid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE " & _
            "p.iprojectforecastedclosingdate > 0 AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "p.sprojectname LIKE '%" & querystring & "%' OR " & _
            "p.sprojectcompanyname LIKE '%" & querystring & "%' OR " & _
            "p.sprojecttype LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' OR " & _
            "p.sprojectfileslocation LIKE '%" & querystring & "%' OR " & _
            "p.slastmodelapplied LIKE '%" & querystring & "%' OR " & _
            "p.dprojectbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectIVApercentage LIKE '%" & querystring & "%' OR " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "ptf.scardlegacycategoryid LIKE '%" & querystring & "%' OR " & _
            "ptf.scarddescription LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%') OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY ptf.iprojectid, ptf.icardid " & _
            ") aux GROUP BY iprojectid " & _
            "UNION" & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "

            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70


        End If

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Persona/Proveedor En Directorio', 'OK')")

        dgvDirectorio.Select()

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

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtBuscar.Text.Contains(arrayCaractProhib(carp)) Then
                txtBuscar.Text = txtBuscar.Text.Replace(arrayCaractProhib(carp), "")
            End If

        Next carp

        txtBuscar.Text = txtBuscar.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        querystring = txtBuscar.Text

        Dim queryBusqueda As String = ""

        If chkInfoAdicional.Checked = False Then

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT pe.ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "pe.speoplefullname AS 'Nombre Completo', pe.speopleaddress AS 'Direccion', pe.speoplemail AS 'Email', " & _
            "pe.speopleobservations AS 'Observaciones', '' AS 'Debe $', '' AS 'Debemos $' " & _
            "FROM people pe " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN projects p ON p.ipeopleid = pe.ipeopleid " & _
            "WHERE " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY pe.ipeopleid " & _
            "UNION " & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "


            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70



        Else

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "speoplefullname AS 'Nombre Completo', speopleaddress AS 'Direccion', speoplemail AS 'Email', " & _
            "speopleobservations AS 'Observaciones', FORMAT(SUM(debe),3) AS 'Debe $', FORMAT(SUM(debemos),3) AS 'Debemos $' " & _
            "FROM ( " & _
            "SELECT p.iprojectid, pe.ipeopleid, pe.speoplefullname, pe.speopleaddress, pe.speoplemail, pe.speopleobservations, " & _
            "ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(1+p.dprojectIVApercentage))-IF(entradas.cash IS NULL, 0, entradas.cash) AS debe, " & _
            "IF(sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage) IS NULL, 0, sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage)) AS debemos " & _
            "FROM projects p " & _
            "JOIN projectcards ptf ON ptf.iprojectid = p.iprojectid " & _
            "JOIN cardlegacycategories ptflc ON ptflc.scardlegacycategoryid = ptf.scardlegacycategoryid " & _
            "JOIN people pe ON pe.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN (SELECT icp.iprojectid, SUM(ic.dincomeamount) AS cash FROM incomes ic JOIN incomeprojects icp ON icp.iincomeid = ic.iincomeid GROUP BY icp.iprojectid) AS entradas ON p.iprojectid = entradas.iprojectid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM projectcardinputs ptfi JOIN projectcards ptf ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.iprojectid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.iprojectid) AS costoEQ ON ptf.iprojectid = costoEQ.iprojectid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid) AS costoMO ON ptf.iprojectid = costoMO.iprojectid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM projectcardinputs ptfi JOIN projectcardcompoundinputs ptfci ON ptfci.iprojectid = ptfi.iprojectid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.iprojectid, ptfi.icardid ORDER BY ptfi.iprojectid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.iprojectid = costoMAT.iprojectid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE " & _
            "p.iprojectforecastedclosingdate > 0 AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "p.sprojectname LIKE '%" & querystring & "%' OR " & _
            "p.sprojectcompanyname LIKE '%" & querystring & "%' OR " & _
            "p.sprojecttype LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' OR " & _
            "p.sprojectfileslocation LIKE '%" & querystring & "%' OR " & _
            "p.slastmodelapplied LIKE '%" & querystring & "%' OR " & _
            "p.dprojectbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectIVApercentage LIKE '%" & querystring & "%' OR " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "ptf.scardlegacycategoryid LIKE '%" & querystring & "%' OR " & _
            "ptf.scarddescription LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%') OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY ptf.iprojectid, ptf.icardid " & _
            ") aux GROUP BY iprojectid " & _
            "UNION" & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "

            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub chkInfoAdicional_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInfoAdicional.CheckedChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim strcaracteresprohibidos As String = "|°!#$&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtBuscar.Text.Contains(arrayCaractProhib(carp)) Then
                txtBuscar.Text = txtBuscar.Text.Replace(arrayCaractProhib(carp), "")
            End If

        Next carp

        txtBuscar.Text = txtBuscar.Text.Replace("--", "").Replace("'", "")

        querystring = txtBuscar.Text

        Dim queryBusqueda As String = ""

        If chkInfoAdicional.Checked = False Then

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT pe.ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "pe.speoplefullname AS 'Nombre Completo', pe.speopleaddress AS 'Direccion', pe.speoplemail AS 'Email', " & _
            "pe.speopleobservations AS 'Observaciones', '' AS 'Debe $', '' AS 'Debemos $' " & _
            "FROM people pe " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN projects p ON p.ipeopleid = pe.ipeopleid " & _
            "WHERE " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY pe.ipeopleid " & _
            "UNION " & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "


            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70



        Else

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "speoplefullname AS 'Nombre Completo', speopleaddress AS 'Direccion', speoplemail AS 'Email', " & _
            "speopleobservations AS 'Observaciones', FORMAT(SUM(debe),3) AS 'Debe $', FORMAT(SUM(debemos),3) AS 'Debemos $' " & _
            "FROM ( " & _
            "SELECT p.iprojectid, pe.ipeopleid, pe.speoplefullname, pe.speopleaddress, pe.speoplemail, pe.speopleobservations, " & _
            "ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(1+p.dprojectIVApercentage))-IF(entradas.cash IS NULL, 0, entradas.cash) AS debe, " & _
            "IF(sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage) IS NULL, 0, sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage)) AS debemos " & _
            "FROM projects p " & _
            "JOIN projectcards ptf ON ptf.iprojectid = p.iprojectid " & _
            "JOIN cardlegacycategories ptflc ON ptflc.scardlegacycategoryid = ptf.scardlegacycategoryid " & _
            "JOIN people pe ON pe.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN (SELECT icp.iprojectid, SUM(ic.dincomeamount) AS cash FROM incomes ic JOIN incomeprojects icp ON icp.iincomeid = ic.iincomeid GROUP BY icp.iprojectid) AS entradas ON p.iprojectid = entradas.iprojectid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM projectcardinputs ptfi JOIN projectcards ptf ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.iprojectid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.iprojectid) AS costoEQ ON ptf.iprojectid = costoEQ.iprojectid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid) AS costoMO ON ptf.iprojectid = costoMO.iprojectid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM projectcardinputs ptfi JOIN projectcardcompoundinputs ptfci ON ptfci.iprojectid = ptfi.iprojectid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.iprojectid, ptfi.icardid ORDER BY ptfi.iprojectid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.iprojectid = costoMAT.iprojectid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE " & _
            "p.iprojectforecastedclosingdate > 0 AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "p.sprojectname LIKE '%" & querystring & "%' OR " & _
            "p.sprojectcompanyname LIKE '%" & querystring & "%' OR " & _
            "p.sprojecttype LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' OR " & _
            "p.sprojectfileslocation LIKE '%" & querystring & "%' OR " & _
            "p.slastmodelapplied LIKE '%" & querystring & "%' OR " & _
            "p.dprojectbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectIVApercentage LIKE '%" & querystring & "%' OR " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "ptf.scardlegacycategoryid LIKE '%" & querystring & "%' OR " & _
            "ptf.scarddescription LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%') OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY ptf.iprojectid, ptf.icardid " & _
            ") aux GROUP BY iprojectid " & _
            "UNION" & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "

            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70


        End If

        txtBuscar.Focus()
        txtBuscar.SelectionStart() = txtBuscar.Text.Length

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvDirectorio_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDirectorio.CellClick

        Try

            If dgvDirectorio.CurrentRow Is Nothing Then
                Exit Sub
            End If

            stype = dgvDirectorio.Rows(e.RowIndex).Cells(1).Value()

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

                ipeopleid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                speoplefullname = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                isupplierid = 0
                ssuppliername = ""
                ssupplierplace = ""

            Else

                isupplierid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                ssuppliername = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                ssupplierplace = dgvDirectorio.Rows(e.RowIndex).Cells(3).Value()
                ipeopleid = 0
                speoplefullname = ""

            End If

        Catch ex As Exception

            ipeopleid = 0
            isupplierid = 0
            speoplefullname = ""
            ssuppliername = ""
            ssupplierplace = ""

        End Try

    End Sub


    Private Sub dgvDirectorio_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDirectorio.CellContentClick

        Try

            If dgvDirectorio.CurrentRow Is Nothing Then
                Exit Sub
            End If

            stype = dgvDirectorio.Rows(e.RowIndex).Cells(1).Value()

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

                ipeopleid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                speoplefullname = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                isupplierid = 0
                ssuppliername = ""
                ssupplierplace = ""

            Else

                isupplierid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                ssuppliername = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                ssupplierplace = dgvDirectorio.Rows(e.RowIndex).Cells(3).Value()
                ipeopleid = 0
                speoplefullname = ""

            End If

        Catch ex As Exception

            ipeopleid = 0
            isupplierid = 0
            speoplefullname = ""
            ssuppliername = ""
            ssupplierplace = ""

        End Try

    End Sub


    Private Sub dgvDirectorio_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDirectorio.SelectionChanged

        Try

            If dgvDirectorio.CurrentRow Is Nothing Then
                Exit Sub
            End If

            stype = dgvDirectorio.CurrentRow.Cells(1).Value()

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

                ipeopleid = CInt(dgvDirectorio.CurrentRow.Cells(0).Value())
                speoplefullname = dgvDirectorio.CurrentRow.Cells(2).Value()
                isupplierid = 0
                ssuppliername = ""
                ssupplierplace = ""

            Else

                isupplierid = CInt(dgvDirectorio.CurrentRow.Cells(0).Value())
                ssuppliername = dgvDirectorio.CurrentRow.Cells(2).Value()
                ssupplierplace = dgvDirectorio.CurrentRow.Cells(3).Value()
                ipeopleid = 0
                speoplefullname = ""

            End If

        Catch ex As Exception

            ipeopleid = 0
            isupplierid = 0
            speoplefullname = ""
            ssuppliername = ""
            ssupplierplace = ""

        End Try

    End Sub


    Private Sub dgvDirectorio_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDirectorio.CellContentDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvDirectorio.CurrentRow Is Nothing Then
                Exit Sub
            End If

            stype = dgvDirectorio.Rows(e.RowIndex).Cells(1).Value()

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

                ipeopleid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                speoplefullname = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                isupplierid = 0
                ssuppliername = ""
                ssupplierplace = ""

            Else

                isupplierid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                ssuppliername = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                ssupplierplace = dgvDirectorio.Rows(e.RowIndex).Cells(3).Value()
                ipeopleid = 0
                speoplefullname = ""

            End If

        Catch ex As Exception

            ipeopleid = 0
            isupplierid = 0
            speoplefullname = ""
            ssuppliername = ""
            ssupplierplace = ""

        End Try

        If dgvDirectorio.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

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

                Dim ap As New AgregarProveedor

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isupplierid = isupplierid

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

            End If

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvDirectorio_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDirectorio.CellDoubleClick

        If openPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvDirectorio.CurrentRow Is Nothing Then
                Exit Sub
            End If

            stype = dgvDirectorio.Rows(e.RowIndex).Cells(1).Value()

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

                ipeopleid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                speoplefullname = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                isupplierid = 0
                ssuppliername = ""
                ssupplierplace = ""

            Else

                isupplierid = CInt(dgvDirectorio.Rows(e.RowIndex).Cells(0).Value())
                ssuppliername = dgvDirectorio.Rows(e.RowIndex).Cells(2).Value()
                ssupplierplace = dgvDirectorio.Rows(e.RowIndex).Cells(3).Value()
                ipeopleid = 0
                speoplefullname = ""

            End If

        Catch ex As Exception

            ipeopleid = 0
            isupplierid = 0
            speoplefullname = ""
            ssuppliername = ""
            ssupplierplace = ""

        End Try

        If dgvDirectorio.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

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

                Dim ap As New AgregarProveedor

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isupplierid = isupplierid

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

            End If

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        querystring = ""

        ipeopleid = 0
        isupplierid = 0
        speoplefullname = ""
        ssuppliername = ""
        ssupplierplace = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If dgvDirectorio.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            If stype = "Persona No Cliente" Or stype = "CLIENTE" Then

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

                Dim ap As New AgregarProveedor

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isupplierid = isupplierid

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

            End If

        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCrear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrear.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bedpn As New BuscaEnDirectorioPreguntaNuevo
        bedpn.susername = susername
        bedpn.bactive = bactive
        bedpn.bonline = bonline
        bedpn.suserfullname = suserfullname
        bedpn.suseremail = suseremail
        bedpn.susersession = susersession
        bedpn.susermachinename = susermachinename
        bedpn.suserip = suserip

        bedpn.ShowDialog(Me)

        If bedpn.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If bedpn.iselectedoption = 1 Then

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

                    Me.ipeopleid = ap.ipeopleid
                    Me.speoplefullname = ap.speoplefullname

                    If ap.wasCreated = True Then
                        Me.wasCreated = True
                    End If

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()

                End If

            ElseIf bedpn.iselectedoption = 2 Then

                Dim ap As New AgregarProveedor
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

                    Me.isupplierid = ap.isupplierid
                    Me.ssuppliername = ap.ssuppliername

                    If ap.wasCreated = True Then
                        Me.wasCreated = True
                    End If

                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnTransformar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransformar.Click

        If getSQLQueryAsInteger(0, "SELECT bsysadmin FROM userspecialattributes where susername = '" & susername & "'") = 0 Then
            If MsgBox("¿Está seguro de que desea transformar este contacto?", MsgBoxStyle.YesNo, "Confirmación Transformacion") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        If stype = "Persona No Cliente" Or stype = "CLIENTE" Then   'De Persona a Proveedor

            isupplierid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierid) + 1 IS NULL, 1, MAX(isupplierid) + 1) AS isupplierid FROM suppliers ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queries(4) As String

            queries(0) = "INSERT INTO suppliers SELECT " & isupplierid & ", speoplefullname, speoplefullname, speopleaddress, speopleaddress, '', speoplemail, speopleobservations, " & fecha & ", '" & hora & "', '" & susername & "' FROM people WHERE ipeopleid = " & ipeopleid
            queries(1) = "INSERT INTO supplierphonenumbers SELECT " & isupplierid & ", ipeoplephonenumberid, speoplephonenumber, speoplephonetype, " & fecha & ", '" & hora & "', '" & susername & "' FROM peoplephonenumbers WHERE ipeopleid = " & ipeopleid
            queries(2) = "DELETE FROM people WHERE ipeopleid = " & ipeopleid
            queries(3) = "DELETE FROM peoplephonenumbers WHERE ipeopleid = " & ipeopleid

            If executeTransactedSQLCommand(0, queries) = True Then   'De Proveedor a Persona
                isupplierid = 0
            End If

        Else

            ipeopleid = getSQLQueryAsInteger(0, "SELECT IF(MAX(ipeopleid) + 1 IS NULL, 1, MAX(ipeopleid) + 1) AS ipeopleid FROM people")

            Dim queries(4) As String

            queries(0) = "INSERT INTO people SELECT " & ipeopleid & ", IF(IF(ssuppliername = ssupplierofficialname, 1, 0)=1, ssuppliername, CONCAT(ssuppliername, '/', ssupplierofficialname)), 'M', IF(IF(ssupplieraddress = ssupplierofficialaddress, 1, 0)=1, ssupplieraddress, CONCAT(ssupplieraddress, '/', ssupplierofficialaddress)), ssupplieremail, ssupplierobservations, " & fecha & ", '" & hora & "', '" & susername & "' FROM suppliers WHERE isupplierid = " & isupplierid
            queries(1) = "INSERT INTO peoplephonenumbers SELECT " & ipeopleid & ", isupplierphonenumberid, ssupplierphonenumber, ssupplierphonetype, " & fecha & ", '" & hora & "', '" & susername & "' FROM supplierphonenumbers WHERE isupplierid = " & isupplierid
            queries(2) = "DELETE FROM suppliers WHERE isupplierid = " & isupplierid
            queries(3) = "DELETE FROM supplierphonenumbers WHERE isupplierid = " & isupplierid

            If executeTransactedSQLCommand(0, queries) = True Then
                ipeopleid = 0
            End If

        End If

        txtBuscar.Text = querystring

        Dim queryBusqueda As String = ""

        If chkInfoAdicional.Checked = False Then

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT pe.ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "pe.speoplefullname AS 'Nombre Completo', pe.speopleaddress AS 'Direccion', pe.speoplemail AS 'Email', " & _
            "pe.speopleobservations AS 'Observaciones', '' AS 'Debe $', '' AS 'Debemos $' " & _
            "FROM people pe " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN projects p ON p.ipeopleid = pe.ipeopleid " & _
            "WHERE " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY pe.ipeopleid " & _
            "UNION " & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "


            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70



        Else

            queryBusqueda = "" & _
            "SELECT * FROM ( " & _
            "SELECT ipeopleid AS 'ID', IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') AS 'Tipo', " & _
            "speoplefullname AS 'Nombre Completo', speopleaddress AS 'Direccion', speoplemail AS 'Email', " & _
            "speopleobservations AS 'Observaciones', FORMAT(SUM(debe),3) AS 'Debe $', FORMAT(SUM(debemos),3) AS 'Debemos $' " & _
            "FROM ( " & _
            "SELECT p.iprojectid, pe.ipeopleid, pe.speoplefullname, pe.speopleaddress, pe.speoplemail, pe.speopleobservations, " & _
            "ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(1+p.dprojectIVApercentage))-IF(entradas.cash IS NULL, 0, entradas.cash) AS debe, " & _
            "IF(sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage) IS NULL, 0, sii.dsupplierinvoiceinputtotalprice*(1+p.dprojectIVApercentage)) AS debemos " & _
            "FROM projects p " & _
            "JOIN projectcards ptf ON ptf.iprojectid = p.iprojectid " & _
            "JOIN cardlegacycategories ptflc ON ptflc.scardlegacycategoryid = ptf.scardlegacycategoryid " & _
            "JOIN people pe ON pe.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers pepn ON pe.ipeopleid = pepn.ipeopleid " & _
            "LEFT JOIN supplierinvoiceprojects sip ON sip.iprojectid = p.iprojectid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN (SELECT icp.iprojectid, SUM(ic.dincomeamount) AS cash FROM incomes ic JOIN incomeprojects icp ON icp.iincomeid = ic.iincomeid GROUP BY icp.iprojectid) AS entradas ON p.iprojectid = entradas.iprojectid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM projectcardinputs ptfi JOIN projectcards ptf ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.iprojectid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.iprojectid) AS costoEQ ON ptf.iprojectid = costoEQ.iprojectid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.iprojectid) AS costoMO ON ptf.iprojectid = costoMO.iprojectid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM projectcards ptf LEFT JOIN projectcardinputs ptfi ON ptf.iprojectid = ptfi.iprojectid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, iprojectid) pp ON ptfi.iprojectid = pp.iprojectid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.iprojectid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM projectcardinputs ptfi JOIN projectcardcompoundinputs ptfci ON ptfci.iprojectid = ptfi.iprojectid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM projectprices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, iprojectid) cipp ON cipp.iprojectid = ptfci.iprojectid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.iprojectid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.iprojectid = cipp.iprojectid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.iprojectid, ptfi.icardid ORDER BY ptfi.iprojectid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.iprojectid = costoMAT.iprojectid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE " & _
            "p.iprojectforecastedclosingdate > 0 AND ( " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(p.iprojectdate, ' ', p.sprojecttime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "p.sprojectname LIKE '%" & querystring & "%' OR " & _
            "p.sprojectcompanyname LIKE '%" & querystring & "%' OR " & _
            "p.sprojecttype LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' OR " & _
            "p.sprojectfileslocation LIKE '%" & querystring & "%' OR " & _
            "p.slastmodelapplied LIKE '%" & querystring & "%' OR " & _
            "p.dprojectbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealbuildingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectrealclosingcommission LIKE '%" & querystring & "%' OR " & _
            "p.dprojectIVApercentage LIKE '%" & querystring & "%' OR " & _
            "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe.speoplegender LIKE '%" & querystring & "%' OR " & _
            "pe.speopleaddress LIKE '%" & querystring & "%' OR " & _
            "pe.speoplemail LIKE '%" & querystring & "%' OR " & _
            "pe.speopleobservations LIKE '%" & querystring & "%' OR " & _
            "ptf.scardlegacycategoryid LIKE '%" & querystring & "%' OR " & _
            "ptf.scarddescription LIKE '%" & querystring & "%' OR " & _
            "pepn.speoplephonenumber LIKE '%" & querystring & "%') OR " & _
            "IF(p.sprojectname IS NULL, 'Persona No Cliente', 'CLIENTE') LIKE '%" & querystring & "%' " & _
            "GROUP BY ptf.iprojectid, ptf.icardid " & _
            ") aux GROUP BY iprojectid " & _
            "UNION" & _
            "SELECT s.isupplierid AS 'ID', 'Proveedor' AS Tipo, s.ssuppliername AS 'Nombre Comercial', " & _
            "s.ssupplieraddress AS 'Direccion', s.ssupplieremail AS 'Email', s.ssupplierobservations AS 'Observaciones', " & _
            "FORMAT(IF(SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage) IS NULL, 0, SUM(oi.dinputordertotalprice)*(1+o.dIVApercentage)) - IF(SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage) IS NULL, 0, SUM(shi.dinputtotalprice)*(1+sh.dIVApercentage)), 2) AS 'Debe $', " & _
            "FORMAT(IF(SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage)) IS NULL, 0, SUM(sii.dsupplierinvoiceinputtotalprice*(1+si.dIVApercentage))) - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS 'Debemos $' " & _
            "FROM suppliers s " & _
            "LEFT JOIN supplierphonenumbers spn ON spn.isupplierid = s.isupplierid " & _
            "LEFT JOIN suppliercontacts sc ON sc.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe1 ON sc.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN peoplephonenumbers ppn ON ppn.ipeopleid = pe1.ipeopleid " & _
            "LEFT JOIN supplierinvoices si ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN supplierinvoiceinputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN supplierinvoicepayments sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN payments py ON py.ipaymentid = sip.ipaymentid " & _
            "LEFT JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "LEFT JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN orders o ON o.isupplierid = s.isupplierid " & _
            "LEFT JOIN orderinputs oi ON oi.iorderid = o.iorderid " & _
            "LEFT JOIN shipmentsuppliers ss ON ss.isupplierid = s.isupplierid " & _
            "LEFT JOIN shipments sh ON sh.ishipmentid = ss.ishipmentid " & _
            "LEFT JOIN shipmentinputs shi ON shi.ishipmentid = sh.ishipmentid AND shi.iinputid = i.iinputid " & _
            "LEFT JOIN projects p ON sh.iprojectid = p.iprojectid " & _
            "LEFT JOIN people pe2 ON pe2.ipeopleid = sh.idriverid " & _
            "LEFT JOIN people pe3 ON pe3.ipeopleid = sh.ireceiverid " & _
            "LEFT JOIN people pe4 ON pe4.ipeopleid = p.ipeopleid " & _
            "LEFT JOIN shipmentcarused shcu ON shcu.ishipmentid = sh.ishipmentid " & _
            "LEFT JOIN assets a ON a.iassetid = shcu.iassetid " & _
            "WHERE " & _
            "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
            "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
            "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
            "pe1.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "spn.ssupplierphonenumber LIKE '%" & querystring & "%' OR " & _
            "ppn.speoplephonenumber LIKE '%" & querystring & "%' OR " & _
            "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
            "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
            "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
            "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
            "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
            "pe2.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe3.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
            "a.sassetdescription LIKE '%" & querystring & "%' OR " & _
            "'Proveedor' LIKE '%" & querystring & "%' OR " & _
            "p.sterrainlocation LIKE '%" & querystring & "%' " & _
            "GROUP BY s.isupplierid " & _
            ") tmpA " & _
            "ORDER BY 3 "

            setDataGridView(dgvDirectorio, queryBusqueda, True)

            dgvDirectorio.Columns(0).Visible = False

            If viewObservations = False Then
                dgvDirectorio.Columns(5).Visible = False
            End If

            If viewPersonDebts = False Then
                dgvDirectorio.Columns(6).Visible = False
            End If

            If viewCompanyDebts = False Then
                dgvDirectorio.Columns(7).Visible = False
            End If

            dgvDirectorio.Columns(0).Width = 20
            dgvDirectorio.Columns(1).Width = 100
            dgvDirectorio.Columns(2).Width = 200
            dgvDirectorio.Columns(3).Width = 200
            dgvDirectorio.Columns(4).Width = 200
            dgvDirectorio.Columns(5).Width = 200
            dgvDirectorio.Columns(6).Width = 70
            dgvDirectorio.Columns(7).Width = 70


        End If

    End Sub


End Class