Public Class BuscaArchivosCerradosIncorrectamente

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


    Private Sub BuscaArchivosCerradosIncorrectamente_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        verifySuspiciousData()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerrando la Ventana de Archivos Recuperados', 'OK')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub BuscaArchivosCerradosIncorrectamente_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub BuscaArchivosCerradosIncorrectamente_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        'setControlsByPermissions(Me.Name, susername)

        setDataGridView(dgvArchivos, "SELECT susersession, sid AS 'ID', sdocumenttype, sdocumenttypetranslation AS 'Tipo Archivo', sdocumentname AS 'Nombre', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha de Ultimo Guardado' FROM ( SELECT * FROM recentlyopenedfiles WHERE susername = '" & susername & "' GROUP BY sdocumenttype DESC, sid ASC ) h1 WHERE sdocumentstatus = '1' AND susername IN (SELECT susername FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC)", True)

        dgvArchivos.Columns(0).Visible = False
        dgvArchivos.Columns(1).Visible = False
        dgvArchivos.Columns(2).Visible = False

        dgvArchivos.Select()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Mostrando Archivos Recuperados', 'OK')")

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


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    aa.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aa.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aa.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Bank"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    ab.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ab.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ab.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Base"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim queries(9) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "IndirectCosts"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Cards"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardInputs"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardCompoundInputs"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Prices"
                queries(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Timber"

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & sselectedid & "CardsAux'") > 0 Then
                    queries(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardsAux"
                End If

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & sselectedid & "CardAux%'") > 0 Then

                    Dim tablename As String = ""
                    Dim otherid As String = ""

                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & sselectedid & "CardAux%'")
                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                    queries(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardAux" & otherid

                End If

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ab.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ab.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ab.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "LegacyCategory"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    aci.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aci.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aci.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "CompanyAccount"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    acc.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        acc.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    acc.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "SupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(6) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Inputs"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Payments"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Assets"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Discounts"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    afp.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        afp.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    afp.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "GasSupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(6) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "GasVouchers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "GasVouchers"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Payments"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Assets"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Discounts"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    afp.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        afp.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    afp.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Income"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Assets"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ai.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ai.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ai.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Model"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(16) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "IndirectCosts"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Cards"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardInputs"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardCompoundInputs"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Prices"
                queries(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Timber"
                queries(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & ""
                queries(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "IndirectCosts"
                queries(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Cards"
                queries(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardInputs"
                queries(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardCompoundInputs"
                queries(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Prices"
                queries(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Timber"

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & sselectedid & "CardsAux'") > 0 Then
                    queries(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardsAux"
                End If

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & sselectedid & "CardAux%'") > 0 Then

                    Dim tablename As String = ""
                    Dim otherid As String = ""

                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & sselectedid & "CardAux%'")
                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                    queries(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardAux" & otherid

                End If

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    m.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        m.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    m.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Payment"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    ap.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ap.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ap.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "People"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(2) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "PhoneNumbers"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ap.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ap.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ap.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Supplier"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "PhoneNumbers"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Contacts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Contacts"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ap.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ap.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ap.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Project"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(18) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "IndirectCosts"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Cards"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardInputs"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardCompoundInputs"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Prices"
                queries(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Explosion RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Explosion"
                queries(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Timber"
                queries(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "AdminCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "AdminCosts"
                queries(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & ""
                queries(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "IndirectCosts"
                queries(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Cards"
                queries(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardInputs"
                queries(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardCompoundInputs"
                queries(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Prices"
                queries(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Timber"

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Project" & sselectedid & "CardsAux'") > 0 Then
                    queries(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardsAux"
                End If

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Project" & sselectedid & "CardAux%'") > 0 Then

                    Dim tablename As String = ""
                    Dim otherid As String = ""

                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Project" & sselectedid & "CardAux%'")
                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                    queries(17) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardAux" & otherid

                End If

                If executeTransactedSQLCommand(0, queries) = True Then

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
                        proy.isRecover = True
                    Else
                        proy.isHistoric = True
                        proy.isRecover = False
                    End If

                    proy.isEdit = True

                    proy.iprojectid = sselectedid

                    If Me.WindowState = FormWindowState.Maximized Then
                        proy.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    proy.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "GasVoucher"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(2) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    av.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        av.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    av.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Payroll"

                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Payments"
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "People RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "People"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    aa.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aa.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aa.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Estimation"

                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.`tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "`"

                If executeTransactedSQLCommand(0, queries) = True Then


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

                    'aa.isEdit = True
                    aa.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aa.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aa.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case Else

                'Nothing to do, shouldn't happen


        End Select


        executeSQLCommand(0, "UPDATE recentlyopenedfiles SET susersession = " & susersession & " WHERE susername = '" & susername & "' AND sdocumenttype = '" & sselectedtypedescription & "' AND sid = " & sselectedid & " AND sdocumentstatus = '1'")


        setDataGridView(dgvArchivos, "SELECT susersession, sid AS 'ID', sdocumenttype, sdocumenttypetranslation AS 'Tipo Archivo', sdocumentname AS 'Nombre', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha de Ultimo Guardado' FROM ( SELECT * FROM recentlyopenedfiles WHERE susername = '" & susername & "' GROUP BY sdocumenttype DESC, sid ASC ) h1 WHERE sdocumentstatus = '1' AND susername IN (SELECT susername FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC)", True)

        dgvArchivos.Columns(0).Visible = False
        dgvArchivos.Columns(1).Visible = False
        dgvArchivos.Columns(2).Visible = False


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


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    aa.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aa.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aa.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Bank"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    ab.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ab.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ab.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Base"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim queries(9) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "IndirectCosts"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Cards"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardInputs"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardCompoundInputs"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Prices"
                queries(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Timber"

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & sselectedid & "CardsAux'") > 0 Then
                    queries(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardsAux"
                End If

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & sselectedid & "CardAux%'") > 0 Then

                    Dim tablename As String = ""
                    Dim otherid As String = ""

                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Base" & sselectedid & "CardAux%'")
                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                    queries(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardAux" & otherid

                End If

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ab.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ab.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ab.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "LegacyCategory"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    aci.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aci.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aci.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "CompanyAccount"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    acc.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        acc.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    acc.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "SupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(6) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Inputs"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Payments"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Assets"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Discounts"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    afp.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        afp.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    afp.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "GasSupplierInvoice"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(6) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "GasVouchers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "GasVouchers"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Payments"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Assets"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Discounts"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    afp.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        afp.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    afp.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Income"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Assets"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ai.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ai.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ai.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Model"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(16) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "IndirectCosts"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Cards"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardInputs"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardCompoundInputs"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Prices"
                queries(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Timber"
                queries(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & ""
                queries(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "IndirectCosts"
                queries(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Cards"
                queries(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardInputs"
                queries(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardCompoundInputs"
                queries(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Prices"
                queries(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Timber"

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & sselectedid & "CardsAux'") > 0 Then
                    queries(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardsAux"
                End If

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & sselectedid & "CardAux%'") > 0 Then

                    Dim tablename As String = ""
                    Dim otherid As String = ""

                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & sselectedid & "CardAux%'")
                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                    queries(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardAux" & otherid

                End If

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    m.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        m.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    m.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Payment"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                If executeSQLCommand(0, "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "") = True Then

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

                    ap.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ap.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ap.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "People"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(2) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "PhoneNumbers"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ap.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ap.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ap.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Supplier"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "PhoneNumbers RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "PhoneNumbers"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Contacts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Contacts"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    ap.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        ap.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    ap.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Project"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(18) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "IndirectCosts"
                queries(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Cards"
                queries(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardInputs"
                queries(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "CardCompoundInputs"
                queries(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Prices"
                queries(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Explosion RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Explosion"
                queries(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Timber"
                queries(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "AdminCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "AdminCosts"
                queries(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & ""
                queries(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "IndirectCosts"
                queries(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Cards"
                queries(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardInputs"
                queries(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "CardCompoundInputs"
                queries(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Prices"
                queries(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & "Base" & sselectedid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & sselectedid & "Timber"

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Project" & sselectedid & "CardsAux'") > 0 Then
                    queries(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardsAux"
                End If

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Project" & sselectedid & "CardAux%'") > 0 Then

                    Dim tablename As String = ""
                    Dim otherid As String = ""

                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Project" & sselectedid & "CardAux%'")
                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                    queries(17) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "CardAux" & otherid

                End If

                If executeTransactedSQLCommand(0, queries) = True Then

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
                        proy.isRecover = True
                    Else
                        proy.isHistoric = True
                        proy.isRecover = False
                    End If

                    proy.isEdit = True

                    proy.iprojectid = sselectedid

                    If Me.WindowState = FormWindowState.Maximized Then
                        proy.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    proy.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "GasVoucher"


                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If


                Dim queries(2) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Projects"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    av.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        av.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    av.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Payroll"

                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & ""
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "Payments"
                queries(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & "People RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "People"

                If executeTransactedSQLCommand(0, queries) = True Then

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

                    aa.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aa.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aa.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case "Estimation"

                If getSQLQueryAsBoolean(0, "SELECT * FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%S" & sselectedsusersession & sselectedtypedescription & sselectedid & "'") = False Then
                    MsgBox("El archivo que estas intentando abrir ya no existe (fue borrado del sistema) o no es recuperable. Lo sentimos. Intenta con otro archivo.")
                    Exit Sub
                End If

                Dim queries(3) As String

                queries(0) = "ALTER TABLE oversight.`tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & sselectedsusersession & sselectedtypedescription & sselectedid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & sselectedtypedescription & sselectedid & "`"

                If executeTransactedSQLCommand(0, queries) = True Then


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

                    'aa.isEdit = True
                    aa.isRecover = True

                    If Me.WindowState = FormWindowState.Maximized Then
                        aa.WindowState = FormWindowState.Maximized
                    End If

                    Me.Visible = False
                    aa.ShowDialog(Me)
                    Me.Visible = True

                Else

                    MsgBox("No se pudo recuperar este archivo", MsgBoxStyle.Exclamation, "Error")

                End If



            Case Else

                'Nothing to do, shouldn't happen


        End Select


        executeSQLCommand(0, "UPDATE recentlyopenedfiles SET susersession = " & susersession & " WHERE susername = '" & susername & "' AND sdocumenttype = '" & sselectedtypedescription & "' AND sid = " & sselectedid & " AND sdocumentstatus = '1'")


        setDataGridView(dgvArchivos, "SELECT susersession, sid AS 'ID', sdocumenttype, sdocumenttypetranslation AS 'Tipo Archivo', sdocumentname AS 'Nombre', STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS 'Fecha de Ultimo Guardado' FROM ( SELECT * FROM recentlyopenedfiles WHERE susername = '" & susername & "' GROUP BY sdocumenttype DESC, sid ASC ) h1 WHERE sdocumentstatus = '1' AND susername IN (SELECT susername FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC)", True)

        dgvArchivos.Columns(0).Visible = False
        dgvArchivos.Columns(1).Visible = False
        dgvArchivos.Columns(2).Visible = False


        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Sub btnCerrarBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrarBorrar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim dsArchivos As DataSet
        dsArchivos = getSQLQueryAsDataset(0, "SELECT susersession, sid, sdocumenttype, sdocumenttypetranslation, sdocumentname, STR_TO_DATE(CONCAT(iupdatedate, ' ', supdatetime), '%Y%c%d %T') AS lastsave FROM ( SELECT * FROM recentlyopenedfiles WHERE susername = '" & susername & "' GROUP BY sdocumenttype DESC, sid ASC ) h1 WHERE sdocumentstatus = '1' AND susername IN (SELECT susername FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC)")

        If dsArchivos.Tables(0).Rows.Count > 0 Then

            If MsgBox("¿Estás seguro de que no deseas abrir y guardar los documentos que te muestro? Se perderán los cambios hechos a estos archivos!", MsgBoxStyle.YesNo, "Confirmación para Borrar Documentos No Recuperados") = MsgBoxResult.Yes Then

                Dim slastsusersession As String
                Dim sid As String
                Dim sdocumenttype As String
                Dim sdocumenttypetranslation As String
                Dim sdocumentname As String

                Dim fecha As Integer = 0
                Dim hora As String = ""


                For i = 0 To dsArchivos.Tables(0).Rows.Count - 1

                    Try

                        slastsusersession = dsArchivos.Tables(0).Rows(i).Item("susersession")
                        sid = dsArchivos.Tables(0).Rows(i).Item("sid")
                        sdocumenttype = dsArchivos.Tables(0).Rows(i).Item("sdocumenttype")
                        sdocumenttypetranslation = dsArchivos.Tables(0).Rows(i).Item("sdocumenttypetranslation")
                        sdocumentname = dsArchivos.Tables(0).Rows(i).Item("sdocumentname")

                        fecha = getMySQLDate()
                        hora = getAppTime()


                        Select Case sdocumenttype


                            Case "Asset"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Bank"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Base"


                                Dim queriesDrop(9) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "IndirectCosts"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Cards"
                                queriesDrop(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardInputs"
                                queriesDrop(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardCompoundInputs"
                                queriesDrop(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Prices"
                                queriesDrop(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Timber"

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardsAux'") > 0 Then
                                    queriesDrop(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardsAux"
                                End If

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardAux%'") > 0 Then

                                    Dim tablename As String = ""
                                    Dim otherid As String = ""

                                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardAux%'")
                                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                                    queriesDrop(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardAux" & otherid

                                End If

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "LegacyCategory"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la" & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "CompanyAccount"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la" & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "SupplierInvoice"


                                Dim queriesDrop(6) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Inputs"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Projects"
                                queriesDrop(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Payments"
                                queriesDrop(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Assets"
                                queriesDrop(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Discounts"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la" & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "GasSupplierInvoice"


                                Dim queriesDrop(6) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "GasVouchers"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Projects"
                                queriesDrop(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Payments"
                                queriesDrop(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Assets"
                                queriesDrop(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Discounts"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la" & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Income"


                                Dim queriesDrop(3) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Assets"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Projects"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Model"


                                Dim queriesDrop(16) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "IndirectCosts"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Cards"
                                queriesDrop(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardInputs"
                                queriesDrop(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardCompoundInputs"
                                queriesDrop(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Prices"
                                queriesDrop(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Timber"
                                queriesDrop(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & ""
                                queriesDrop(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "IndirectCosts"
                                queriesDrop(9) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "Cards"
                                queriesDrop(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "CardInputs"
                                queriesDrop(11) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "CardCompoundInputs"
                                queriesDrop(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "Prices"
                                queriesDrop(13) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "Timber"

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardsAux'") > 0 Then
                                    queriesDrop(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardsAux"
                                End If

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardAux%'") > 0 Then

                                    Dim tablename As String = ""
                                    Dim otherid As String = ""

                                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardAux%'")
                                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                                    queriesDrop(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardAux" & otherid

                                End If

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Payment"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "People"


                                Dim queriesDrop(2) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "PhoneNumbers"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la" & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Supplier"


                                Dim queriesDrop(3) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "PhoneNumbers"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Contacts"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Project"


                                Dim queriesDrop(18) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "IndirectCosts"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Cards"
                                queriesDrop(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardInputs"
                                queriesDrop(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardCompoundInputs"
                                queriesDrop(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Prices"
                                queriesDrop(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Timber"
                                queriesDrop(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Explosion"
                                queriesDrop(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "AdminCosts"
                                queriesDrop(9) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & ""
                                queriesDrop(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "IndirectCosts"
                                queriesDrop(11) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "Cards"
                                queriesDrop(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "CardInputs"
                                queriesDrop(13) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "CardCompoundInputs"
                                queriesDrop(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "Prices"
                                queriesDrop(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & "Base" & sid & "Timber"

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardsAux'") > 0 Then
                                    queriesDrop(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardsAux"
                                End If

                                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardAux%'") > 0 Then

                                    Dim tablename As String = ""
                                    Dim otherid As String = ""

                                    tablename = getSQLQueryAsString(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%" & sdocumenttype & sid & "CardAux%'")
                                    otherid = tablename.Substring(tablename.IndexOf("CardAux"))

                                    queriesDrop(17) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "CardAux" & otherid

                                End If

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "GasVoucher"

                                Dim queriesDrop(2) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Projects"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "User"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal del " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Payroll"


                                Dim queriesDrop(3) As String

                                queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & ""
                                queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "Payments"
                                queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "People"

                                executeTransactedSQLCommand(0, queriesDrop)

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case "Estimation"


                                executeSQLCommand(0, "DROP TABLE IF EXISTS oversight.`tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & slastsusersession & sdocumenttype & sid & "`")

                                Dim queries(2) As String

                                queries(0) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Borré la tabla temporal de la " & sdocumenttypetranslation & " " & sid & " : " & sdocumentname & "', 'OK')"
                                queries(1) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & slastsusersession & "', '" & sdocumenttype & "', '" & sdocumenttypetranslation & "', '" & sid & "', 'No recuperado', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

                                executeTransactedSQLCommand(0, queries)



                            Case Else


                                'Nothing to do, shouldn't happen



                        End Select

                    Catch ex As Exception

                    End Try

                Next i


            End If

        End If


        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub



End Class