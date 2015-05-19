Public Class BuscaFacturasProveedor

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

    Public isupplierinvoiceid As Integer = 0
    Public isupplierid As Integer = 0
    Public ssuppliername As String = ""
    Public ssupplierinvoicedescription As String = ""

    Public isEdit As Boolean = False

    Public wasCreated As Boolean = False

    Private openPermission As Boolean = False
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

                If permission = "Ver Importe" Then
                    viewAmount = True
                End If

                If permission = "Exportar" Then
                    btnExportar.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Buscar Proveedores', 'OK')")

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


    Private Sub BuscaFacturasProveedor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la ventana de Buscar Facturas de Proveedor con la Factura " & isupplierinvoiceid & " : " & ssupplierinvoicedescription & " : " & ssuppliername & " seleccionada', 'OK')")

    End Sub


    Private Sub BuscaFacturasProveedor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaFacturasProveedor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

        queryBusqueda = "" & _
        "SELECT tmpA.isupplierinvoiceid AS 'ID Factura', tmpA.isupplierid AS 'ID Proveedor', " & _
        "tmpA.ssuppliername AS 'Nombre Comercial', tmpA.sexpensedescription AS 'Descripcion Factura', " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'Fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'Tipo Factura', tmpA.ssupplierinvoicefolio AS 'Folio Factura', " & _
        "FORMAT(tmpA.total, 2) AS 'Monto Factura', tmpA.sexpenselocation AS 'Lugar donde se hizo la compra', " & _
        "tmpA.speoplefullname AS 'Persona que recibio la Factura', tmpA.srevisionresult AS 'Revisado hasta' " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, IF(re.srevisionresult IS NULL, '', re.srevisionresult) AS srevisionresult " & _
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
        "LEFT JOIN (SELECT re.irevisionid, re.sid, re.irevisionresultid, rer.srevisionresult FROM revisions re JOIN revisionresults rer ON rer.irevisionresultid = re.irevisionresultid WHERE re.srevisiondocument = 'Factura de Proveedor' ORDER BY re.irevisionresultid DESC) re ON re.sid = si.isupplierinvoiceid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "AND( " & _
        "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
        "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
        "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
        "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
        "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
        "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
        "p.sterrainlocation LIKE '%" & querystring & "%') " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(re.srevisionresult IS NULL, '', re.srevisionresult) AS srevisionresult " & _
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
        "LEFT JOIN (SELECT re.irevisionid, re.sid, re.irevisionresultid, rer.srevisionresult FROM revisions re JOIN revisionresults rer ON rer.irevisionresultid = re.irevisionresultid WHERE re.srevisiondocument = 'Factura de Proveedor' ORDER BY re.irevisionresultid DESC) re ON re.sid = si.isupplierinvoiceid " & _
        "WHERE " & _
        "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
        "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
        "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
        "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
        "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
        "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
        "p.sterrainlocation LIKE '%" & querystring & "%' OR " & _
        "sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR " & _
        "sid.dsupplierinvoicediscountpercentage LIKE '%" & querystring & "%' " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "ORDER BY 5 DESC "

        setDataGridView(dgvFacturas, queryBusqueda, True)

        dgvFacturas.Columns(0).Visible = False
        dgvFacturas.Columns(1).Visible = False

        If viewAmount = False Then
            dgvFacturas.Columns(5).Visible = False
        End If

        dgvFacturas.Columns(0).Width = 20
        dgvFacturas.Columns(1).Width = 20
        dgvFacturas.Columns(2).Width = 200
        dgvFacturas.Columns(3).Width = 200
        dgvFacturas.Columns(4).Width = 70
        dgvFacturas.Columns(5).Width = 70

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Ventana de Buscar Facturas de Proveedor', 'OK')")

        dgvFacturas.Select()

        txtBuscar.Select()
        txtBuscar.Focus()

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

        queryBusqueda = "" & _
        "SELECT tmpA.isupplierinvoiceid AS 'ID Factura', tmpA.isupplierid AS 'ID Proveedor', " & _
        "tmpA.ssuppliername AS 'Nombre Comercial', tmpA.sexpensedescription AS 'Descripcion Factura', " & _
        "STR_TO_DATE(CONCAT(tmpA.iinvoicedate, ' ', tmpA.sinvoicetime), '%Y%c%d %T') AS 'Fecha', " & _
        "tmpA.ssupplierinvoicetypedescription AS 'Tipo Factura', tmpA.ssupplierinvoicefolio AS 'Folio Factura', " & _
        "FORMAT(tmpA.total, 2) AS 'Monto Factura', tmpA.sexpenselocation AS 'Lugar donde se hizo la compra', " & _
        "tmpA.speoplefullname AS 'Persona que recibio la Factura', tmpA.srevisionresult AS 'Revisado hasta' " & _
        "FROM ( " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty)*(1+si.dIVApercentage) as total, IF(re.srevisionresult IS NULL, '', re.srevisionresult) AS srevisionresult " & _
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
        "LEFT JOIN (SELECT re.irevisionid, re.sid, re.irevisionresultid, rer.srevisionresult FROM revisions re JOIN revisionresults rer ON rer.irevisionresultid = re.irevisionresultid WHERE re.srevisiondocument = 'Factura de Proveedor' ORDER BY re.irevisionresultid DESC) re ON re.sid = si.isupplierinvoiceid " & _
        "WHERE si.isupplierinvoiceid NOT IN (SELECT isupplierinvoiceid FROM supplierinvoicediscounts) " & _
        "AND( " & _
        "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
        "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
        "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
        "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
        "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
        "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
        "p.sterrainlocation LIKE '%" & querystring & "%') " & _
        "GROUP BY si.isupplierinvoiceid " & _
        "UNION " & _
        "SELECT sip.iprojectid, si.isupplierid, s.ssuppliername, s.ssupplierofficialname, s.ssupplieraddress, s.ssupplierofficialaddress, " & _
        "s.ssupplierrfc, s.ssupplieremail, s.ssupplierobservations, si.isupplierinvoiceid, si.isupplierinvoicetypeid, sit.ssupplierinvoicetypedescription, " & _
        "si.ssupplierinvoicefolio, si.sexpensedescription, si.iinvoicedate, si.sinvoicetime, si.sexpenselocation, si.ipeopleid, pe.speoplefullname, si.iupdatedate, si.supdatetime, " & _
        "IF((SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage) IS NULL, 0, (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) - (SUM(sii.dsupplierinvoiceinputunitprice*sii.dinputqty) * sid.dsupplierinvoicediscountpercentage))*(1+si.dIVApercentage)) AS total, " & _
        "IF(re.srevisionresult IS NULL, '', re.srevisionresult) AS srevisionresult " & _
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
        "LEFT JOIN (SELECT re.irevisionid, re.sid, re.irevisionresultid, rer.srevisionresult FROM revisions re JOIN revisionresults rer ON rer.irevisionresultid = re.irevisionresultid WHERE re.srevisiondocument = 'Factura de Proveedor' ORDER BY re.irevisionresultid DESC) re ON re.sid = si.isupplierinvoiceid " & _
        "WHERE " & _
        "s.ssuppliername LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialname LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieraddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierofficialaddress LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierrfc LIKE '%" & querystring & "%' OR " & _
        "s.ssupplieremail LIKE '%" & querystring & "%' OR " & _
        "s.ssupplierobservations LIKE '%" & querystring & "%' OR " & _
        "pe.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "pe4.speoplefullname LIKE '%" & querystring & "%' OR " & _
        "CONCAT(STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T')) LIKE '%" & querystring & "%' OR " & _
        "si.ssupplierinvoicefolio LIKE '%" & querystring & "%' OR " & _
        "si.sexpenselocation LIKE '%" & querystring & "%' OR " & _
        "si.dIVApercentage LIKE '%" & querystring & "%' OR " & _
        "i.sinputdescription LIKE '%" & querystring & "%' OR " & _
        "p.sterrainlocation LIKE '%" & querystring & "%' OR " & _
        "sid.ssupplierinvoicediscountdescription LIKE '%" & querystring & "%' OR " & _
        "sid.dsupplierinvoicediscountpercentage LIKE '%" & querystring & "%' " & _
        "GROUP BY si.isupplierinvoiceid " & _
        ") tmpA " & _
        "ORDER BY 5 DESC "

        setDataGridView(dgvFacturas, queryBusqueda, True)

        dgvFacturas.Columns(0).Visible = False
        dgvFacturas.Columns(1).Visible = False

        If viewAmount = False Then
            dgvFacturas.Columns(5).Visible = False
        End If

        dgvFacturas.Columns(0).Width = 20
        dgvFacturas.Columns(1).Width = 20
        dgvFacturas.Columns(2).Width = 200
        dgvFacturas.Columns(3).Width = 200
        dgvFacturas.Columns(4).Width = 70
        dgvFacturas.Columns(5).Width = 70

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvFacturas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFacturas.CellClick

        Try

            If dgvFacturas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            isupplierinvoiceid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(0).Value())
            isupplierid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(1).Value())
            ssuppliername = dgvFacturas.Rows(e.RowIndex).Cells(2).Value()
            ssupplierinvoicedescription = dgvFacturas.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            isupplierinvoiceid = 0
            isupplierid = 0
            ssuppliername = ""
            ssupplierinvoicedescription = ""

        End Try

    End Sub


    Private Sub dgvFacturas_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFacturas.CellContentClick

        Try

            If dgvFacturas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            isupplierinvoiceid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(0).Value())
            isupplierid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(1).Value())
            ssuppliername = dgvFacturas.Rows(e.RowIndex).Cells(2).Value()
            ssupplierinvoicedescription = dgvFacturas.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            isupplierinvoiceid = 0
            isupplierid = 0
            ssuppliername = ""
            ssupplierinvoicedescription = ""

        End Try

    End Sub


    Private Sub dgvFacturas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvFacturas.SelectionChanged

        Try

            If dgvFacturas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            isupplierinvoiceid = CInt(dgvFacturas.CurrentRow.Cells(0).Value())
            isupplierid = CInt(dgvFacturas.CurrentRow.Cells(1).Value())
            ssuppliername = dgvFacturas.CurrentRow.Cells(2).Value()
            ssupplierinvoicedescription = dgvFacturas.CurrentRow.Cells(3).Value()

        Catch ex As Exception

            isupplierinvoiceid = 0
            isupplierid = 0
            ssuppliername = ""
            ssupplierinvoicedescription = ""

        End Try

    End Sub


    Private Sub dgvFacturas_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFacturas.CellContentDoubleClick

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvFacturas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            isupplierinvoiceid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(0).Value())
            isupplierid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(1).Value())
            ssuppliername = dgvFacturas.Rows(e.RowIndex).Cells(2).Value()
            ssupplierinvoicedescription = dgvFacturas.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            isupplierinvoiceid = 0
            isupplierid = 0
            ssuppliername = ""
            ssupplierinvoicedescription = ""

        End Try

        If dgvFacturas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim afp As New AgregarFacturaProveedor

            afp.susername = susername
            afp.bactive = bactive
            afp.bonline = bonline
            afp.suserfullname = suserfullname
            afp.suseremail = suseremail
            afp.susersession = susersession
            afp.susermachinename = susermachinename
            afp.suserip = suserip

            afp.isupplierinvoiceid = isupplierinvoiceid

            afp.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                afp.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            afp.ShowDialog(Me)
            Me.Visible = True

            If afp.DialogResult = Windows.Forms.DialogResult.OK Then

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


    Private Sub dgvFacturas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFacturas.CellDoubleClick

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvFacturas.CurrentRow Is Nothing Then
                Exit Sub
            End If

            isupplierinvoiceid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(0).Value())
            isupplierid = CInt(dgvFacturas.Rows(e.RowIndex).Cells(1).Value())
            ssuppliername = dgvFacturas.Rows(e.RowIndex).Cells(2).Value()
            ssupplierinvoicedescription = dgvFacturas.Rows(e.RowIndex).Cells(3).Value()

        Catch ex As Exception

            isupplierinvoiceid = 0
            isupplierid = 0
            ssuppliername = ""
            ssupplierinvoicedescription = ""

        End Try

        If dgvFacturas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim afp As New AgregarFacturaProveedor

            afp.susername = susername
            afp.bactive = bactive
            afp.bonline = bonline
            afp.suserfullname = suserfullname
            afp.suseremail = suseremail
            afp.susersession = susersession
            afp.susermachinename = susermachinename
            afp.suserip = suserip

            afp.isupplierinvoiceid = isupplierinvoiceid

            afp.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                afp.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            afp.ShowDialog(Me)
            Me.Visible = True

            If afp.DialogResult = Windows.Forms.DialogResult.OK Then

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


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        querystring = ""

        isupplierinvoiceid = 0
        isupplierid = 0
        ssuppliername = ""
        ssupplierinvoicedescription = ""

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If dgvFacturas.SelectedRows.Count = 1 Then

            If isEdit = False Then

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

            Dim afp As New AgregarFacturaProveedor

            afp.susername = susername
            afp.bactive = bactive
            afp.bonline = bonline
            afp.suserfullname = suserfullname
            afp.suseremail = suseremail
            afp.susersession = susersession
            afp.susermachinename = susermachinename
            afp.suserip = suserip

            afp.isupplierinvoiceid = isupplierinvoiceid

            afp.isEdit = True

            If Me.WindowState = FormWindowState.Maximized Then
                afp.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            afp.ShowDialog(Me)
            Me.Visible = True

            If afp.DialogResult = Windows.Forms.DialogResult.OK Then

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


    Private Sub btnCrear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCrear.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim afp As New AgregarFacturaProveedor
        afp.susername = susername
        afp.bactive = bactive
        afp.bonline = bonline
        afp.suserfullname = suserfullname
        afp.suseremail = suseremail
        afp.susersession = susersession
        afp.susermachinename = susermachinename
        afp.suserip = suserip

        afp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            afp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        afp.ShowDialog(Me)
        Me.Visible = True

        If afp.DialogResult = Windows.Forms.DialogResult.OK Then

            Me.isupplierinvoiceid = afp.isupplierinvoiceid
            Me.isupplierid = afp.isupplierid
            Me.ssuppliername = afp.ssuppliername
            Me.ssupplierinvoicedescription = afp.ssupplierinvoicedescription

            If afp.wasCreated = True Then
                Me.wasCreated = True
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux



            msSaveFileDialog.FileName = "Facturas " & fecha
            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Facturas Exportadas Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar las Facturas. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar las Facturas")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the project header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""189""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""183.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""52.5""/>")
            fs.WriteLine("   <Column ss:Width=""108.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""55.5""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""67.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""123.75""/>")
            fs.WriteLine("   <Column ss:Width=""126""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""8"" ss:StyleID=""1""><Data ss:Type=""String"">FACTURAS</Data></Cell>")
            fs.WriteLine("   </Row>")


            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next

            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""22.5"">")

            For Each col As DataGridViewColumn In dgvFacturas.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvFacturas.Rows

                If dgvFacturas.AllowUserToAddRows = True And row.Index = dgvFacturas.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvFacturas.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))

                        Else

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

            'Write the separation between results and totals
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            'Write the totals 
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


End Class