Public Class BuscaArchivosAbiertosRecientemente

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public sselectedsusersession As Integer = 1
    Public sselectedid As String = ""
    Public sselectedtypedescription As String = ""

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

                If permission = "Exportar" Then
                    btnExportar.Enabled = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j

    End Sub


    Private Sub BuscaArchivosAbiertosRecientemente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        verifySuspiciousData()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerrando la Ventana de Archivos Recientes', 'OK')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub BuscaArchivosAbiertosRecientemente_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaArchivosAbiertosRecientemente_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        executeSQLCommand(0, "SELECT @z:=0")

        setDataGridView(dgvArchivos, "" & _
        "SELECT susersession, sid AS 'ID', sdocumenttype, @z:=@z+1 AS 'Numero', " & _
        "sdocumenttypetranslation AS 'Tipo Archivo', sdocumentname AS 'Nombre', " & _
        "STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha de Ultimo Guardado' " & _
        "FROM ( " & _
        "SELECT * FROM " & _
        "( " & _
        "  SELECT * FROM recentlyopenedfiles WHERE susername = '" & susername & "' " & _
        "  AND sdocumentstatus = '0' " & _
        "  AND sid > 0 " & _
        "  ORDER BY iupdatedate DESC, supdatetime DESC " & _
        ") tmpA " & _
        "GROUP BY sid ASC, sdocumenttype DESC " & _
        "ORDER BY iupdatedate DESC, supdatetime DESC " & _
        "LIMIT 10 " & _
        ") tmpB ", True)

        dgvArchivos.Columns(0).Visible = False
        dgvArchivos.Columns(1).Visible = False
        dgvArchivos.Columns(2).Visible = False

        dgvArchivos.Columns(0).Width = 30
        dgvArchivos.Columns(1).Width = 30
        dgvArchivos.Columns(2).Width = 100
        dgvArchivos.Columns(3).Width = 25
        dgvArchivos.Columns(4).Width = 100
        dgvArchivos.Columns(5).Width = 200
        dgvArchivos.Columns(6).Width = 150


        dgvArchivos.Select()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Mostrando Archivos Recientes', 'OK')")

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


    Private Sub dgvArchivos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvArchivos.CellClick

        Try

            sselectedsusersession = CInt(dgvArchivos.Rows(e.RowIndex).Cells(0).Value)
            sselectedid = dgvArchivos.Rows(e.RowIndex).Cells(1).Value
            sselectedtypedescription = dgvArchivos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvArchivos_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvArchivos.CellContentClick

        Try

            sselectedsusersession = CInt(dgvArchivos.Rows(e.RowIndex).Cells(0).Value)
            sselectedid = dgvArchivos.Rows(e.RowIndex).Cells(1).Value
            sselectedtypedescription = dgvArchivos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvArchivos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvArchivos.SelectionChanged

        Try

            sselectedsusersession = CInt(dgvArchivos.CurrentRow.Cells(0).Value)
            sselectedid = CInt(dgvArchivos.CurrentRow.Cells(1).Value)
            sselectedtypedescription = dgvArchivos.CurrentRow.Cells(2).Value

        Catch ex As Exception

        End Try

    End Sub


    Private Sub dgvArchivos_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvArchivos.CellContentDoubleClick

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            sselectedsusersession = CInt(dgvArchivos.Rows(e.RowIndex).Cells(0).Value)
            sselectedid = dgvArchivos.Rows(e.RowIndex).Cells(1).Value
            sselectedtypedescription = dgvArchivos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try


        Select Case sselectedtypedescription


            Case "Asset"


                If getSQLQueryAsBoolean(0, "SELECT * FROM assets WHERE iassetid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarActivo

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.iassetid = sselectedid

                aa.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True


            Case "Bank"


                If getSQLQueryAsBoolean(0, "SELECT * FROM banks WHERE ibankid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ab As New AgregarBanco

                ab.susername = susername
                ab.bactive = bactive
                ab.bonline = bonline
                ab.suserfullname = suserfullname
                ab.suseremail = suseremail
                ab.susersession = susersession
                ab.susermachinename = susermachinename
                ab.suserip = suserip

                ab.ibankid = sselectedid

                ab.isEdit = True


                If Me.WindowState = FormWindowState.Maximized Then
                    ab.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ab.ShowDialog(Me)
                Me.Visible = True



            Case "Base"


                If getSQLQueryAsBoolean(0, "SELECT * FROM base WHERE ibaseid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ab As New AgregarBase

                ab.susername = susername
                ab.bactive = bactive
                ab.bonline = bonline
                ab.suserfullname = suserfullname

                ab.suseremail = suseremail
                ab.susersession = susersession
                ab.susermachinename = susermachinename
                ab.suserip = suserip

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If sselectedid = baseid Then

                    ab.isHistoric = False

                Else

                    ab.isHistoric = True

                End If

                ab.iprojectid = sselectedid
                ab.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ab.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ab.ShowDialog(Me)
                Me.Visible = True



            Case "LegacyCategory"


                If getSQLQueryAsBoolean(0, "SELECT * FROM cardlegacycategories WHERE scardlegacycategoryid = '" & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aci As New AgregarCategoriaInsumo

                aci.susername = susername
                aci.bactive = bactive
                aci.bonline = bonline
                aci.suserfullname = suserfullname
                aci.suseremail = suseremail
                aci.susersession = susersession
                aci.susermachinename = susermachinename
                aci.suserip = suserip

                aci.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aci.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aci.ShowDialog(Me)
                Me.Visible = True



            Case "CompanyAccount"


                If getSQLQueryAsBoolean(0, "SELECT * FROM companyaccounts WHERE iaccountid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim acc As New AgregarCuentaCompania

                acc.susername = susername
                acc.bactive = bactive
                acc.bonline = bonline
                acc.suserfullname = suserfullname
                acc.suseremail = suseremail
                acc.susersession = susersession
                acc.susermachinename = susermachinename
                acc.suserip = suserip

                acc.iaccountid = sselectedid

                acc.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    acc.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                acc.ShowDialog(Me)
                Me.Visible = True



            Case "SupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierinvoices WHERE isupplierinvoiceid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
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

                afp.isupplierinvoiceid = sselectedid

                afp.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    afp.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                afp.ShowDialog(Me)
                Me.Visible = True



            Case "GasSupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierinvoices WHERE isupplierinvoiceid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim afp As New AgregarFacturaCombustible

                afp.susername = susername
                afp.bactive = bactive
                afp.bonline = bonline
                afp.suserfullname = suserfullname
                afp.suseremail = suseremail
                afp.susersession = susersession
                afp.susermachinename = susermachinename
                afp.suserip = suserip

                afp.isupplierinvoiceid = sselectedid

                afp.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    afp.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                afp.ShowDialog(Me)
                Me.Visible = True



            Case "Income"


                If getSQLQueryAsBoolean(0, "SELECT * FROM incomes WHERE iincomeid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ai As New AgregarIngreso

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iincomeid = sselectedid

                ai.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True



            Case "Model"


                If getSQLQueryAsBoolean(0, "SELECT * FROM models WHERE imodelid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim m As New AgregarModelo

                m.susername = susername
                m.bactive = bactive
                m.bonline = bonline
                m.suserfullname = suserfullname
                m.suseremail = suseremail
                m.susersession = susersession
                m.susermachinename = susermachinename
                m.suserip = suserip

                m.imodelid = sselectedid

                m.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    m.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                m.ShowDialog(Me)
                Me.Visible = True



            Case "Payment"


                If getSQLQueryAsBoolean(0, "SELECT * FROM payments WHERE ipaymentid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ap As New AgregarPago

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.ipaymentid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            Case "People"


                If getSQLQueryAsBoolean(0, "SELECT * FROM people WHERE ipeopleid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
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

                ap.ipeopleid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True



            Case "Supplier"


                If getSQLQueryAsBoolean(0, "SELECT * FROM suppliers WHERE isupplierid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ap As New AgregarProveedor

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isupplierid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            Case "Project"


                If getSQLQueryAsBoolean(0, "SELECT * FROM projects WHERE iprojectid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim proy As New AgregarProyecto

                proy.susername = susername
                proy.bactive = bactive
                proy.bonline = bonline
                proy.suserfullname = suserfullname
                proy.suseremail = suseremail
                proy.susersession = susersession
                proy.susermachinename = susermachinename
                proy.suserip = suserip

                Dim fechaTerminoReal As Integer = 0
                Try
                    fechaTerminoReal = getSQLQueryAsInteger(0, "SELECT iprojectrealclosingdate FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " WHERE iprojectid = " & sselectedid)
                Catch ex As Exception

                End Try

                If fechaTerminoReal = 0 Then
                    proy.isHistoric = False
                Else
                    proy.isHistoric = True
                End If

                proy.isEdit = True

                proy.iprojectid = sselectedid

                If Me.WindowState = FormWindowState.Maximized Then
                    proy.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                proy.ShowDialog(Me)
                Me.Visible = True



            Case "GasVoucher"


                If getSQLQueryAsBoolean(0, "SELECT * FROM gasvouchers WHERE igasvoucherid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim av As New AgregarVale

                av.susername = susername
                av.bactive = bactive
                av.bonline = bonline
                av.suserfullname = suserfullname
                av.suseremail = suseremail
                av.susersession = susersession
                av.susermachinename = susermachinename
                av.suserip = suserip

                av.igasvoucherid = sselectedid

                av.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    av.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                av.ShowDialog(Me)
                Me.Visible = True



            Case "Payroll"

                If getSQLQueryAsBoolean(0, "SELECT * FROM payrolls WHERE ipayrollid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarNomina

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.ipayrollid = sselectedid

                aa.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True



            Case "Estimation"

                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierestimations WHERE iestimationid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarCotizacionInsumo

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.iestimationid = sselectedid

                aa.isOpeningFromRecents = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True



            Case Else

                'Nothing to do, shouldn't happen


        End Select


        executeSQLCommand(0, "UPDATE recentlyopenedfiles SET susersession = " & susersession & " WHERE susername = '" & susername & "' AND sdocumenttype = '" & sselectedtypedescription & "' AND sid = " & sselectedid & " AND sdocumentstatus = '1'")

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvArchivos_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvArchivos.CellDoubleClick

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            sselectedsusersession = CInt(dgvArchivos.Rows(e.RowIndex).Cells(0).Value)
            sselectedid = dgvArchivos.Rows(e.RowIndex).Cells(1).Value
            sselectedtypedescription = dgvArchivos.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

        End Try


        Select Case sselectedtypedescription


            Case "Asset"


                If getSQLQueryAsBoolean(0, "SELECT * FROM assets WHERE iassetid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarActivo

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.iassetid = sselectedid

                aa.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True


            Case "Bank"


                If getSQLQueryAsBoolean(0, "SELECT * FROM banks WHERE ibankid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ab As New AgregarBanco

                ab.susername = susername
                ab.bactive = bactive
                ab.bonline = bonline
                ab.suserfullname = suserfullname
                ab.suseremail = suseremail
                ab.susersession = susersession
                ab.susermachinename = susermachinename
                ab.suserip = suserip

                ab.ibankid = sselectedid

                ab.isEdit = True


                If Me.WindowState = FormWindowState.Maximized Then
                    ab.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ab.ShowDialog(Me)
                Me.Visible = True



            Case "Base"


                If getSQLQueryAsBoolean(0, "SELECT * FROM base WHERE ibaseid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ab As New AgregarBase

                ab.susername = susername
                ab.bactive = bactive
                ab.bonline = bonline
                ab.suserfullname = suserfullname

                ab.suseremail = suseremail
                ab.susersession = susersession
                ab.susermachinename = susermachinename
                ab.suserip = suserip

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If sselectedid = baseid Then

                    ab.isHistoric = False

                Else

                    ab.isHistoric = True

                End If

                ab.iprojectid = sselectedid
                ab.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ab.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ab.ShowDialog(Me)
                Me.Visible = True



            Case "LegacyCategory"


                If getSQLQueryAsBoolean(0, "SELECT * FROM cardlegacycategories WHERE scardlegacycategoryid = '" & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aci As New AgregarCategoriaInsumo

                aci.susername = susername
                aci.bactive = bactive
                aci.bonline = bonline
                aci.suserfullname = suserfullname
                aci.suseremail = suseremail
                aci.susersession = susersession
                aci.susermachinename = susermachinename
                aci.suserip = suserip

                aci.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aci.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aci.ShowDialog(Me)
                Me.Visible = True



            Case "CompanyAccount"


                If getSQLQueryAsBoolean(0, "SELECT * FROM companyaccounts WHERE iaccountid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim acc As New AgregarCuentaCompania

                acc.susername = susername
                acc.bactive = bactive
                acc.bonline = bonline
                acc.suserfullname = suserfullname
                acc.suseremail = suseremail
                acc.susersession = susersession
                acc.susermachinename = susermachinename
                acc.suserip = suserip

                acc.iaccountid = sselectedid

                acc.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    acc.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                acc.ShowDialog(Me)
                Me.Visible = True



            Case "SupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierinvoices WHERE isupplierinvoiceid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
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

                afp.isupplierinvoiceid = sselectedid

                afp.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    afp.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                afp.ShowDialog(Me)
                Me.Visible = True



            Case "Income"


                If getSQLQueryAsBoolean(0, "SELECT * FROM incomes WHERE iincomeid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ai As New AgregarIngreso

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iincomeid = sselectedid

                ai.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True



            Case "Model"


                If getSQLQueryAsBoolean(0, "SELECT * FROM models WHERE imodelid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim m As New AgregarModelo

                m.susername = susername
                m.bactive = bactive
                m.bonline = bonline
                m.suserfullname = suserfullname
                m.suseremail = suseremail
                m.susersession = susersession
                m.susermachinename = susermachinename
                m.suserip = suserip

                m.imodelid = sselectedid

                m.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    m.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                m.ShowDialog(Me)
                Me.Visible = True



            Case "Payment"


                If getSQLQueryAsBoolean(0, "SELECT * FROM payments WHERE ipaymentid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ap As New AgregarPago

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.ipaymentid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            Case "People"


                If getSQLQueryAsBoolean(0, "SELECT * FROM people WHERE ipeopleid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
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

                ap.ipeopleid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True



            Case "Supplier"


                If getSQLQueryAsBoolean(0, "SELECT * FROM suppliers WHERE isupplierid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ap As New AgregarProveedor

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isupplierid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            Case "Project"


                If getSQLQueryAsBoolean(0, "SELECT * FROM projects WHERE iprojectid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim proy As New AgregarProyecto

                proy.susername = susername
                proy.bactive = bactive
                proy.bonline = bonline
                proy.suserfullname = suserfullname
                proy.suseremail = suseremail
                proy.susersession = susersession
                proy.susermachinename = susermachinename
                proy.suserip = suserip

                Dim fechaTerminoReal As Integer = 0
                Try
                    fechaTerminoReal = getSQLQueryAsInteger(0, "SELECT iprojectrealclosingdate FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " WHERE iprojectid = " & sselectedid)
                Catch ex As Exception

                End Try

                If fechaTerminoReal = 0 Then
                    proy.isHistoric = False
                Else
                    proy.isHistoric = True
                End If

                proy.isEdit = True

                proy.iprojectid = sselectedid

                If Me.WindowState = FormWindowState.Maximized Then
                    proy.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                proy.ShowDialog(Me)
                Me.Visible = True



            Case "GasVoucher"


                If getSQLQueryAsBoolean(0, "SELECT * FROM gasvouchers WHERE igasvoucherid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim av As New AgregarVale

                av.susername = susername
                av.bactive = bactive
                av.bonline = bonline
                av.suserfullname = suserfullname
                av.suseremail = suseremail
                av.susersession = susersession
                av.susermachinename = susermachinename
                av.suserip = suserip

                av.igasvoucherid = sselectedid

                av.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    av.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                av.ShowDialog(Me)
                Me.Visible = True



            Case "Payroll"

                If getSQLQueryAsBoolean(0, "SELECT * FROM payrolls WHERE ipayrollid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarNomina

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.ipayrollid = sselectedid

                aa.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True



            Case "Estimation"

                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierestimations WHERE iestimationid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarCotizacionInsumo

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.iestimationid = sselectedid

                aa.isOpeningFromRecents = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True



            Case Else

                'Nothing to do, shouldn't happen


        End Select


        executeSQLCommand(0, "UPDATE recentlyopenedfiles SET susersession = " & susersession & " WHERE susername = '" & susername & "' AND sdocumenttype = '" & sselectedtypedescription & "' AND sid = " & sselectedid & " AND sdocumentstatus = '1'")

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        'wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click

        Select Case sselectedtypedescription


            Case "Asset"


                If getSQLQueryAsBoolean(0, "SELECT * FROM assets WHERE iassetid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarActivo

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.iassetid = sselectedid

                aa.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True


            Case "Bank"


                If getSQLQueryAsBoolean(0, "SELECT * FROM banks WHERE ibankid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ab As New AgregarBanco

                ab.susername = susername
                ab.bactive = bactive
                ab.bonline = bonline
                ab.suserfullname = suserfullname
                ab.suseremail = suseremail
                ab.susersession = susersession
                ab.susermachinename = susermachinename
                ab.suserip = suserip

                ab.ibankid = sselectedid

                ab.isEdit = True


                If Me.WindowState = FormWindowState.Maximized Then
                    ab.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ab.ShowDialog(Me)
                Me.Visible = True



            Case "Base"


                If getSQLQueryAsBoolean(0, "SELECT * FROM base WHERE ibaseid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ab As New AgregarBase

                ab.susername = susername
                ab.bactive = bactive
                ab.bonline = bonline
                ab.suserfullname = suserfullname

                ab.suseremail = suseremail
                ab.susersession = susersession
                ab.susermachinename = susermachinename
                ab.suserip = suserip

                Dim baseid As Integer = 0
                baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                If baseid = 0 Then
                    baseid = 99999
                End If

                If sselectedid = baseid Then

                    ab.isHistoric = False

                Else

                    ab.isHistoric = True

                End If

                ab.iprojectid = sselectedid
                ab.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ab.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ab.ShowDialog(Me)
                Me.Visible = True



            Case "LegacyCategory"


                If getSQLQueryAsBoolean(0, "SELECT * FROM cardlegacycategories WHERE scardlegacycategoryid = '" & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aci As New AgregarCategoriaInsumo

                aci.susername = susername
                aci.bactive = bactive
                aci.bonline = bonline
                aci.suserfullname = suserfullname
                aci.suseremail = suseremail
                aci.susersession = susersession
                aci.susermachinename = susermachinename
                aci.suserip = suserip

                aci.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aci.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aci.ShowDialog(Me)
                Me.Visible = True



            Case "CompanyAccount"


                If getSQLQueryAsBoolean(0, "SELECT * FROM companyaccounts WHERE iaccountid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim acc As New AgregarCuentaCompania

                acc.susername = susername
                acc.bactive = bactive
                acc.bonline = bonline
                acc.suserfullname = suserfullname
                acc.suseremail = suseremail
                acc.susersession = susersession
                acc.susermachinename = susermachinename
                acc.suserip = suserip

                acc.iaccountid = sselectedid

                acc.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    acc.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                acc.ShowDialog(Me)
                Me.Visible = True



            Case "SupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierinvoices WHERE isupplierinvoiceid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
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

                afp.isupplierinvoiceid = sselectedid

                afp.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    afp.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                afp.ShowDialog(Me)
                Me.Visible = True



            Case "Income"


                If getSQLQueryAsBoolean(0, "SELECT * FROM incomes WHERE iincomeid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ai As New AgregarIngreso

                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iincomeid = sselectedid

                ai.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True



            Case "Model"


                If getSQLQueryAsBoolean(0, "SELECT * FROM models WHERE imodelid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim m As New AgregarModelo

                m.susername = susername
                m.bactive = bactive
                m.bonline = bonline
                m.suserfullname = suserfullname
                m.suseremail = suseremail
                m.susersession = susersession
                m.susermachinename = susermachinename
                m.suserip = suserip

                m.imodelid = sselectedid

                m.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    m.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                m.ShowDialog(Me)
                Me.Visible = True



            Case "Payment"


                If getSQLQueryAsBoolean(0, "SELECT * FROM payments WHERE ipaymentid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ap As New AgregarPago

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.ipaymentid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            Case "People"


                If getSQLQueryAsBoolean(0, "SELECT * FROM people WHERE ipeopleid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
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

                ap.ipeopleid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True



            Case "Supplier"


                If getSQLQueryAsBoolean(0, "SELECT * FROM suppliers WHERE isupplierid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim ap As New AgregarProveedor

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isupplierid = sselectedid

                ap.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            Case "Project"


                If getSQLQueryAsBoolean(0, "SELECT * FROM projects WHERE iprojectid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim proy As New AgregarProyecto

                proy.susername = susername
                proy.bactive = bactive
                proy.bonline = bonline
                proy.suserfullname = suserfullname
                proy.suseremail = suseremail
                proy.susersession = susersession
                proy.susermachinename = susermachinename
                proy.suserip = suserip

                Dim fechaTerminoReal As Integer = 0
                Try
                    fechaTerminoReal = getSQLQueryAsInteger(0, "SELECT iprojectrealclosingdate FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " WHERE iprojectid = " & sselectedid)
                Catch ex As Exception

                End Try

                If fechaTerminoReal = 0 Then
                    proy.isHistoric = False
                Else
                    proy.isHistoric = True
                End If

                proy.isEdit = True

                proy.iprojectid = sselectedid

                If Me.WindowState = FormWindowState.Maximized Then
                    proy.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                proy.ShowDialog(Me)
                Me.Visible = True



            Case "GasVoucher"


                If getSQLQueryAsBoolean(0, "SELECT * FROM gasvouchers WHERE igasvoucherid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim av As New AgregarVale

                av.susername = susername
                av.bactive = bactive
                av.bonline = bonline
                av.suserfullname = suserfullname
                av.suseremail = suseremail
                av.susersession = susersession
                av.susermachinename = susermachinename
                av.suserip = suserip

                av.igasvoucherid = sselectedid

                av.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    av.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                av.ShowDialog(Me)
                Me.Visible = True



            Case "Payroll"

                If getSQLQueryAsBoolean(0, "SELECT * FROM payrolls WHERE ipayrollid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarNomina

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.ipayrollid = sselectedid

                aa.isEdit = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True



            Case "Estimation"

                If getSQLQueryAsBoolean(0, "SELECT * FROM supplierestimations WHERE iestimationid = " & sselectedid) = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe, fue borrado del sistema. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim aa As New AgregarCotizacionInsumo

                aa.susername = susername
                aa.bactive = bactive
                aa.bonline = bonline
                aa.suserfullname = suserfullname
                aa.suseremail = suseremail
                aa.susersession = susersession
                aa.susermachinename = susermachinename
                aa.suserip = suserip

                aa.iestimationid = sselectedid

                aa.isOpeningFromRecents = True

                If Me.WindowState = FormWindowState.Maximized Then
                    aa.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                aa.ShowDialog(Me)
                Me.Visible = True



            Case Else

                'Nothing to do, shouldn't happen


        End Select


        executeSQLCommand(0, "UPDATE recentlyopenedfiles SET susersession = " & susersession & " WHERE susername = '" & susername & "' AND sdocumenttype = '" & sselectedtypedescription & "' AND sid = " & sselectedid & " AND sdocumentstatus = '1'")

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

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



            msSaveFileDialog.FileName = "Archivos Recientes " & fecha
            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Archivos Recientes Exportados Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar los Archviso Recientes. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar los Archivos Recientes")
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
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""50.25""/>")
            fs.WriteLine("   <Column ss:Width=""115.5""/>")
            fs.WriteLine("   <Column ss:Width=""142.5""/>")
            fs.WriteLine("   <Column ss:Width=""144.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""119.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""126.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""147.75""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""72""/>")
            fs.WriteLine("   <Column ss:Width=""126""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""3"" ss:StyleID=""1""><Data ss:Type=""String"">ARCHIVOS ABIERTOS RECIENTEMENTE</Data></Cell>")
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

            For Each col As DataGridViewColumn In dgvArchivos.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvArchivos.Rows

                If dgvArchivos.AllowUserToAddRows = True And row.Index = dgvArchivos.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvArchivos.Columns

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