Public Class AgregarDescuentoAFacturaProveedor

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

    Private isFormReadyForAction As Boolean = False

    Public isupplierinvoiceid As Integer = 0

    Private iselectedinvoicediscountid As Integer = 0
    Private sselectedinvoicediscountdescription As String = ""
    Private dselectedinvoicediscountpercentage As Double = 0.0

    Private WithEvents txtNumeroDgvDescuentos As TextBox
    Private WithEvents txtNombreDescuentoDgvDescuentos As TextBox

    Private txtNombreDescuentoDgvDescuentos_OldText As String = ""
    Private txtNumeroDgvDescuentos_OldText As String = ""

    Private openPermission As Boolean = False

    Private newDiscountPermission As Boolean = False
    Private modifyDiscountPermission As Boolean = False
    Private deleteDiscountPermission As Boolean = False

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

                If permission = "Modificar Descuentos" Then
                    btnCerrarDescuentos.Enabled = True
                End If

                If permission = "Nuevo Descuento" Then
                    newDiscountPermission = True
                    btnNuevoDescuento.Enabled = True
                End If

                If permission = "Eliminar Descuento" Then
                    deleteDiscountPermission = True
                    btnEliminarDescuento.Enabled = True
                End If

                If permission = "Modificar Descuento" Then
                    dgvDescuentos.Enabled = True
                    modifyDiscountPermission = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Descuentos a Facturas de Proveedores', 'OK')")

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


    Private Sub AgregarDescuentoAFactura_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaDatosDescuento(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró los Descuentos de la Factura " & isupplierinvoiceid & "', 'OK')")
        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Descuento de Factura de Proveedor', '" & isupplierinvoiceid & "', '', 0, " & fecha & ", '" & hora & "', '" & susername & "')")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarDescuentoAFactura_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarDescuentoAFactura_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        'Dim timesInvoiceDiscountIsOpen As Integer = 0

        'timesInvoiceDiscountIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "Discounts'")

        'If timesInvoiceDiscountIsOpen > 0 And isEdit = True Then

        '    If MsgBox("Otro usuario tiene abierta la ventana de Descuentos la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar abriendo?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

        '        Cursor.Current = System.Windows.Forms.Cursors.Default

        '        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        '        Me.Close()

        '        Exit Sub

        '    End If

        'End If


        Dim querySupplierInvoiceDiscounts As String
        querySupplierInvoiceDiscounts = "" & _
        "SELECT sid.isupplierinvoiceid, sid.isupplierinvoicediscountid, sid.ssupplierinvoicediscountdescription AS 'Descuento', " & _
        "FORMAT(sid.dsupplierinvoicediscountpercentage, 2) AS 'Porcentaje Descuento' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts sid " & _
        "WHERE " & _
        "sid.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDescuentos, querySupplierInvoiceDiscounts, False)

        dgvDescuentos.Columns(0).Visible = False
        dgvDescuentos.Columns(1).Visible = False

        dgvDescuentos.Columns(0).ReadOnly = True
        dgvDescuentos.Columns(1).ReadOnly = True

        dgvDescuentos.Columns(0).Width = 30
        dgvDescuentos.Columns(1).Width = 30
        dgvDescuentos.Columns(2).Width = 200
        dgvDescuentos.Columns(3).Width = 70




        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        txtSubtotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            Dim subTotal As Double = 0.0
            subTotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subTotal = getSQLQueryAsDouble(0, "SELECT " & subTotal & " - (" & subTotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i

            txtSubtotalTrasDescuentos.Text = FormatCurrency(subTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIVA.Text = FormatCurrency(subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(subTotal + (subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Else

            txtSubtotalTrasDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS totalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If isEdit = True Then
            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió los Descuentos de la Factura " & isupplierinvoiceid & "', 'OK')")
        Else
            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Comenzando Nuevos Descuentos para la Factura " & isupplierinvoiceid & "', 'OK')")
        End If

        'executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Descuento de Factura de Proveedor', '" & isupplierinvoiceid & "', '', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

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


    Private Sub dgvDescuentos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDescuentos.CellClick

        Try

            If dgvDescuentos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinvoicediscountid = CInt(dgvDescuentos.Rows(e.RowIndex).Cells(1).Value())
            sselectedinvoicediscountdescription = dgvDescuentos.Rows(e.RowIndex).Cells(2).Value
            dselectedinvoicediscountpercentage = CDbl(dgvDescuentos.Rows(e.RowIndex).Cells(3).Value)

        Catch ex As Exception

            iselectedinvoicediscountid = 0
            sselectedinvoicediscountdescription = ""
            dselectedinvoicediscountpercentage = 0.0

        End Try

    End Sub


    Private Sub dgvDescuentos_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDescuentos.CellContentClick

        Try

            If dgvDescuentos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinvoicediscountid = CInt(dgvDescuentos.Rows(e.RowIndex).Cells(1).Value())
            sselectedinvoicediscountdescription = dgvDescuentos.Rows(e.RowIndex).Cells(2).Value
            dselectedinvoicediscountpercentage = CDbl(dgvDescuentos.Rows(e.RowIndex).Cells(3).Value)

        Catch ex As Exception

            iselectedinvoicediscountid = 0
            sselectedinvoicediscountdescription = ""
            dselectedinvoicediscountpercentage = 0.0

        End Try

    End Sub


    Private Sub dgvDescuentos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDescuentos.SelectionChanged

        Try

            If dgvDescuentos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinvoicediscountid = CInt(dgvDescuentos.CurrentRow.Cells(1).Value())
            sselectedinvoicediscountdescription = dgvDescuentos.CurrentRow.Cells(2).Value
            dselectedinvoicediscountpercentage = CDbl(dgvDescuentos.CurrentRow.Cells(3).Value)

        Catch ex As Exception

            iselectedinvoicediscountid = 0
            sselectedinvoicediscountdescription = ""
            dselectedinvoicediscountpercentage = 0.0

        End Try

    End Sub


    Private Sub dgvDescuentos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDescuentos.CellEndEdit

        Dim querySupplierInvoiceDiscounts As String
        querySupplierInvoiceDiscounts = "" & _
        "SELECT sid.isupplierinvoiceid, sid.isupplierinvoicediscountid, sid.ssupplierinvoicediscountdescription AS 'Descuento', " & _
        "FORMAT(sid.dsupplierinvoicediscountpercentage, 2) AS 'Porcentaje Descuento' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts sid " & _
        "WHERE " & _
        "sid.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDescuentos, querySupplierInvoiceDiscounts, False)

        dgvDescuentos.Columns(0).Visible = False
        dgvDescuentos.Columns(1).Visible = False

        dgvDescuentos.Columns(0).ReadOnly = True
        dgvDescuentos.Columns(1).ReadOnly = True

        dgvDescuentos.Columns(0).Width = 30
        dgvDescuentos.Columns(1).Width = 30
        dgvDescuentos.Columns(2).Width = 200
        dgvDescuentos.Columns(3).Width = 70


    End Sub


    Private Sub dgvDescuentos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDescuentos.CellValueChanged

        If modifyDiscountPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        'LAS UNICAS COLUMNAS EDITABLES SON LAs 2, 3 : ssupplierinvoicediscountdescription, dsupplierinvoicediscountpercentage

        If e.ColumnIndex = 2 Then   'ssupplierinvoicediscountdescription

            If dgvDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar este Descuento de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Descuento de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & iselectedinvoicediscountid) = True Then

                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Descuento (" & dselectedinvoicediscountpercentage * 100 & " %) de la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")

                    End If

                Else
                    'Nothing
                End If

            Else

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts SET ssupplierinvoicediscountdescription = '" & dgvDescuentos.Rows(e.RowIndex).Cells(2).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & iselectedinvoicediscountid) = True Then

                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Actualizó la descripción del Descuento " & iselectedinvoicediscountid & " de la Factura de Combustible " & isupplierinvoiceid & " a " & dgvDescuentos.Rows(e.RowIndex).Cells(2).Value & "', 'OK')")

                End If

            End If

        ElseIf e.ColumnIndex = 3 Then 'dsupplierinvoicediscountpercentage


            If dgvDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar este Descuento de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Descuento de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & iselectedinvoicediscountid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Descuento (" & dselectedinvoicediscountpercentage * 100 & " %) de la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")
                    End If

                Else
                    'dgvDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinvoicediscountdescription
                End If

            ElseIf dgvDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Está seguro que deseas quitar este Descuento de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Descuento de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & iselectedinvoicediscountid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Descuento (" & dselectedinvoicediscountpercentage * 100 & " %) de la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")
                    End If

                Else
                    'dgvDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinvoicediscountdescription
                End If

            Else

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts SET dsupplierinvoicediscountpercentage = " & dgvDescuentos.Rows(e.RowIndex).Cells(3).Value & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & iselectedinvoicediscountid) = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Actualizó el porcentaje del Descuento " & iselectedinvoicediscountid & " de la Factura de Combustible " & isupplierinvoiceid & " a " & dgvDescuentos.Rows(e.RowIndex).Cells(3).Value & "', 'OK')")
                End If

            End If

        End If


        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        txtSubtotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            Dim subTotal As Double = 0.0
            subTotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subTotal = getSQLQueryAsDouble(0, "SELECT " & subTotal & " - (" & subTotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i

            txtSubtotalTrasDescuentos.Text = FormatCurrency(subTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIVA.Text = FormatCurrency(subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(subTotal + (subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Else

            txtSubtotalTrasDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS totalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvDescuentos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvDescuentos.EditingControlShowing

        'If dgvDescuentos.CurrentCell.ColumnIndex = 2 Then

        '    txtNombreDescuentoDgvDescuentos = CType(e.Control, TextBox)
        '    txtNombreDescuentoDgvDescuentos_OldText = txtNombreDescuentoDgvDescuentos.Text

        'Else
        If dgvDescuentos.CurrentCell.ColumnIndex = 3 Then

            txtNumeroDgvDescuentos = CType(e.Control, TextBox)
            txtNumeroDgvDescuentos_OldText = txtNumeroDgvDescuentos.Text

        Else

            txtNumeroDgvDescuentos = Nothing
            txtNumeroDgvDescuentos_OldText = Nothing
            txtNombreDescuentoDgvDescuentos = Nothing
            txtNombreDescuentoDgvDescuentos_OldText = Nothing

        End If

    End Sub


    Private Sub txtNumeroDgvDescuentos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroDgvDescuentos.KeyUp

        If Not IsNumeric(txtNumeroDgvDescuentos.Text) Then

            Dim strForbiddenDiscount2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbiddenDiscount2 As Char() = strForbiddenDiscount2.ToCharArray

            For cp = 0 To arrayForbiddenDiscount2.Length - 1

                If txtNumeroDgvDescuentos.Text.Contains(arrayForbiddenDiscount2(cp)) Then
                    txtNumeroDgvDescuentos.Text = txtNumeroDgvDescuentos.Text.Replace(arrayForbiddenDiscount2(cp), "")
                End If

            Next cp

            If txtNumeroDgvDescuentos.Text.Contains(".") Then

                Dim comparaPuntosDiscount As Char() = txtNumeroDgvDescuentos.Text.ToCharArray
                Dim cuantosPuntosDiscount As Integer = 0


                For letra = 0 To comparaPuntosDiscount.Length - 1

                    If comparaPuntosDiscount(letra) = "." Then
                        cuantosPuntosDiscount = cuantosPuntosDiscount + 1
                    End If

                Next

                If cuantosPuntosDiscount > 1 Then

                    For cantidad = 1 To cuantosPuntosDiscount
                        Dim lugarDiscount As Integer = txtNumeroDgvDescuentos.Text.LastIndexOf(".")
                        Dim longitudDiscount As Integer = txtNumeroDgvDescuentos.Text.Length

                        If longitudDiscount > (lugarDiscount + 1) Then
                            txtNumeroDgvDescuentos.Text = txtNumeroDgvDescuentos.Text.Substring(0, lugarDiscount) & txtNumeroDgvDescuentos.Text.Substring(lugarDiscount + 1)
                            Exit For
                        Else
                            txtNumeroDgvDescuentos.Text = txtNumeroDgvDescuentos.Text.Substring(0, lugarDiscount)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtNumeroDgvDescuentos.Text = txtNumeroDgvDescuentos.Text.Replace("--", "").Replace("'", "")
            txtNumeroDgvDescuentos.Text = txtNumeroDgvDescuentos.Text.Trim

        Else
            txtNumeroDgvDescuentos_OldText = txtNumeroDgvDescuentos.Text
        End If

    End Sub


    Private Sub txtNombreDescuentoDgvDescuentos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreDescuentoDgvDescuentos.KeyUp

        Dim strcaracteresprohibidos9 As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib9 As Char() = strcaracteresprohibidos9.ToCharArray
        Dim resultadoDiscount As Boolean = False

        For carp = 0 To arrayCaractProhib9.Length - 1

            If txtNombreDescuentoDgvDescuentos.Text.Contains(arrayCaractProhib9(carp)) Then
                txtNombreDescuentoDgvDescuentos.Text = txtNombreDescuentoDgvDescuentos.Text.Replace(arrayCaractProhib9(carp), "")
                resultadoDiscount = True
            End If

        Next carp

        If resultadoDiscount = True Then
            txtNombreDescuentoDgvDescuentos.Select(txtNombreDescuentoDgvDescuentos.Text.Length, 0)
        End If

    End Sub


    Private Sub dgvDescuentos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvDescuentos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteDiscountPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas quitar este Descuento de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Descuento de la Factura") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedinvoicediscountid As Integer = 0
                Try
                    tmpselectedinvoicediscountid = CInt(dgvDescuentos.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & tmpselectedinvoicediscountid) = True Then

                    Dim fecha As Integer = getMySQLDate()
                    Dim hora As String = getAppTime()

                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Descuento (" & dselectedinvoicediscountpercentage * 100 & " %) de la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")

                End If

                Dim dsDescuentos As DataSet
                dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                txtSubtotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                If dsDescuentos.Tables(0).Rows.Count > 0 Then

                    Dim subTotal As Double = 0.0
                    subTotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                    For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                        subTotal = getSQLQueryAsDouble(0, "SELECT " & subTotal & " - (" & subTotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                    Next i

                    txtSubtotalTrasDescuentos.Text = FormatCurrency(subTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    txtIVA.Text = FormatCurrency(subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    txtTotal.Text = FormatCurrency(subTotal + (subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Else

                    txtSubtotalTrasDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    txtIVA.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS totalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                End If

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvDescuentos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvDescuentos.UserAddedRow

        If newDiscountPermission = False Then
            Exit Sub
        End If

        Dim tmpPermanentisupplierinvoicediscountid As Integer = 0
        Dim tmpVolatilisupplierinvoicediscountid As Integer = 0

        tmpPermanentisupplierinvoicediscountid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoicediscountid) + 1 IS NULL, 1, MAX(isupplierinvoicediscountid) + 1) AS isupplierinvoicediscountid FROM supplierinvoicediscounts ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
        tmpVolatilisupplierinvoicediscountid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoicediscountid) + 1 IS NULL, 1, MAX(isupplierinvoicediscountid) + 1) AS isupplierinvoicediscountid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If tmpPermanentisupplierinvoicediscountid > tmpVolatilisupplierinvoicediscountid Then
            iselectedinvoicediscountid = tmpPermanentisupplierinvoicediscountid
        ElseIf tmpVolatilisupplierinvoicediscountid > tmpPermanentisupplierinvoicediscountid Then
            iselectedinvoicediscountid = tmpVolatilisupplierinvoicediscountid
        Else 'Iguales
            iselectedinvoicediscountid = tmpPermanentisupplierinvoicediscountid
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts VALUES ( " & isupplierinvoiceid & ", " & iselectedinvoicediscountid & ", '', 0.01, " & fecha & ", '" & hora & "', '" & susername & "')") = True Then
            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agregó un Nuevo Descuento a la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")
        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoDescuento.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim tmpPermanentisupplierinvoicediscountid As Integer = 0
        Dim tmpVolatilisupplierinvoicediscountid As Integer = 0

        tmpPermanentisupplierinvoicediscountid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoicediscountid) + 1 IS NULL, 1, MAX(isupplierinvoicediscountid) + 1) AS isupplierinvoicediscountid FROM supplierinvoicediscounts ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
        tmpVolatilisupplierinvoicediscountid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoicediscountid) + 1 IS NULL, 1, MAX(isupplierinvoicediscountid) + 1) AS isupplierinvoicediscountid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If tmpPermanentisupplierinvoicediscountid > tmpVolatilisupplierinvoicediscountid Then
            iselectedinvoicediscountid = tmpPermanentisupplierinvoicediscountid
        ElseIf tmpVolatilisupplierinvoicediscountid > tmpPermanentisupplierinvoicediscountid Then
            iselectedinvoicediscountid = tmpVolatilisupplierinvoicediscountid
        Else 'Iguales
            iselectedinvoicediscountid = tmpPermanentisupplierinvoicediscountid
        End If


        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts VALUES ( " & isupplierinvoiceid & ", " & iselectedinvoicediscountid & ", '', 0.01, " & fecha & ", '" & hora & "', '" & susername & "')") = True Then
            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agregó un Nuevo Descuento a la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")
        End If


        Dim querySupplierInvoiceDiscounts As String
        querySupplierInvoiceDiscounts = "" & _
        "SELECT sid.isupplierinvoiceid, sid.isupplierinvoicediscountid, sid.ssupplierinvoicediscountdescription AS 'Descuento', " & _
        "FORMAT(sid.dsupplierinvoicediscountpercentage, 2) AS 'Porcentaje Descuento' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts sid " & _
        "WHERE " & _
        "sid.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDescuentos, querySupplierInvoiceDiscounts, False)

        dgvDescuentos.Columns(0).Visible = False
        dgvDescuentos.Columns(1).Visible = False

        dgvDescuentos.Columns(0).ReadOnly = True
        dgvDescuentos.Columns(1).ReadOnly = True

        dgvDescuentos.Columns(0).Width = 30
        dgvDescuentos.Columns(1).Width = 30
        dgvDescuentos.Columns(2).Width = 200
        dgvDescuentos.Columns(3).Width = 70

        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        txtSubtotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            Dim subTotal As Double = 0.0
            subTotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subTotal = getSQLQueryAsDouble(0, "SELECT " & subTotal & " - (" & subTotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i

            txtSubtotalTrasDescuentos.Text = FormatCurrency(subTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIVA.Text = FormatCurrency(subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(subTotal + (subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Else

            txtSubtotalTrasDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS totalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarDescuento.Click

        If MsgBox("¿Está seguro que deseas quitar este Descuento de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Descuento de la Factura") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedinvoicediscountid As Integer = 0
            Try
                tmpselectedinvoicediscountid = CInt(dgvDescuentos.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try


            If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & tmpselectedinvoicediscountid) = True Then

                Dim fecha As Integer = getMySQLDate()
                Dim hora As String = getAppTime()

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Descuento (" & dselectedinvoicediscountpercentage * 100 & " %) de la Factura de Combustible " & isupplierinvoiceid & "', 'OK')")

            End If


            Dim querySupplierInvoiceDiscounts As String
            querySupplierInvoiceDiscounts = "" & _
            "SELECT sid.isupplierinvoiceid, sid.isupplierinvoicediscountid, sid.ssupplierinvoicediscountdescription AS 'Descuento', " & _
            "FORMAT(sid.dsupplierinvoicediscountpercentage, 2) AS 'Porcentaje Descuento' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts sid " & _
            "WHERE " & _
            "sid.isupplierinvoiceid = " & isupplierinvoiceid

            setDataGridView(dgvDescuentos, querySupplierInvoiceDiscounts, False)

            dgvDescuentos.Columns(0).Visible = False
            dgvDescuentos.Columns(1).Visible = False

            dgvDescuentos.Columns(0).ReadOnly = True
            dgvDescuentos.Columns(1).ReadOnly = True

            dgvDescuentos.Columns(0).Width = 30
            dgvDescuentos.Columns(1).Width = 30
            dgvDescuentos.Columns(2).Width = 200
            dgvDescuentos.Columns(3).Width = 70

            Dim dsDescuentos As DataSet
            dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            txtSubtotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            If dsDescuentos.Tables(0).Rows.Count > 0 Then

                Dim subTotal As Double = 0.0
                subTotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                    subTotal = getSQLQueryAsDouble(0, "SELECT " & subTotal & " - (" & subTotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                Next i

                txtSubtotalTrasDescuentos.Text = FormatCurrency(subTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                txtIVA.Text = FormatCurrency(subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtTotal.Text = FormatCurrency(subTotal + (subTotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Else

                txtSubtotalTrasDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtIVA.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS totalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Function validaDatosDescuento(ByVal silent As Boolean) As Boolean

        Dim subTotalTrasDescuentos As Double = 0.0

        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            subTotalTrasDescuentos = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subTotalTrasDescuentos = getSQLQueryAsDouble(0, "SELECT " & subTotalTrasDescuentos & " - (" & subTotalTrasDescuentos & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i


        Else

            subTotalTrasDescuentos = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        If subTotalTrasDescuentos <= 0 And dsDescuentos.Tables(0).Rows.Count > 0 Then
            If silent = False Then
                MsgBox("No está permitido que el importe de la factura sea 0 o menor que 0.", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        Return True

    End Function


    Private Sub btnCerrarDescuentos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrarDescuentos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class