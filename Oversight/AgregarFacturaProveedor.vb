Public Class AgregarFacturaProveedor

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

    Public isupplierinvoiceid As Integer = 0
    Public ipeopleid As Integer = 0
    Public isupplierid As Integer = 0
    Public ssuppliername As String = ""
    Public ssupplierinvoicedescription As String = ""

    Private iselectedinputid As Integer = 0
    Private sselectedinputdescription As String = ""
    Private iselectedinputwithoutrelationid As Integer = 0
    Private iselectedinputwithrelationid As Integer = 0
    Private iselectedrelationtypeid As Integer = 0
    Private iselectedrelationid As Integer = 0
    Private iselectedpaymentid As Integer = 0
    Private sselectedpaymentdescription As String = ""

    Private WithEvents txtInsumoDgvDetalle As TextBox
    Private WithEvents txtNotasDgvDetalle As TextBox
    Private WithEvents txtCantidad1DgvDetalle As TextBox
    Private WithEvents txtCantidad2DgvDetalle As TextBox
    Private WithEvents txtCantidad3DgvDetalle As TextBox
    Private WithEvents txtCantidadDgvDetalleConRelacion As TextBox
    Private WithEvents txtObservacionesDgvPagos As TextBox

    Private txtInsumoDgvDetalle_OldText As String = ""
    Private txtNotasDgvDetalle_OldText As String = ""
    Private txtCantidad1DgvDetalle_OldText As String = ""
    Private txtCantidad2DgvDetalle_OldText As String = ""
    Private txtCantidad3DgvDetalle_OldText As String = ""
    Private txtCantidadDgvDetalleConRelacion_OldText As String = ""
    Private txtObservacionesDgvPagos_OldText As String = ""

    Private openPermission As Boolean = False

    Private newInputPermission As Boolean = False
    Private insertInputPermission As Boolean = False
    Private modifyInputPermission As Boolean = False
    Private deleteInputPermission As Boolean = False
    Private openInputPermission As Boolean = False

    Private addRelationPermission As Boolean = False
    Private modifyRelationPermission As Boolean = False
    Private deleteRelationPermission As Boolean = False

    Private openProjectPermission As Boolean = False
    Private openAssetPermission As Boolean = False

    Private openPaymentPermission As Boolean = False
    Private newPaymentPermission As Boolean = False
    Private insertPaymentPermission As Boolean = False
    Private modifyPaymentPermission As Boolean = False
    Private deletePaymentPermission As Boolean = False

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

                If permission = "Modificar Factura" Then
                    btnPaso2.Enabled = True
                    btnGuardarDatosFactura.Enabled = True
                    btnGuardarDatosFacturaYCerrar.Enabled = True
                End If

                If permission = "Modificar Relaciones" Then
                    btnPaso3.Enabled = True
                    btnGuardarRelacion.Enabled = True
                    btnGuardarRelacionYCerrar.Enabled = True
                End If

                If permission = "Modificar Pagos" Then
                    btnGuardarPago.Enabled = True
                    btnGuardarPagoYCerrar.Enabled = True
                End If

                If permission = "Abrir Proveedor" Then
                    btnProveedor.Enabled = True
                End If

                If permission = "Abrir Tipos de Factura" Then
                    btnTipoFactura.Enabled = True
                End If

                If permission = "Abrir Persona" Then
                    btnPersona.Enabled = True
                End If

                If permission = "Nuevo Insumo" Then
                    newInputPermission = True
                    btnNuevoInsumo.Enabled = True
                End If

                If permission = "Insertar Insumo" Then
                    insertInputPermission = True
                    btnInsertarInsumo.Enabled = True
                    btnDescuentos.Enabled = True
                End If

                If permission = "Eliminar Insumo" Then
                    deleteInputPermission = True
                    btnEliminarInsumo.Enabled = True
                End If

                If permission = "Modificar Insumo" Then
                    modifyInputPermission = True
                End If

                If permission = "Ver Relaciones" Then
                    dgvDetalleSinRelacion.Visible = True
                    dgvDetalleConRelacion.Visible = True
                End If

                If permission = "Agregar Relacion" Then
                    addRelationPermission = True
                    btnBuscarRelacion.Enabled = True
                End If

                If permission = "Modificar Relacion" Then
                    modifyRelationPermission = True
                    dgvDetalleConRelacion.ReadOnly = False
                    dgvDetalleConRelacion.Enabled = True
                End If

                If permission = "Eliminar Relacion" Then
                    deleteRelationPermission = True
                    btnEliminarRelacion.Enabled = True
                End If

                If permission = "Abrir Proyecto" Then
                    openProjectPermission = True
                End If

                If permission = "Abrir Activo" Then
                    openAssetPermission = True
                End If

                If permission = "Ver Pagos" Then
                    dgvPagos.Visible = True
                    lblSumaPagos.Visible = True
                    txtSumaPagos.Visible = True
                    lblRestanteAPagar.Visible = True
                    txtMontoRestante.Visible = True
                End If

                If permission = "Abrir Pago" Then
                    openPaymentPermission = True
                End If

                If permission = "Modificar Pago" Then
                    modifyPaymentPermission = True
                    dgvPagos.Enabled = True
                End If

                If permission = "Nuevo Pago" Then
                    newPaymentPermission = True
                    btnAgregarPago.Enabled = True
                End If

                If permission = "Insertar Pago" Then
                    insertPaymentPermission = True
                    btnInsertarPago.Enabled = True
                End If

                If permission = "Eliminar Pago" Then
                    deletePaymentPermission = True
                    btnEliminarPago.Enabled = True
                End If

                If permission = "Ver Revisiones" Then
                    btnRevisiones.Enabled = True
                    btnRevisionesPagos.Enabled = True
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

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Factura de Proveedores', 'OK')")

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


    Private Sub AgregarFacturaProveedor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

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
        Dim conteo10 As Integer = 0
        Dim conteo11 As Integer = 0
        Dim conteo12 As Integer = 0
        Dim conteo13 As Integer = 0
        Dim conteo14 As Integer = 0
        Dim conteo15 As Integer = 0
        Dim conteo16 As Integer = 0
        Dim conteo17 As Integer = 0
        Dim conteo18 As Integer = 0

        Dim conteogrupo1 As Integer = 0
        Dim conteogrupo2 As Integer = 0
        Dim conteogrupo3 As Integer = 0

        Dim unsaved As Boolean = False

        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierinvoices " & _
        "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierinvoiceinputs " & _
        "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierinvoiceprojects " & _
        "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierinvoiceassets " & _
        "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM supplierinvoicepayments " & _
        "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) ")

        conteo7 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsi.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi JOIN supplierinvoices si ON tsi.isupplierinvoiceid = si.isupplierinvoiceid AND tsi.iinputid = si.iinputid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsii.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii JOIN supplierinvoiceinputs sii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') ")

        conteo10 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsip.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip JOIN supplierinvoiceprojects sip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') ")

        conteo11 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsia.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia JOIN supplierinvoiceassets sia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') ")

        conteo12 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tsipy.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy JOIN supplierinvoicepayments sipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') ")

        conteo13 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") ")

        conteo14 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") ")

        conteo16 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") ")

        conteo17 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") ")

        conteo18 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
        "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") ")

        conteogrupo1 = conteo1 + conteo2 + conteo3 + conteo7 + conteo8 + conteo9 + conteo13 + conteo14 + conteo15
        conteogrupo2 = conteo4 + conteo5 + conteo10 + conteo11 + conteo16 + conteo17
        conteogrupo3 = conteo6 + conteo12 + conteo18

        If conteogrupo1 + conteogrupo2 + conteogrupo3 > 0 Then

            unsaved = True

        End If

        Dim incomplete As Boolean = False
        Dim stepNumber As Integer = 0
        Dim msg As String = ""
        Dim result As Integer = 0

        If validaFacturaCompleta(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK Then

            incomplete = True
            stepNumber = 1

        Else

            If validaRelaciones(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK And getSQLQueryAsInteger(0, "SELECT SUM(conteo) AS conteo FROM (SELECT COUNT(*) AS conteo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia WHERE isupplierinvoiceid = " & isupplierinvoiceid & " UNION SELECT COUNT(*) AS conteo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip WHERE isupplierinvoiceid = " & isupplierinvoiceid & ") c") > 0 Then

                incomplete = True
                stepNumber = 2

            Else

                If validaPagos(True) = False And Me.DialogResult <> Windows.Forms.DialogResult.OK And getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sip WHERE isupplierinvoiceid = " & isupplierinvoiceid) > 0 Then
                    incomplete = True
                    stepNumber = 3
                End If

            End If

        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True And stepNumber = 1 Then
            result = MsgBox("Esta Factura está incompleta. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf incomplete = True And stepNumber = 2 Then
            result = MsgBox("Las Relaciones de esta Factura estan incompletas. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf incomplete = True And stepNumber = 3 Then
            result = MsgBox("Los Pagos de esta Factura estan incompletos. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf conteogrupo1 > 0 Or conteogrupo2 > 0 Or conteogrupo3 > 0 Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        ElseIf conteogrupo1 + conteogrupo2 Then
            result = MsgBox("Tienes datos sin guardar! y son de áreas diferentes (Facturas y Relaciones). Tienes 3 opciones: " & Chr(13) & "Guardar TODOS los cambios (Sí), Regresar a revisarlos y guardarlos los que deseas guardar (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        ElseIf conteogrupo1 + conteogrupo3 Then
            result = MsgBox("Tienes datos sin guardar! y son de áreas diferentes (Facturas y Pagos). Tienes 3 opciones: " & Chr(13) & "Guardar TODOS los cambios (Sí), Regresar a revisarlos y guardarlos los que deseas guardar (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        ElseIf conteogrupo2 + conteogrupo3 Then
            result = MsgBox("Tienes datos sin guardar! y son de áreas diferentes (Relaciones y Pagos). Tienes 3 opciones: " & Chr(13) & "Guardar TODOS los cambios (Sí), Regresar a revisarlos y guardarlos los que deseas guardar (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        ElseIf result = MsgBoxResult.Yes And incomplete = False Then

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(4) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If


            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & " al Cerrar', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                unsaved = False
                wasCreated = True


            Else

                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub

            End If



        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If



        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim queriesDelete(9) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs"
        queriesDelete(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects"
        queriesDelete(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments"
        queriesDelete(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets"
        queriesDelete(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts"

        If unsaved = True Then
            queriesDelete(7) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Factura " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " (" & isupplierinvoiceid & ") del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & " SIN Guardar', 'OK')"
        Else
            queriesDelete(7) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró la Factura " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " (" & isupplierinvoiceid & ") del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')"
        End If

        queriesDelete(8) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'SupplierInvoice', 'Factura de Proveedor', '" & isupplierinvoiceid & "', '" & txtDescripcionFactura.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarFacturaProveedor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarFacturaProveedor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        Dim timesInvoiceIsOpen As Integer = 0

        timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

        If timesInvoiceIsOpen > 0 And isEdit = True Then

            If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar abriendo?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Exit Sub

            End If

        End If


        If isRecover = False Then

            Dim queriesCreation(14) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierid` int(11) NOT NULL, `iinvoicedate` int(11) NOT NULL, `sinvoicetime` varchar(11) CHARACTER SET latin1 NOT NULL, `isupplierinvoicetypeid` int(11) NOT NULL, `ssupplierinvoicefolio` varchar(20) CHARACTER SET latin1 NOT NULL, `sexpensedescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `sexpenselocation` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `dIVApercentage` decimal(20,5) NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`), KEY `isupplierid` (`isupplierid`), KEY `folioid` (`ssupplierinvoicefolio`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `sinputextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoiceinputtotalprice` decimal(20,5) DEFAULT NULL, `dsupplierinvoiceinputunitprice` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `iinputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects ( `isupplierinvoiceid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iprojectid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `projectid` (`iprojectid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments"
            queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments ( `ipaymentid` int(11) NOT NULL, `isupplierinvoiceid` int(11) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`,`isupplierinvoiceid`), KEY `supplierinvoiceid` (`isupplierinvoiceid`), KEY `updateuser` (`supdateusername`), KEY `paymentid` (`ipaymentid`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets"
            queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets ( `isupplierinvoiceid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iinsertiondate` int(11) NOT NULL, `sinsertiontime` varchar(11) CHARACTER SET latin1 NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iassetid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `assetid` (`iassetid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts"
            queriesCreation(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierinvoicediscountid` int(11) NOT NULL, `ssupplierinvoicediscountdescription` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoicediscountpercentage` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`isupplierinvoicediscountid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `supplierinvoicediscountid` (`isupplierinvoicediscountid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        cmbTipoFactura.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM supplierinvoicetypes ORDER BY ssupplierinvoicetypedescription")
        cmbTipoFactura.DisplayMember = "ssupplierinvoicetypedescription"
        cmbTipoFactura.ValueMember = "isupplierinvoicetypeid"
        cmbTipoFactura.SelectedIndex = -1


        If isEdit = False Then

            txtProveedor.Text = ""
            txtPersona.Text = ""
            txtLugar.Text = "Tuxtla Gutierrez, Chiapas"
            txtFolioFacturaProveedor.Text = ""
            txtPorcentajeIVA.Text = "16"

            btnNuevoInsumo.Enabled = False
            btnInsertarInsumo.Enabled = False
            btnEliminarInsumo.Enabled = False
            btnEliminarRelacion.Enabled = False
            btnAgregarPago.Enabled = False
            btnInsertarPago.Enabled = False
            btnEliminarPago.Enabled = False
            btnDescuentos.Enabled = False
            btnRevisiones.Enabled = False
            btnRevisionesPagos.Enabled = False

        Else

            If isRecover = False Then

                Dim queriesInsert(7) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SELECT * FROM supplierinvoices WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs SELECT * FROM supplierinvoiceinputs WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects SELECT * FROM supplierinvoiceprojects WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments SELECT * FROM supplierinvoicepayments WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets SELECT * FROM supplierinvoiceassets WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesInsert(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts SELECT * FROM supplierinvoicediscounts WHERE isupplierinvoiceid = " & isupplierinvoiceid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim querySupplierInvoice As String
            Dim dsSupplierInvoice As DataSet

            querySupplierInvoice = "" & _
            "SELECT si.isupplierinvoiceid, s.isupplierid, s.ssuppliername, " & _
            "si.iupdatedate, si.supdatetime, si.iinvoicedate, si.sinvoicetime, " & _
            "si.isupplierinvoicetypeid, si.ssupplierinvoicefolio, si.sexpensedescription, si.sexpenselocation, FORMAT(si.dIVApercentage, 2) AS dIVApercentage, " & _
            "pe.ipeopleid, pe.speoplefullname " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
            "JOIN suppliers s ON si.isupplierid = s.isupplierid " & _
            "LEFT JOIN people pe ON si.ipeopleid = pe.ipeopleid " & _
            "WHERE si.isupplierinvoiceid = " & isupplierinvoiceid

            dsSupplierInvoice = getSQLQueryAsDataset(0, querySupplierInvoice)

            dtFechaFactura.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsSupplierInvoice.Tables(0).Rows(0).Item("iinvoicedate"))
            isupplierid = dsSupplierInvoice.Tables(0).Rows(0).Item("isupplierid")
            txtProveedor.Text = dsSupplierInvoice.Tables(0).Rows(0).Item("ssuppliername")
            txtFolioFacturaProveedor.Text = dsSupplierInvoice.Tables(0).Rows(0).Item("ssupplierinvoicefolio")
            txtDescripcionFactura.Text = dsSupplierInvoice.Tables(0).Rows(0).Item("sexpensedescription")

            cmbTipoFactura.SelectedValue = dsSupplierInvoice.Tables(0).Rows(0).Item("isupplierinvoicetypeid")

            txtPorcentajeIVA.Text = FormatCurrency(dsSupplierInvoice.Tables(0).Rows(0).Item("dIVApercentage") * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtLugar.Text = dsSupplierInvoice.Tables(0).Rows(0).Item("sexpenselocation")
            ipeopleid = dsSupplierInvoice.Tables(0).Rows(0).Item("ipeopleid")
            txtPersona.Text = dsSupplierInvoice.Tables(0).Rows(0).Item("speoplefullname")

        End If


        Dim querySupplierInvoiceInputs As String
        querySupplierInvoiceInputs = "" & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', sii.sinputextranote AS 'Marca/Observaciones', FORMAT(sii.dinputqty, 2) AS Cantidad, i.sinputunit AS 'Unidad', " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(sii.dsupplierinvoiceinputtotalprice, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDetalle, querySupplierInvoiceInputs, False)

        dgvDetalle.Columns(0).Visible = False
        dgvDetalle.Columns(1).Visible = False

        dgvDetalle.Columns(0).ReadOnly = True
        dgvDetalle.Columns(1).ReadOnly = True
        dgvDetalle.Columns(2).ReadOnly = True
        dgvDetalle.Columns(5).ReadOnly = True

        dgvDetalle.Columns(0).Width = 30
        dgvDetalle.Columns(1).Width = 30
        dgvDetalle.Columns(2).Width = 100
        dgvDetalle.Columns(3).Width = 100
        dgvDetalle.Columns(4).Width = 70
        dgvDetalle.Columns(5).Width = 70
        dgvDetalle.Columns(6).Width = 70

        If isEdit = False Then
            dgvDetalle.Enabled = False
        Else
            dgvDetalle.Enabled = True
        End If

        If isEdit = True Then

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

        Else

            txtSubtotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtSubtotalTrasDescuentos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtTotal.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) + IF(SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice) * si.dIVApercentage), 2) AS totalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si ON sii.isupplierinvoiceid = si.isupplierinvoiceid WHERE sii.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If


        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        If isEdit = True Then
            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " (" & isupplierinvoiceid & ") del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
        Else
            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Comenzando Nueva Factura de Proveedor', 'OK')")
        End If

        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'SupplierInvoice', 'Factura de Proveedor', '" & isupplierinvoiceid & "', '" & txtDescripcionFactura.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True

        btnProveedor.Select()
        btnProveedor.Focus()

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


    Private Sub btnProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtProveedor.Text = txtProveedor.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", "")

        Dim bp As New BuscaProveedores

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.querystring = txtProveedor.Text.Trim

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            txtProveedor.Text = bp.ssuppliername
            isupplierid = bp.isupplierid

            txtLugar.Text = bp.ssupplierplace.Replace("'", "").Replace(",", " ").Replace("--", "")

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            If isEdit = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió el Proveedor por " & bp.ssuppliername & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            Else
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'El Proveedor de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " es " & bp.ssuppliername & "', 'OK')")
            End If

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            If validaDatosFactura(True, False) = True And isupplierinvoiceid = 0 Then

                isupplierinvoiceid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoiceid) + 1 IS NULL, 1, MAX(isupplierinvoiceid) + 1) AS isupplierinvoiceid FROM supplierinvoices ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(21) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierid` int(11) NOT NULL, `iinvoicedate` int(11) NOT NULL, `sinvoicetime` varchar(11) CHARACTER SET latin1 NOT NULL, `isupplierinvoicetypeid` int(11) NOT NULL, `ssupplierinvoicefolio` varchar(20) CHARACTER SET latin1 NOT NULL, `sexpensedescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `sexpenselocation` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `dIVApercentage` decimal(20,5) NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`), KEY `isupplierid` (`isupplierid`), KEY `folioid` (`ssupplierinvoicefolio`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Inputs"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `sinputextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoiceinputtotalprice` decimal(20,5) DEFAULT NULL, `dsupplierinvoiceinputunitprice` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `iinputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(9) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Projects"
                queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects"
                queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects ( `isupplierinvoiceid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iprojectid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `projectid` (`iprojectid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Payments"
                queriesCreation(13) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments"
                queriesCreation(14) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments ( `ipaymentid` int(11) NOT NULL, `isupplierinvoiceid` int(11) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`,`isupplierinvoiceid`), KEY `supplierinvoiceid` (`isupplierinvoiceid`), KEY `updateuser` (`supdateusername`), KEY `paymentid` (`ipaymentid`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Assets"
                queriesCreation(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets"
                queriesCreation(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets ( `isupplierinvoiceid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iassetid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `assetid` (`iassetid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Discounts"
                queriesCreation(19) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts"
                queriesCreation(20) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierinvoicediscountid` int(11) NOT NULL, `ssupplierinvoicediscountdescription` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoicediscountpercentage` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`isupplierinvoicediscountid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `supplierinvoicediscountid` (`isupplierinvoicediscountid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                Dim tipofactura As Integer = 0

                Try
                    tipofactura = cmbTipoFactura.SelectedValue
                Catch ex As Exception

                End Try

                If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " VALUES (" & isupplierinvoiceid & ", " & isupplierid & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "").Replace("'", "") & "', " & tipofactura & ", '" & txtFolioFacturaProveedor.Text & "', '" & txtDescripcionFactura.Text & "', '" & txtLugar.Text & "', " & porcentajeIVA / 100 & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                    dgvDetalle.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                    btnEliminarRelacion.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    btnDescuentos.Enabled = True

                End If


            ElseIf validaDatosFactura(True, False) = True And isupplierinvoiceid > 0 Then

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then

                    dgvDetalle.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                    btnEliminarRelacion.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    btnDescuentos.Enabled = True

                End If

            End If

            dtFechaFactura.Focus()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dtFechaFactura_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtFechaFactura.ValueChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió la Fecha de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " a " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & "', 'OK')")
            End If

        End If

    End Sub


    Private Sub btnPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPersona.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        txtPersona.Text = txtPersona.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", "")

        Dim bp As New BuscaPersonas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname

        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.querystring = txtPersona.Text.Trim

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            txtPersona.Text = bp.speoplefullname
            ipeopleid = bp.ipeopleid

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            If isEdit = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió la persona que recibió la Factura por " & bp.speoplefullname & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            Else
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'La persona que recibió la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " fue " & bp.speoplefullname & "', 'OK')")
            End If

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            If validaDatosFactura(True, False) = True And isupplierinvoiceid = 0 Then

                isupplierinvoiceid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoiceid) + 1 IS NULL, 1, MAX(isupplierinvoiceid) + 1) AS isupplierinvoiceid FROM supplierinvoices ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

                Dim queriesCreation(21) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0"
                queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid
                queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierid` int(11) NOT NULL, `iinvoicedate` int(11) NOT NULL, `sinvoicetime` varchar(11) CHARACTER SET latin1 NOT NULL, `isupplierinvoicetypeid` int(11) NOT NULL, `ssupplierinvoicefolio` varchar(20) CHARACTER SET latin1 NOT NULL, `sexpensedescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `sexpenselocation` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `dIVApercentage` decimal(20,5) NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`), KEY `isupplierid` (`isupplierid`), KEY `folioid` (`ssupplierinvoicefolio`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Inputs"
                queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs"
                queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `sinputextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoiceinputtotalprice` decimal(20,5) DEFAULT NULL, `dsupplierinvoiceinputunitprice` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `iinputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(9) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Projects"
                queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects"
                queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects ( `isupplierinvoiceid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iprojectid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `projectid` (`iprojectid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Payments"
                queriesCreation(13) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments"
                queriesCreation(14) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments ( `ipaymentid` int(11) NOT NULL, `isupplierinvoiceid` int(11) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`,`isupplierinvoiceid`), KEY `supplierinvoiceid` (`isupplierinvoiceid`), KEY `updateuser` (`supdateusername`), KEY `paymentid` (`ipaymentid`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Assets"
                queriesCreation(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets"
                queriesCreation(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets ( `isupplierinvoiceid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iassetid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `assetid` (`iassetid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                queriesCreation(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Discounts"
                queriesCreation(19) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts"
                queriesCreation(20) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierinvoicediscountid` int(11) NOT NULL, `ssupplierinvoicediscountdescription` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoicediscountpercentage` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`isupplierinvoicediscountid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `supplierinvoicediscountid` (`isupplierinvoicediscountid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                Dim tipofactura As Integer = 0

                Try
                    tipofactura = cmbTipoFactura.SelectedValue
                Catch ex As Exception

                End Try

                If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " VALUES (" & isupplierinvoiceid & ", " & isupplierid & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', " & tipofactura & ", '" & txtFolioFacturaProveedor.Text & "', '" & txtDescripcionFactura.Text & "', '" & txtLugar.Text & "', " & porcentajeIVA / 100 & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                    dgvDetalle.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                    btnEliminarRelacion.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    btnDescuentos.Enabled = True

                End If


            ElseIf validaDatosFactura(True, False) = True And isupplierinvoiceid > 0 Then

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then

                    dgvDetalle.Enabled = True
                    btnNuevoInsumo.Enabled = True
                    btnInsertarInsumo.Enabled = True
                    btnEliminarInsumo.Enabled = True
                    btnEliminarRelacion.Enabled = True
                    btnAgregarPago.Enabled = True
                    btnInsertarPago.Enabled = True
                    btnEliminarPago.Enabled = True
                    btnDescuentos.Enabled = True

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnTipoFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoFactura.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim btf As New BuscaTipoFacturas

        btf.susername = susername
        btf.bactive = bactive
        btf.bonline = bonline
        btf.suserfullname = suserfullname

        btf.suseremail = suseremail
        btf.susersession = susersession
        btf.susermachinename = susermachinename
        btf.suserip = suserip

        btf.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            btf.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        btf.ShowDialog(Me)
        Me.Visible = True

        cmbTipoFactura.DataSource = getSQLQueryAsDataTable(0, "SELECT * FROM supplierinvoicetypes ORDER BY ssupplierinvoicetypedescription")
        cmbTipoFactura.DisplayMember = "ssupplierinvoicetypedescription"
        cmbTipoFactura.ValueMember = "isupplierinvoicetypeid"
        cmbTipoFactura.SelectedIndex = -1

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtFolioFacturaProveedor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFolioFacturaProveedor.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]:;,{}+´¿'¬^`~\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtFolioFacturaProveedor.Text.Contains(arrayCaractProhib(carp)) Then
                txtFolioFacturaProveedor.Text = txtFolioFacturaProveedor.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtFolioFacturaProveedor.Select(txtFolioFacturaProveedor.Text.Length, 0)
        End If

    End Sub


    Private Sub txtFolioFacturaProveedor_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFolioFacturaProveedor.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió el Folio de la Factura de Proveedor por " & txtFolioFacturaProveedor.Text.Replace("--", "").Replace("'", "") & " en la Factura de Proveedor " & isupplierinvoiceid & "', 'OK')")
            End If

        End If

    End Sub


    Private Sub txtDescripcionFactura_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDescripcionFactura.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]:;,{}+´¿'¬^`~\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtDescripcionFactura.Text.Contains(arrayCaractProhib(carp)) Then
                txtDescripcionFactura.Text = txtDescripcionFactura.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtDescripcionFactura.Select(txtDescripcionFactura.Text.Length, 0)
        End If

    End Sub


    Private Sub txtDescripcionFactura_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcionFactura.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET sexpensedescription = '" & txtDescripcionFactura.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió la descripción de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " por " & txtDescripcionFactura.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
            End If

        End If

    End Sub


    Private Sub txtPorcentajeIVA_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIVA.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIVA.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIVA.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIVA.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIVA.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIVA.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Substring(0, lugar) & txtPorcentajeIVA.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIVA.Select(txtPorcentajeIVA.Text.Length, 0)
        End If

        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Trim

    End Sub


    Private Sub txtPorcentajeIVA_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPorcentajeIVA.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim porcentajeIVA As Double = 0.0

        Try
            porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
        Catch ex As Exception

        End Try

        If isupplierinvoiceid > 0 And executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET dIVApercentage = " & porcentajeIVA / 100 & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió el porcentaje de IVA de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " a " & porcentajeIVA / 100 & "', 'OK')")

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

        Else

            Exit Sub

        End If

    End Sub


    Private Sub txtLugar_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLugar.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;.,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtLugar.Text.Contains(arrayCaractProhib(carp)) Then
                txtLugar.Text = txtLugar.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtLugar.Select(txtLugar.Text.Length, 0)
        End If

        txtLugar.Text = txtLugar.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtLugar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLugar.TextChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If isEdit = True Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET sexpenselocation = '" & txtLugar.Text.Replace("--", "").Replace("'", "") & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cambió el Lugar de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " a " & txtLugar.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
            End If

        End If

    End Sub


    Private Sub dgvDetalle_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalle.CellClick

        Try

            If dgvDetalle.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvDetalle.Rows(e.RowIndex).Cells(1).Value())
            sselectedinputdescription = dgvDetalle.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""

        End Try

    End Sub


    Private Sub dgvDetalle_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalle.CellContentClick

        Try

            If dgvDetalle.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvDetalle.Rows(e.RowIndex).Cells(1).Value())
            sselectedinputdescription = dgvDetalle.Rows(e.RowIndex).Cells(2).Value

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""

        End Try

    End Sub


    Private Sub dgvDetalle_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDetalle.SelectionChanged

        Try


            If dgvDetalle.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedinputid = CInt(dgvDetalle.CurrentRow.Cells(1).Value())
            sselectedinputdescription = dgvDetalle.CurrentRow.Cells(2).Value

        Catch ex As Exception

            iselectedinputid = 0
            sselectedinputdescription = ""

        End Try

    End Sub


    Private Sub dgvDetalle_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalle.CellEndEdit

        Dim querySupplierInvoiceInputs As String
        querySupplierInvoiceInputs = "" & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', sii.sinputextranote AS 'Marca/Observaciones', FORMAT(sii.dinputqty, 2) AS 'Cantidad', i.sinputunit AS 'Unidad', " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(sii.dsupplierinvoiceinputtotalprice, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDetalle, querySupplierInvoiceInputs, False)

        dgvDetalle.Columns(0).Visible = False
        dgvDetalle.Columns(1).Visible = False

        dgvDetalle.Columns(0).ReadOnly = True
        dgvDetalle.Columns(1).ReadOnly = True
        dgvDetalle.Columns(2).ReadOnly = True
        dgvDetalle.Columns(5).ReadOnly = True

        dgvDetalle.Columns(0).Width = 30
        dgvDetalle.Columns(1).Width = 30
        dgvDetalle.Columns(2).Width = 100
        dgvDetalle.Columns(3).Width = 100
        dgvDetalle.Columns(4).Width = 70
        dgvDetalle.Columns(5).Width = 70
        dgvDetalle.Columns(6).Width = 70



    End Sub


    Private Sub dgvDetalle_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalle.CellValueChanged

        If modifyInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()


        Dim dsBusquedaInsumoRepetido As DataSet

        'LAS UNICAS COLUMNAS EDITABLES SON LAs 2, 3, 4, 5 : sinputdescription, sinputextranote, dinputqty, dsupplierinvoiceinputunitprice, dsupplierinvoiceinputtotalprice 

        If e.ColumnIndex = 2 Then   'sinputdescription


            If dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            Else

                Dim bi As New BuscaInsumos

                bi.susername = susername
                bi.bactive = bactive
                bi.bonline = bonline
                bi.suserfullname = suserfullname

                bi.suseremail = suseremail
                bi.susersession = susersession
                bi.susermachinename = susermachinename
                bi.suserip = suserip

                bi.querystring = dgvDetalle.CurrentCell.EditedFormattedValue

                bi.IsEdit = False

                If Me.WindowState = FormWindowState.Maximized Then
                    bi.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                bi.ShowDialog(Me)
                Me.Visible = True

                If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                    dsBusquedaInsumoRepetido = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & bi.iinputid)

                    If dsBusquedaInsumoRepetido.Tables(0).Rows.Count > 0 Then

                        MsgBox("Ya tienes a ese Insumo en la Factura. Mejor intenta cambiando la cantidad del Insumo", MsgBoxStyle.OkOnly, "Insumo Repetido")
                        Exit Sub

                    End If


                    If MsgBox("¿Estás seguro de que deseas reemplazar el Insumo " & dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Insumo") = MsgBoxResult.Yes Then

                        Dim cantidaddeinsumo As Double = 1
                        Dim preciototalinsumo As Double = 0.0
                        Dim preciounitarioinsumo As Double = 0.0

                        Try

                            cantidaddeinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(4).Value)
                            preciounitarioinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(6).Value)
                            preciototalinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(7).Value)

                        Catch ex As Exception

                        End Try

                        Dim queriesReplace(2) As String

                        queriesReplace(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid

                        queriesReplace(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs VALUES ( " & isupplierinvoiceid & ", " & bi.iinputid & ", " & cantidaddeinsumo & ", " & preciototalinsumo & ", " & preciounitarioinsumo & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')"

                        If executeTransactedSQLCommand(0, queriesReplace) = True Then
                            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Reemplazo el Insumo anterior (" & iselectedinputid & ") por el Insumo (" & bi.iinputid & ")" & bi.sinputdescription & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                        End If

                    Else

                        'Si cancela el reemplazo de insumo
                        'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                    End If

                Else

                    'Si cancela el reemplazo de insumo
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                End If

            End If


        ElseIf e.ColumnIndex = 3 Then 'sinputextranote


            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs SET sinputextranote = '" & dgvDetalle.Rows(e.RowIndex).Cells(3).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Actualizo las Observaciones del Insumo " & iselectedinputid & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            End If


        ElseIf e.ColumnIndex = 4 Then 'dinputqty


            If dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            ElseIf dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            Else

                Dim cantidaddeinsumo As Double = 1
                Dim preciounitarioinsumo As Double = 0.0

                Try

                    cantidaddeinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(4).Value)
                    preciounitarioinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(6).Value)

                Catch ex As Exception

                End Try

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs SET dinputqty = " & cantidaddeinsumo & ", dsupplierinvoiceinputtotalprice = " & (preciounitarioinsumo * cantidaddeinsumo) & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Actualizo la cantidad del Insumo " & iselectedinputid & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                End If

            End If


        ElseIf e.ColumnIndex = 6 Then 'dsupplierinvoiceinputunitprice


            If dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            ElseIf dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            Else

                Dim cantidaddeinsumo As Double = 1
                Dim preciounitarioinsumo As Double = 0.0

                Try

                    cantidaddeinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(4).Value)
                    preciounitarioinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(6).Value)

                Catch ex As Exception

                End Try

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs SET dsupplierinvoiceinputunitprice = " & preciounitarioinsumo & ", dsupplierinvoiceinputtotalprice = " & (cantidaddeinsumo * preciounitarioinsumo) & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Actualizo los precios del Insumo " & iselectedinputid & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                End If

            End If


        ElseIf e.ColumnIndex = 7 Then 'dsupplierinvoiceinputtotalprice 


            If dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            ElseIf dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & iselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else
                    'dgvDetalle.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription
                End If

            Else

                Dim cantidaddeinsumo As Double = 1
                Dim preciototalinsumo As Double = 0.0

                Try

                    cantidaddeinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(4).Value)
                    preciototalinsumo = CDbl(dgvDetalle.Rows(e.RowIndex).Cells(7).Value)

                Catch ex As Exception

                End Try

                If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs SET dsupplierinvoiceinputtotalprice = " & preciototalinsumo & ", dsupplierinvoiceinputunitprice = " & (preciototalinsumo / cantidaddeinsumo) & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputid) = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Actualizo los precios del Insumo " & iselectedinputid & " en la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
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


    Private Sub dgvDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDetalle.Click

        Dim fecha As Integer = 0
        Dim hora As String = "00:00:00"

        fecha = getMySQLDate()
        hora = getAppTime()

        Dim porcentajeIVA As Double = 0.0

        Try
            porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
        Catch ex As Exception

        End Try

        If validaDatosFactura(True, False) = True And isupplierinvoiceid = 0 Then

            isupplierinvoiceid = getSQLQueryAsInteger(0, "SELECT IF(MAX(isupplierinvoiceid) + 1 IS NULL, 1, MAX(isupplierinvoiceid) + 1) AS isupplierinvoiceid FROM supplierinvoices ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim queriesCreation(21) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0"
            queriesCreation(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid
            queriesCreation(2) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierid` int(11) NOT NULL, `iinvoicedate` int(11) NOT NULL, `sinvoicetime` varchar(11) CHARACTER SET latin1 NOT NULL, `isupplierinvoicetypeid` int(11) NOT NULL, `ssupplierinvoicefolio` varchar(20) CHARACTER SET latin1 NOT NULL, `sexpensedescription` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `sexpenselocation` varchar(500) CHARACTER SET latin1 DEFAULT NULL, `dIVApercentage` decimal(20,5) NOT NULL, `ipeopleid` int(11) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`), KEY `isupplierid` (`isupplierid`), KEY `folioid` (`ssupplierinvoicefolio`), KEY `peopleid` (`ipeopleid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Inputs"
            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `sinputextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoiceinputtotalprice` decimal(20,5) DEFAULT NULL, `dsupplierinvoiceinputunitprice` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `iinputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(9) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Projects"
            queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects"
            queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects ( `isupplierinvoiceid` int(11) NOT NULL, `iprojectid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iprojectid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `projectid` (`iprojectid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Payments"
            queriesCreation(13) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments"
            queriesCreation(14) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments ( `ipaymentid` int(11) NOT NULL, `isupplierinvoiceid` int(11) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ipaymentid`,`isupplierinvoiceid`), KEY `supplierinvoiceid` (`isupplierinvoiceid`), KEY `updateuser` (`supdateusername`), KEY `paymentid` (`ipaymentid`) USING BTREE) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Assets"
            queriesCreation(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets"
            queriesCreation(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets ( `isupplierinvoiceid` int(11) NOT NULL, `iassetid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputqty` decimal(20,5) NOT NULL, `ssupplierinvoiceextranote` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`iassetid`,`iinputid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `assetid` (`iassetid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice0Discounts"
            queriesCreation(19) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts"
            queriesCreation(20) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts ( `isupplierinvoiceid` int(11) NOT NULL AUTO_INCREMENT, `isupplierinvoicediscountid` int(11) NOT NULL, `ssupplierinvoicediscountdescription` varchar(500) COLLATE latin1_spanish_ci NOT NULL, `dsupplierinvoicediscountpercentage` decimal(20,5) DEFAULT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`isupplierinvoiceid`,`isupplierinvoicediscountid`), KEY `supplierinvoice` (`isupplierinvoiceid`), KEY `supplierinvoicediscountid` (`isupplierinvoicediscountid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            Dim tipofactura As Integer = 0

            Try
                tipofactura = cmbTipoFactura.SelectedValue
            Catch ex As Exception

            End Try

            If executeTransactedSQLCommand(0, queriesCreation) = True And executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " VALUES (" & isupplierinvoiceid & ", " & isupplierid & ", " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', " & tipofactura & ", '" & txtFolioFacturaProveedor.Text & "', '" & txtDescripcionFactura.Text & "', '" & txtLugar.Text & "', " & porcentajeIVA / 100 & ", " & ipeopleid & ", " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                dgvDetalle.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
                btnEliminarRelacion.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                btnDescuentos.Enabled = True

            End If


        ElseIf validaDatosFactura(True, False) = True And isupplierinvoiceid > 0 Then

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid) Then

                dgvDetalle.Enabled = True
                btnNuevoInsumo.Enabled = True
                btnInsertarInsumo.Enabled = True
                btnEliminarInsumo.Enabled = True
                btnEliminarRelacion.Enabled = True
                btnAgregarPago.Enabled = True
                btnInsertarPago.Enabled = True
                btnEliminarPago.Enabled = True
                btnDescuentos.Enabled = True

            End If

        End If

    End Sub


    Private Sub dgvDetalle_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvDetalle.EditingControlShowing

        'If dgvDetalle.CurrentCell.ColumnIndex = 2 Then

        '    txtInsumoDgvDetalle = CType(e.Control, TextBox)
        '    txtInsumoDgvDetalle_OldText = txtInsumoDgvDetalle.Text

        'ElseIf dgvDetalle.CurrentCell.ColumnIndex = 3 Then

        '    txtNotasDgvDetalle = CType(e.Control, TextBox)
        '    txtNotasDgvDetalle_OldText = txtNotasDgvDetalle.Text

        'Else
        If dgvDetalle.CurrentCell.ColumnIndex = 4 Then

            txtCantidad1DgvDetalle = CType(e.Control, TextBox)
            txtCantidad1DgvDetalle_OldText = txtCantidad1DgvDetalle.Text

        ElseIf dgvDetalle.CurrentCell.ColumnIndex = 6 Then

            txtCantidad2DgvDetalle = CType(e.Control, TextBox)
            txtCantidad2DgvDetalle_OldText = txtCantidad2DgvDetalle.Text

        ElseIf dgvDetalle.CurrentCell.ColumnIndex = 7 Then

            txtCantidad3DgvDetalle = CType(e.Control, TextBox)
            txtCantidad3DgvDetalle_OldText = txtCantidad3DgvDetalle.Text

        Else

            txtInsumoDgvDetalle = Nothing
            txtInsumoDgvDetalle_OldText = Nothing
            txtNotasDgvDetalle = Nothing
            txtNotasDgvDetalle_OldText = Nothing
            txtCantidad1DgvDetalle = Nothing
            txtCantidad1DgvDetalle_OldText = Nothing
            txtCantidad2DgvDetalle = Nothing
            txtCantidad2DgvDetalle_OldText = Nothing
            txtCantidad3DgvDetalle = Nothing
            txtCantidad3DgvDetalle_OldText = Nothing

        End If

    End Sub


    Private Sub txtInsumoDgvDetalle_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInsumoDgvDetalle.KeyUp

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For cp1 = 0 To arrayForbidden1.Length - 1

            If txtInsumoDgvDetalle.Text.Contains(arrayForbidden1(cp1)) Then
                txtInsumoDgvDetalle.Text = txtInsumoDgvDetalle.Text.Replace(arrayForbidden1(cp1), "")
            End If

        Next cp1

        txtInsumoDgvDetalle.Text = txtInsumoDgvDetalle.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtNotasDgvDetalle_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNotasDgvDetalle.KeyUp

        Dim strForbidden2 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

        For cp2 = 0 To arrayForbidden2.Length - 1

            If txtNotasDgvDetalle.Text.Contains(arrayForbidden2(cp2)) Then
                txtNotasDgvDetalle.Text = txtNotasDgvDetalle.Text.Replace(arrayForbidden2(cp2), "")
            End If

        Next cp2

        txtNotasDgvDetalle.Text = txtNotasDgvDetalle.Text.Replace("--", "").Replace("'", "")

    End Sub


    Private Sub txtCantidad1DgvDetalle_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidad1DgvDetalle.KeyUp

        If Not IsNumeric(txtCantidad1DgvDetalle.Text) Then

            Dim strForbidden3 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden3 As Char() = strForbidden3.ToCharArray

            For cp3 = 0 To arrayForbidden3.Length - 1

                If txtCantidad1DgvDetalle.Text.Contains(arrayForbidden3(cp3)) Then
                    txtCantidad1DgvDetalle.Text = txtCantidad1DgvDetalle.Text.Replace(arrayForbidden3(cp3), "")
                End If

            Next cp3

            If txtCantidad1DgvDetalle.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidad1DgvDetalle.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidad1DgvDetalle.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidad1DgvDetalle.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidad1DgvDetalle.Text = txtCantidad1DgvDetalle.Text.Substring(0, lugar) & txtCantidad1DgvDetalle.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidad1DgvDetalle.Text = txtCantidad1DgvDetalle.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidad1DgvDetalle.Text = txtCantidad1DgvDetalle.Text.Replace("--", "").Replace("'", "")
            txtCantidad1DgvDetalle.Text = txtCantidad1DgvDetalle.Text.Trim

        Else
            txtCantidad1DgvDetalle_OldText = txtCantidad1DgvDetalle.Text
        End If

    End Sub


    Private Sub txtCantidad2DgvDetalle_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidad2DgvDetalle.KeyUp

        If Not IsNumeric(txtCantidad2DgvDetalle.Text) Then

            Dim strForbidden4 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden4 As Char() = strForbidden4.ToCharArray

            For cp4 = 0 To arrayForbidden4.Length - 1

                If txtCantidad2DgvDetalle.Text.Contains(arrayForbidden4(cp4)) Then
                    txtCantidad2DgvDetalle.Text = txtCantidad2DgvDetalle.Text.Replace(arrayForbidden4(cp4), "")
                End If

            Next cp4

            If txtCantidad2DgvDetalle.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidad2DgvDetalle.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidad2DgvDetalle.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidad2DgvDetalle.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidad2DgvDetalle.Text = txtCantidad2DgvDetalle.Text.Substring(0, lugar) & txtCantidad2DgvDetalle.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidad2DgvDetalle.Text = txtCantidad2DgvDetalle.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidad2DgvDetalle.Text = txtCantidad2DgvDetalle.Text.Replace("--", "").Replace("'", "")
            txtCantidad2DgvDetalle.Text = txtCantidad2DgvDetalle.Text.Trim

        Else
            txtCantidad2DgvDetalle_OldText = txtCantidad2DgvDetalle.Text
        End If

    End Sub


    Private Sub txtCantidad3DgvDetalle_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidad3DgvDetalle.KeyUp

        If Not IsNumeric(txtCantidad3DgvDetalle.Text) Then

            Dim strForbidden5 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden5 As Char() = strForbidden5.ToCharArray

            For cp5 = 0 To arrayForbidden5.Length - 1

                If txtCantidad3DgvDetalle.Text.Contains(arrayForbidden5(cp5)) Then
                    txtCantidad3DgvDetalle.Text = txtCantidad3DgvDetalle.Text.Replace(arrayForbidden5(cp5), "")
                End If

            Next cp5

            If txtCantidad3DgvDetalle.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidad3DgvDetalle.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidad3DgvDetalle.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidad3DgvDetalle.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidad3DgvDetalle.Text = txtCantidad3DgvDetalle.Text.Substring(0, lugar) & txtCantidad3DgvDetalle.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidad3DgvDetalle.Text = txtCantidad3DgvDetalle.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidad3DgvDetalle.Text = txtCantidad3DgvDetalle.Text.Replace("--", "").Replace("'", "")
            txtCantidad3DgvDetalle.Text = txtCantidad3DgvDetalle.Text.Trim

        Else
            txtCantidad3DgvDetalle_OldText = txtCantidad3DgvDetalle.Text
        End If

    End Sub


    Private Sub dgvDetalle_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvDetalle.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteInputPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedinputid As Integer = 0
                Try
                    tmpselectedinputid = CInt(dgvDetalle.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & tmpselectedinputid) = True Then

                    Dim fecha As Integer = 0
                    Dim hora As String = ""

                    fecha = getMySQLDate()
                    hora = getAppTime()

                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & tmpselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

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


    Private Sub dgvDetalle_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvDetalle.UserAddedRow

        If insertInputPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaInsumos

        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname

        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.querystring = dgvDetalle.CurrentCell.EditedFormattedValue

        bi.IsEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs VALUES ( " & isupplierinvoiceid & ", " & bi.iinputid & ", 1, '', 0.00, 0.00, " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')") = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Insumo " & bi.iinputid & " : " & bi.sinputdescription & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            End If

        End If

        'dgvDetalle.EndEdit()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoInsumo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ai As New AgregarInsumo

        ai.susername = susername
        ai.bactive = bactive
        ai.bonline = bonline
        ai.suserfullname = suserfullname
        ai.suseremail = suseremail
        ai.susersession = susersession
        ai.susermachinename = susermachinename
        ai.suserip = suserip

        ai.IsEdit = False
        ai.IsHistoric = False

        ai.IsBase = True
        ai.IsModel = False
        ai.IsRecover = isRecover

        'Drop-Create ALL base tables

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        ai.iprojectid = baseid

        Dim queriesCreation(14) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts"
        queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts" & " ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards"
        queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs"
        queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs"
        queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices"
        queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices" & " ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber"
        queriesCreation(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)


        'Insert Info from Base

        Dim queriesInsert(7) As String

        queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " SELECT * FROM base WHERE ibaseid = " & baseid
        queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts SELECT * FROM baseindirectcosts WHERE ibaseid = " & baseid
        queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards SELECT * FROM basecards WHERE ibaseid = " & baseid
        queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs SELECT * FROM basecardinputs WHERE ibaseid = " & baseid
        queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs SELECT * FROM basecardcompoundinputs WHERE ibaseid = " & baseid
        queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices SELECT * FROM baseprices WHERE ibaseid = " & baseid
        queriesInsert(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber SELECT * FROM basetimber WHERE ibaseid = " & baseid

        executeTransactedSQLCommand(0, queriesInsert)

        'Show Add Window

        If Me.WindowState = FormWindowState.Maximized Then
            ai.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False

        ai.ShowDialog(Me)

        If ai.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim lastPrice As Double = 0.0

            lastPrice = getSQLQueryAsDouble(0, "SELECT IF(dinputfinalprice IS NULL, 0.0, dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices WHERE ibaseid = " & baseid & " AND iinputid = " & ai.iinputid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs VALUES ( " & isupplierinvoiceid & ", " & ai.iinputid & ", 1, '', " & lastPrice & ", " & lastPrice & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')") = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Nuevo Insumo " & ai.iinputid & " : " & ai.sinputdescription & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            End If

            Dim queriesSave(29) As String

            queriesSave(0) = "" & _
            "DELETE " & _
            "FROM base " & _
            "WHERE ibaseid = " & baseid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tb WHERE base.ibaseid = tb.ibaseid) "

            queriesSave(1) = "" & _
            "DELETE " & _
            "FROM baseindirectcosts " & _
            "WHERE ibaseid = " & baseid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

            queriesSave(2) = "" & _
            "DELETE " & _
            "FROM basecards " & _
            "WHERE ibaseid = " & baseid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

            'queriesSave(3) = "" & _
            '"DELETE " & _
            '"FROM basecards " & _
            '"WHERE ibaseid = " & baseid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

            'queriesSave(4) = "" & _
            '"DELETE " & _
            '"FROM projectcards " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & baseid & " AND projectcards.icardid = bc.icardid) "

            'queriesSave(5) = "" & _
            '"DELETE " & _
            '"FROM modelcards " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE bc.ibaseid = " & baseid & " AND modelcards.icardid = bc.icardid) "

            queriesSave(6) = "" & _
            "DELETE " & _
            "FROM basecardinputs " & _
            "WHERE ibaseid = " & baseid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

            'queriesSave(7) = "" & _
            '"DELETE " & _
            '"FROM projectcardinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & baseid & " AND projectcardinputs.icardid = bci.icardid AND projectcardinputs.iinputid = bci.iinputid) "

            'queriesSave(8) = "" & _
            '"DELETE " & _
            '"FROM modelcardinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE bci.ibaseid = " & baseid & " AND modelcardinputs.icardid = bci.icardid AND modelcardinputs.iinputid = bci.iinputid) "

            queriesSave(9) = "" & _
            "DELETE " & _
            "FROM basecardcompoundinputs " & _
            "WHERE ibaseid = " & baseid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

            'queriesSave(10) = "" & _
            '"DELETE " & _
            '"FROM projectcardcompoundinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & baseid & " AND projectcardcompoundinputs.icardid = bcci.icardid AND projectcardcompoundinputs.iinputid = bcci.iinputid AND projectcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

            'queriesSave(11) = "" & _
            '"DELETE " & _
            '"FROM modelcardcompoundinputs " & _
            '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE bcci.ibaseid = " & baseid & " AND modelcardcompoundinputs.icardid = bcci.icardid AND modelcardcompoundinputs.iinputid = bcci.iinputid AND modelcardcompoundinputs.icompoundinputid = bcci.icompoundinputid) "

            ' '' '' '' '' ''queriesSave(12) = "" & _
            ' '' '' '' '' ''"DELETE " & _
            ' '' '' '' '' ''"FROM baseprices " & _
            ' '' '' '' '' ''"WHERE ibaseid = " & baseid & " AND " & _
            ' '' '' '' '' ''"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

            '' '' '' '' '' ''queriesSave(13) = "" & _
            '' '' '' '' '' ''"DELETE " & _
            '' '' '' '' '' ''"FROM basetimber " & _
            '' '' '' '' '' ''"WHERE ibaseid = " & baseid & " AND " & _
            '' '' '' '' '' ''"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

            queriesSave(14) = "" & _
            "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queriesSave(15) = "" & _
            "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

            queriesSave(16) = "" & _
            "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

            queriesSave(17) = "" & _
            "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

            queriesSave(18) = "" & _
            "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

            queriesSave(19) = "" & _
            "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

            queriesSave(20) = "" & _
            "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

            queriesSave(21) = "" & _
            "INSERT INTO base " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tb " & _
            "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

            queriesSave(22) = "" & _
            "INSERT INTO baseindirectcosts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tbic " & _
            "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

            queriesSave(23) = "" & _
            "INSERT INTO basecards " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc " & _
            "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

            queriesSave(24) = "" & _
            "INSERT INTO basecardinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tbci " & _
            "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

            queriesSave(25) = "" & _
            "INSERT INTO basecardcompoundinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tbcci " & _
            "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

            queriesSave(26) = "" & _
            "INSERT INTO baseprices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp " & _
            "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

            queriesSave(27) = "" & _
            "INSERT INTO basetimber " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tbt " & _
            "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") "

            queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Materiales directamente (Insumo ID: " & ai.iinputid & "', 'OK')"

            If executeTransactedSQLCommand(0, queriesSave) = True Then
                MsgBox("Insumo guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If


        End If

        Me.Visible = True


        'Drop ALL base tables

        Dim queriesDelete(9) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards"
        queriesDelete(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardsAux"
        queriesDelete(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs"
        queriesDelete(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs"
        queriesDelete(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices"
        queriesDelete(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber"

        executeTransactedSQLCommand(0, queriesDelete)


        Dim querySupplierInvoiceInputs As String
        querySupplierInvoiceInputs = "" & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', sii.sinputextranote AS 'Marca/Observaciones', FORMAT(sii.dinputqty, 2) AS 'Cantidad', i.sinputunit AS 'Unidad', " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(sii.dsupplierinvoiceinputtotalprice, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDetalle, querySupplierInvoiceInputs, False)

        dgvDetalle.Columns(0).Visible = False
        dgvDetalle.Columns(1).Visible = False

        dgvDetalle.Columns(0).ReadOnly = True
        dgvDetalle.Columns(1).ReadOnly = True
        dgvDetalle.Columns(2).ReadOnly = True
        dgvDetalle.Columns(5).ReadOnly = True

        dgvDetalle.Columns(0).Width = 30
        dgvDetalle.Columns(1).Width = 30
        dgvDetalle.Columns(2).Width = 100
        dgvDetalle.Columns(3).Width = 100
        dgvDetalle.Columns(4).Width = 70
        dgvDetalle.Columns(5).Width = 70
        dgvDetalle.Columns(6).Width = 70


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


    Private Sub btnInsertarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarInsumo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaInsumos
        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname
        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.IsBase = True
        bi.IsModel = False
        bi.IsHistoric = False

        bi.restrictCompounds = True
        bi.mayNeedBaseTablesCreation = True

        bi.IsEdit = False

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False

        bi.ShowDialog(Me)

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim lastPrice As Double = 0.0

            'lastPrice = getSQLQueryAsDouble(0, "SELECT IF(dinputfinalprice IS NULL, 0.0, dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
            lastPrice = getSQLQueryAsDouble(0, "SELECT IF(dinputfinalprice IS NULL, 0.0, dinputfinalprice) AS dinputfinalprice FROM baseprices WHERE ibaseid = " & baseid & " AND iinputid = " & bi.iinputid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs VALUES ( " & isupplierinvoiceid & ", " & bi.iinputid & ", 1, '', " & lastPrice & ", " & lastPrice & ", " & fecha & ", '" & hora & "', '" & susername & "')") = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Insumo " & bi.iinputid & " : " & bi.sinputdescription & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            End If

        End If

        Me.Visible = True


        Dim querySupplierInvoiceInputs As String
        querySupplierInvoiceInputs = "" & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', sii.sinputextranote AS 'Marca/Observaciones', FORMAT(sii.dinputqty, 2) AS 'Cantidad', i.sinputunit AS 'Unidad', " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(sii.dsupplierinvoiceinputtotalprice, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvDetalle, querySupplierInvoiceInputs, False)

        dgvDetalle.Columns(0).Visible = False
        dgvDetalle.Columns(1).Visible = False

        dgvDetalle.Columns(0).ReadOnly = True
        dgvDetalle.Columns(1).ReadOnly = True
        dgvDetalle.Columns(2).ReadOnly = True
        dgvDetalle.Columns(5).ReadOnly = True

        dgvDetalle.Columns(0).Width = 30
        dgvDetalle.Columns(1).Width = 30
        dgvDetalle.Columns(2).Width = 100
        dgvDetalle.Columns(3).Width = 100
        dgvDetalle.Columns(4).Width = 70
        dgvDetalle.Columns(5).Width = 70
        dgvDetalle.Columns(6).Width = 70


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


    Private Sub btnEliminarInsumo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarInsumo.Click

        If MsgBox("¿Está seguro que deseas quitar este Insumo de la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Insumo de la Factura") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedinputid As Integer = 0
            Try
                tmpselectedinputid = CInt(dgvDetalle.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try


            If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & tmpselectedinputid) = True Then

                Dim fecha As Integer = 0
                Dim hora As String = ""

                fecha = getMySQLDate()
                hora = getAppTime()

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Elimino el Insumo " & tmpselectedinputid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

            End If


            Dim querySupplierInvoiceInputs As String
            querySupplierInvoiceInputs = "" & _
            "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', sii.sinputextranote AS 'Marca/Observaciones', FORMAT(sii.dinputqty, 2) AS 'Cantidad', i.sinputunit AS 'Unidad', " & _
            "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(sii.dsupplierinvoiceinputtotalprice, 2) AS 'Importe Total' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "WHERE " & _
            "si.isupplierinvoiceid = " & isupplierinvoiceid

            setDataGridView(dgvDetalle, querySupplierInvoiceInputs, False)

            dgvDetalle.Columns(0).Visible = False
            dgvDetalle.Columns(1).Visible = False

            dgvDetalle.Columns(0).ReadOnly = True
            dgvDetalle.Columns(1).ReadOnly = True
            dgvDetalle.Columns(2).ReadOnly = True
            dgvDetalle.Columns(5).ReadOnly = True

            dgvDetalle.Columns(0).Width = 30
            dgvDetalle.Columns(1).Width = 30
            dgvDetalle.Columns(2).Width = 100
            dgvDetalle.Columns(3).Width = 100
            dgvDetalle.Columns(4).Width = 70
            dgvDetalle.Columns(5).Width = 70
            dgvDetalle.Columns(6).Width = 70


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


    Private Sub btnDescuentos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDescuentos.Click

        Dim adaf As New AgregarDescuentoAFacturaProveedor

        adaf.susername = susername
        adaf.bactive = bactive
        adaf.bonline = bonline
        adaf.suserfullname = suserfullname
        adaf.suseremail = suseremail
        adaf.susersession = susersession
        adaf.susermachinename = susermachinename
        adaf.suserip = suserip

        adaf.isEdit = True

        adaf.isupplierinvoiceid = isupplierinvoiceid

        If Me.WindowState = FormWindowState.Maximized Then
            adaf.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        adaf.ShowDialog(Me)
        Me.Visible = True

        If adaf.DialogResult = Windows.Forms.DialogResult.OK Then

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

        End If

    End Sub


    Private Function validaDatosFactura(ByVal silent As Boolean, ByVal save As Boolean) As Boolean

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos2 As String = "|°!#$%&/()=?¡*¨[]_:;-{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos3 As String = "|°!$%&/()=?¡*¨[]_:;{}+´¿'¬^`~@\<>"
        Dim strcaracteresprohibidos4 As String = "|°#$%&/()=¡*¨[]_:;,-{}+´¿'¬^`~@\<>"

        txtProveedor.Text = txtProveedor.Text.Trim.Trim(strcaracteresprohibidos.ToCharArray).Replace("--", "").Replace("'", "")
        txtFolioFacturaProveedor.Text = txtFolioFacturaProveedor.Text.Trim.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")
        txtDescripcionFactura.Text = txtDescripcionFactura.Text.Trim.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")
        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Trim.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")
        txtLugar.Text = txtLugar.Text.Trim.Trim(strcaracteresprohibidos3.ToCharArray).Replace("--", "").Replace("'", "")
        txtPersona.Text = txtPersona.Text.Trim.Trim(strcaracteresprohibidos2.ToCharArray).Replace("--", "").Replace("'", "")

        If txtProveedor.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías indicarme de que Proveedor es la Factura?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtFolioFacturaProveedor.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner el Folio de la Factura del Proveedor?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If isEdit = False And save = True And getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM supplierinvoices WHERE isupplierid = " & isupplierid & " AND ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "'") > 0 Then
            If silent = False Then
                MsgBox("Ya está registrada una Factura de este Proveedor con ese folio. Verifica si ya la registraron.", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtPorcentajeIVA.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías poner el porcentaje de IVA que utiliza la Factura?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If txtLugar.Text = "" Then
            If silent = False Then
                MsgBox("¿Podrías indicarme en que Lugar se originó el gasto que representa esta Factura?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If
            Return False
        End If

        If save = True Then

            If txtPersona.Text = "" Then
                If silent = False Then
                    MsgBox("¿Podrías poner el Nombre de la Persona que recibió la Factura?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                Return False
            End If

        End If

        Return True

    End Function


    Private Function validaInsumosFactura(ByVal silent As Boolean) As Boolean

        Dim subtotalfactura As Double = 0.0
        subtotalfactura = getSQLQueryAsDouble(0, "SELECT IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        If subtotalfactura <= 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías poner algun Insumo en la Factura? Está vacía...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        Return True

    End Function


    Private Function validaFacturaCompleta(ByVal silent As Boolean) As Boolean

        If validaDatosFactura(silent, True) = False Or validaInsumosFactura(silent) = False Then
            Return False
        Else
            Return True
        End If

    End Function


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelarFactura.Click, btnCancelarPago.Click, btnCancelarRelacion.Click

        'isupplierinvoiceid = 0
        'isupplierid = 0
        'ipeopleid = 0

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnGuardarDatosFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarDatosFactura.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaFacturaCompleta(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(12) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(10) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(11) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If


            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
                btnRevisiones.Enabled = True
                btnRevisionesRelaciones.Enabled = True
                btnRevisionesPagos.Enabled = True

            Else

                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnGuardarDatosFacturaYCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarDatosFacturaYCerrar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaFacturaCompleta(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(12) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(10) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(11) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If


            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & " para Cerrar', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnPaso2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaso2.Click

        tcFacturas.SelectedTab = tpRelaciones

    End Sub


    Private Sub tcFacturas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcFacturas.SelectedIndexChanged

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaDatosFactura(True, False) = True And (tcFacturas.SelectedTab Is tpFactura) = False Then

            'Continue


            If validaRelaciones(True) = True And (tcFacturas.SelectedTab Is tpFactura) = False And (tcFacturas.SelectedTab Is tpRelaciones) = False Then
                'Continue
            Else
                tcFacturas.SelectedTab = tpRelaciones
            End If


            If tcFacturas.SelectedTab Is tpFactura Then


                Dim querySupplierInvoiceInputs As String
                querySupplierInvoiceInputs = "" & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', sii.sinputextranote AS 'Marca/Observaciones', FORMAT(sii.dinputqty, 2) AS 'Cantidad', i.sinputunit AS 'Unidad', " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(sii.dsupplierinvoiceinputtotalprice, 2) AS 'Importe Total' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid

                setDataGridView(dgvDetalle, querySupplierInvoiceInputs, False)

                dgvDetalle.Columns(0).Visible = False
                dgvDetalle.Columns(1).Visible = False

                dgvDetalle.Columns(0).ReadOnly = True
                dgvDetalle.Columns(1).ReadOnly = True
                dgvDetalle.Columns(2).ReadOnly = True
                dgvDetalle.Columns(5).ReadOnly = True

                dgvDetalle.Columns(0).Width = 30
                dgvDetalle.Columns(1).Width = 30
                dgvDetalle.Columns(2).Width = 100
                dgvDetalle.Columns(3).Width = 100
                dgvDetalle.Columns(4).Width = 70
                dgvDetalle.Columns(5).Width = 70
                dgvDetalle.Columns(6).Width = 70


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


            ElseIf tcFacturas.SelectedTab Is tpRelaciones Then


                Dim queryNoRelacionados As String
                queryNoRelacionados = "" & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT((sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty)) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "LEFT JOIN (SELECT isupplierinvoiceid, iprojectid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid  AND i.iinputid = sip.iinputid " & _
                "LEFT JOIN (SELECT isupplierinvoiceid, iassetid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid  AND i.iinputid = sia.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
                "AND sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty) > 0"


                setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

                dgvDetalleSinRelacion.Columns(0).Visible = False
                dgvDetalleSinRelacion.Columns(1).Visible = False

                dgvDetalleSinRelacion.Columns(0).Width = 30
                dgvDetalleSinRelacion.Columns(1).Width = 30
                dgvDetalleSinRelacion.Columns(2).Width = 200
                dgvDetalleSinRelacion.Columns(3).Width = 70
                dgvDetalleSinRelacion.Columns(4).Width = 70
                dgvDetalleSinRelacion.Columns(5).Width = 70


                Dim queryRelacionados As String

                queryRelacionados = "" & _
                "SELECT * FROM ( " & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty), 2) AS Cantidad, " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
                "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
                "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
                "LEFT JOIN assets a ON sia.iassetid = a.iassetid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid AND sia.iinputid = i.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
                "UNION " & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
                "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
                "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
                "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid AND sip.iinputid = i.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
                ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

                setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

                dgvDetalleConRelacion.Columns(0).Visible = False
                dgvDetalleConRelacion.Columns(1).Visible = False
                dgvDetalleConRelacion.Columns(6).Visible = False
                dgvDetalleConRelacion.Columns(8).Visible = False
                dgvDetalleConRelacion.Columns(10).Visible = False
                dgvDetalleConRelacion.Columns(11).Visible = False

                dgvDetalleConRelacion.Columns(0).ReadOnly = True
                dgvDetalleConRelacion.Columns(1).ReadOnly = True
                dgvDetalleConRelacion.Columns(2).ReadOnly = True
                dgvDetalleConRelacion.Columns(3).ReadOnly = False
                dgvDetalleConRelacion.Columns(4).ReadOnly = True
                dgvDetalleConRelacion.Columns(5).ReadOnly = True
                dgvDetalleConRelacion.Columns(6).ReadOnly = True
                dgvDetalleConRelacion.Columns(7).ReadOnly = True
                dgvDetalleConRelacion.Columns(8).ReadOnly = True
                dgvDetalleConRelacion.Columns(9).ReadOnly = True

                dgvDetalleConRelacion.Columns(0).Width = 30
                dgvDetalleConRelacion.Columns(1).Width = 30
                dgvDetalleConRelacion.Columns(2).Width = 200
                dgvDetalleConRelacion.Columns(3).Width = 70
                dgvDetalleConRelacion.Columns(4).Width = 70
                dgvDetalleConRelacion.Columns(5).Width = 70
                dgvDetalleConRelacion.Columns(6).Width = 30
                dgvDetalleConRelacion.Columns(7).Width = 200
                dgvDetalleConRelacion.Columns(8).Width = 30
                dgvDetalleConRelacion.Columns(9).Width = 300


                cmbTipoRelacion.SelectedIndex = 1


            ElseIf tcFacturas.SelectedTab Is tpPagos Then


                Dim querySupplierInvoicePayments As String
                querySupplierInvoicePayments = "" & _
                "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
                "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
                "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
                "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
                "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
                "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

                setDataGridView(dgvPagos, querySupplierInvoicePayments, False)

                dgvPagos.Columns(0).Visible = False
                dgvPagos.Columns(1).Visible = False

                dgvPagos.Columns(0).ReadOnly = True
                dgvPagos.Columns(1).ReadOnly = True
                dgvPagos.Columns(2).ReadOnly = True
                dgvPagos.Columns(3).ReadOnly = True
                dgvPagos.Columns(4).ReadOnly = True
                dgvPagos.Columns(5).ReadOnly = True

                dgvPagos.Columns(0).Width = 30
                dgvPagos.Columns(1).Width = 30
                dgvPagos.Columns(2).Width = 70
                dgvPagos.Columns(3).Width = 70
                dgvPagos.Columns(4).Width = 100
                dgvPagos.Columns(5).Width = 200
                dgvPagos.Columns(6).Width = 200

                txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim dsDescuentos As DataSet
                dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                Dim subtotal As Double = 0.0
                Dim iva As Double = 0.0
                Dim total As Double = 0.0

                subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                If dsDescuentos.Tables(0).Rows.Count > 0 Then

                    For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                        subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                    Next i

                End If

                iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
                total = subtotal + iva

                txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            End If


        Else

            tcFacturas.SelectedTab = tpFactura

        End If

    End Sub


    Private Sub dgvDetalleSinRelacion_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleSinRelacion.CellClick

        Try

            If dgvDetalleSinRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedinputwithoutrelationid = CInt(dgvDetalleSinRelacion.Rows(e.RowIndex).Cells(1).Value())

        Catch ex As Exception

            iselectedinputwithoutrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleSinRelacion_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleSinRelacion.CellContentClick

        Try

            If dgvDetalleSinRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedinputwithoutrelationid = CInt(dgvDetalleSinRelacion.Rows(e.RowIndex).Cells(1).Value())

        Catch ex As Exception

            iselectedinputwithoutrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleSinRelacion_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDetalleSinRelacion.SelectionChanged

        Try

            If dgvDetalleSinRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedinputwithoutrelationid = CInt(dgvDetalleSinRelacion.CurrentRow.Cells(1).Value())

        Catch ex As Exception

            iselectedinputwithoutrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellClick

        Try

            If dgvDetalleConRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedinputwithrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(1).Value())
            iselectedrelationtypeid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(6).Value())
            iselectedrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(8).Value())

        Catch ex As Exception

            iselectedinputwithrelationid = 0
            iselectedrelationtypeid = 0
            iselectedrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellContentClick

        Try

            If dgvDetalleConRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedinputwithrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(1).Value())
            iselectedrelationtypeid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(6).Value())
            iselectedrelationid = CInt(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(8).Value())

        Catch ex As Exception

            iselectedinputwithrelationid = 0
            iselectedrelationtypeid = 0
            iselectedrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDetalleConRelacion.SelectionChanged

        Try

            If dgvDetalleConRelacion.CurrentRow Is Nothing Then
                Exit Sub
            End If

            iselectedinputwithrelationid = CInt(dgvDetalleConRelacion.CurrentRow.Cells(1).Value())
            iselectedrelationtypeid = CInt(dgvDetalleConRelacion.CurrentRow.Cells(6).Value())
            iselectedrelationid = CInt(dgvDetalleConRelacion.CurrentRow.Cells(8).Value())

        Catch ex As Exception

            iselectedinputwithrelationid = 0
            iselectedrelationtypeid = 0
            iselectedrelationid = 0

        End Try

    End Sub


    Private Sub dgvDetalleConRelacion_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvDetalleConRelacion.KeyUp

        If iselectedrelationtypeid = 0 Then
            Exit Sub
        End If

        If e.KeyCode = Keys.Delete Then

            If deleteRelationPermission = False Then
                Exit Sub
            End If

            If iselectedrelationtypeid = 0 Then
                Exit Sub
            End If

            If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Insumo Factura") = MsgBoxResult.Yes Then

                Dim fecha As Integer = 0
                Dim hora As String = ""

                fecha = getMySQLDate()
                hora = getAppTime()

                If iselectedrelationtypeid = 1 Then

                    'Assets
                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iassetid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                Else

                    'Projects
                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iprojectid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Proyecto " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If

                End If

                Dim queryNoRelacionados As String
                queryNoRelacionados = "" & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT((sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty)) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid " & _
                "LEFT JOIN (SELECT isupplierinvoiceid, iprojectid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid  AND i.iinputid = sip.iinputid " & _
                "LEFT JOIN (SELECT isupplierinvoiceid, iassetid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid  AND i.iinputid = sia.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
                "AND sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty) > 0"


                setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

                dgvDetalleSinRelacion.Columns(0).Visible = False
                dgvDetalleSinRelacion.Columns(1).Visible = False

                dgvDetalleSinRelacion.Columns(0).Width = 30
                dgvDetalleSinRelacion.Columns(1).Width = 30
                dgvDetalleSinRelacion.Columns(2).Width = 200
                dgvDetalleSinRelacion.Columns(3).Width = 70
                dgvDetalleSinRelacion.Columns(4).Width = 70
                dgvDetalleSinRelacion.Columns(5).Width = 70


                Dim queryRelacionados As String

                queryRelacionados = "" & _
                "SELECT * FROM ( " & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty), 2) AS Cantidad, " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
                "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
                "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
                "LEFT JOIN assets a ON sia.iassetid = a.iassetid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid AND sia.iinputid = i.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
                "UNION " & _
                "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
                "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
                "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
                "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A' " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
                "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
                "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
                "JOIN inputs i ON sii.iinputid = i.iinputid AND sip.iinputid = i.iinputid " & _
                "WHERE " & _
                "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
                "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
                ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

                setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

                dgvDetalleConRelacion.Columns(0).Visible = False
                dgvDetalleConRelacion.Columns(1).Visible = False
                dgvDetalleConRelacion.Columns(6).Visible = False
                dgvDetalleConRelacion.Columns(8).Visible = False

                dgvDetalleConRelacion.Columns(0).ReadOnly = True
                dgvDetalleConRelacion.Columns(1).ReadOnly = True
                dgvDetalleConRelacion.Columns(2).ReadOnly = True
                dgvDetalleConRelacion.Columns(3).ReadOnly = False
                dgvDetalleConRelacion.Columns(4).ReadOnly = True
                dgvDetalleConRelacion.Columns(5).ReadOnly = True
                dgvDetalleConRelacion.Columns(6).ReadOnly = True
                dgvDetalleConRelacion.Columns(7).ReadOnly = True
                dgvDetalleConRelacion.Columns(8).ReadOnly = True
                dgvDetalleConRelacion.Columns(9).ReadOnly = True

                dgvDetalleConRelacion.Columns(0).Width = 30
                dgvDetalleConRelacion.Columns(1).Width = 30
                dgvDetalleConRelacion.Columns(2).Width = 200
                dgvDetalleConRelacion.Columns(3).Width = 70
                dgvDetalleConRelacion.Columns(4).Width = 70
                dgvDetalleConRelacion.Columns(5).Width = 70
                dgvDetalleConRelacion.Columns(6).Width = 30
                dgvDetalleConRelacion.Columns(7).Width = 200
                dgvDetalleConRelacion.Columns(8).Width = 30
                dgvDetalleConRelacion.Columns(9).Width = 300

            End If

        End If

    End Sub


    Private Sub dgvDetalleConRelacion_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvDetalleConRelacion.EditingControlShowing

        If dgvDetalleConRelacion.CurrentCell.ColumnIndex = 3 Then

            txtCantidadDgvDetalleConRelacion = CType(e.Control, TextBox)
            txtCantidadDgvDetalleConRelacion_OldText = txtCantidadDgvDetalleConRelacion.Text

        Else

            txtCantidadDgvDetalleConRelacion = Nothing
            txtCantidadDgvDetalleConRelacion_OldText = Nothing

        End If

    End Sub


    Private Sub txtCantidadDgvDetalleConRelacion_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvDetalleConRelacion.KeyUp

        If Not IsNumeric(txtCantidadDgvDetalleConRelacion.Text) Then

            Dim strForbidden4 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden4 As Char() = strForbidden4.ToCharArray

            For cp = 0 To arrayForbidden4.Length - 1

                If txtCantidadDgvDetalleConRelacion.Text.Contains(arrayForbidden4(cp)) Then
                    txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Replace(arrayForbidden4(cp), "")
                End If

            Next cp

            If txtCantidadDgvDetalleConRelacion.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvDetalleConRelacion.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvDetalleConRelacion.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvDetalleConRelacion.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Substring(0, lugar) & txtCantidadDgvDetalleConRelacion.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvDetalleConRelacion.Text = txtCantidadDgvDetalleConRelacion.Text.Trim

        Else
            txtCantidadDgvDetalleConRelacion_OldText = txtCantidadDgvDetalleConRelacion.Text
        End If

    End Sub


    Private Sub dgvDetalleConRelacion_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellValueChanged

        If modifyRelationPermission = False Then
            Exit Sub
        End If

        If iselectedrelationtypeid = 0 Then
            Exit Sub
        End If

        Dim fecha As Integer = 0
        Dim hora As String = ""

        fecha = getMySQLDate()
        hora = getAppTime()

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LA UNICA COLUMNA EDITABLE ES LA 3: DQTY

        If e.ColumnIndex = 3 Then

            Try

                If dgvDetalleConRelacion.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                    If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Insumo Factura") = MsgBoxResult.Yes Then

                        If iselectedrelationtypeid = 1 Then

                            'Assets
                            If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iassetid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                            End If

                        Else

                            'Projects
                            If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iprojectid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Proyecto " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                            End If

                        End If

                    Else

                        'dgvDetalleConRelacion.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedinputdescription

                    End If

                Else

                    Dim cantidadrelacionada As Double = 0.0
                    Dim cantidadUsadaAparteDeEsta As Double = 0

                    Try

                        cantidadrelacionada = CDbl(dgvDetalleConRelacion.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

                    Catch ex As Exception

                    End Try

                    If iselectedrelationtypeid = 1 Then
                        cantidadUsadaAparteDeEsta = getSQLQueryAsDouble(0, "SELECT IF(SUM(dinputqty) IS NULL, 0, SUM(dinputqty)) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputwithrelationid & " AND iassetid <> " & iselectedrelationid) + getSQLQueryAsDouble(0, "SELECT IF(SUM(dinputqty) IS NULL, 0, SUM(dinputqty)) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputwithrelationid)
                    Else
                        cantidadUsadaAparteDeEsta = getSQLQueryAsDouble(0, "SELECT IF(SUM(dinputqty) IS NULL, 0, SUM(dinputqty)) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputwithrelationid) + getSQLQueryAsDouble(0, "SELECT IF(SUM(dinputqty) IS NULL, 0, SUM(dinputqty)) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputwithrelationid & " AND iprojectid <> " & iselectedrelationid)
                    End If

                    If (cantidadrelacionada + cantidadUsadaAparteDeEsta) > getSQLQueryAsDouble(0, "SELECT dinputqty FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iinputid = " & iselectedinputwithrelationid) Then
                        Exit Sub
                    End If

                    If iselectedrelationtypeid = 1 Then

                        'Assets
                        If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets SET dinputqty = " & cantidadrelacionada & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iassetid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                        End If

                    Else

                        'Projects
                        If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects SET dinputqty = " & cantidadrelacionada & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iprojectid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Proyecto " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                        End If

                    End If

                End If

            Catch ex As Exception

                If iselectedrelationtypeid = 1 Then

                    'Assets
                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iassetid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If
                    MsgBox("Ha ocurrido un error. No es grave. Intenta de nuevo. Si intentas poner un numero como .5, esta vez, intenta ponerlo con el 0 antes del punto, como 0.5", MsgBoxStyle.OkOnly, "Error de captura")

                ElseIf iselectedrelationtypeid = 2 Then

                    'Projects
                    If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iprojectid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Proyecto " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    End If
                    MsgBox("Ha ocurrido un error. No es grave. Intenta de nuevo. Si intentas poner un numero como .5, esta vez, intenta ponerlo con el 0 antes del punto, como 0.5", MsgBoxStyle.OkOnly, "Error de captura")

                End If

            End Try

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvDetalleConRelacion_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetalleConRelacion.CellEndEdit

        Dim queryNoRelacionados As String
        queryNoRelacionados = "" & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT((sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty)) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN (SELECT isupplierinvoiceid, iprojectid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid  AND i.iinputid = sip.iinputid " & _
        "LEFT JOIN (SELECT isupplierinvoiceid, iassetid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid  AND i.iinputid = sia.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
        "AND sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty) > 0"

        setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

        dgvDetalleSinRelacion.Columns(0).Visible = False
        dgvDetalleSinRelacion.Columns(1).Visible = False

        dgvDetalleSinRelacion.Columns(0).Width = 30
        dgvDetalleSinRelacion.Columns(1).Width = 30
        dgvDetalleSinRelacion.Columns(2).Width = 200
        dgvDetalleSinRelacion.Columns(3).Width = 70
        dgvDetalleSinRelacion.Columns(4).Width = 70
        dgvDetalleSinRelacion.Columns(5).Width = 70


        Dim queryRelacionados As String

        queryRelacionados = "" & _
        "SELECT * FROM ( " & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty), 2) AS Cantidad, " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
        "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
        "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a ON sia.iassetid = a.iassetid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid AND sia.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
        "UNION " & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
        "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
        "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid AND sip.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
        ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

        setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

        dgvDetalleConRelacion.Columns(0).Visible = False
        dgvDetalleConRelacion.Columns(1).Visible = False
        dgvDetalleConRelacion.Columns(6).Visible = False
        dgvDetalleConRelacion.Columns(8).Visible = False
        dgvDetalleConRelacion.Columns(10).Visible = False
        dgvDetalleConRelacion.Columns(11).Visible = False

        dgvDetalleConRelacion.Columns(0).ReadOnly = True
        dgvDetalleConRelacion.Columns(1).ReadOnly = True
        dgvDetalleConRelacion.Columns(2).ReadOnly = True
        dgvDetalleConRelacion.Columns(3).ReadOnly = False
        dgvDetalleConRelacion.Columns(4).ReadOnly = True
        dgvDetalleConRelacion.Columns(5).ReadOnly = True
        dgvDetalleConRelacion.Columns(6).ReadOnly = True
        dgvDetalleConRelacion.Columns(7).ReadOnly = True
        dgvDetalleConRelacion.Columns(8).ReadOnly = True
        dgvDetalleConRelacion.Columns(9).ReadOnly = True

        dgvDetalleConRelacion.Columns(0).Width = 30
        dgvDetalleConRelacion.Columns(1).Width = 30
        dgvDetalleConRelacion.Columns(2).Width = 200
        dgvDetalleConRelacion.Columns(3).Width = 70
        dgvDetalleConRelacion.Columns(4).Width = 70
        dgvDetalleConRelacion.Columns(5).Width = 70
        dgvDetalleConRelacion.Columns(6).Width = 30
        dgvDetalleConRelacion.Columns(7).Width = 200
        dgvDetalleConRelacion.Columns(8).Width = 30
        dgvDetalleConRelacion.Columns(9).Width = 300

    End Sub


    Private Sub btnEliminarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarRelacion.Click

        If iselectedrelationtypeid = 0 Then
            Exit Sub
        End If

        If MsgBox("¿Realmente deseas borrar esta relación?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Insumo Factura") = MsgBoxResult.Yes Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If iselectedrelationtypeid = 1 Then

                'Assets
                If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iassetid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                End If

            Else

                'Projects
                If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND iprojectid = " & iselectedrelationid & " AND iinputid = " & iselectedinputwithrelationid) = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Proyecto " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                End If

            End If

            Dim queryNoRelacionados As String

            queryNoRelacionados = "" & _
            "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
            "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT((sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty)) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total' " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "JOIN inputs i ON sii.iinputid = i.iinputid " & _
            "LEFT JOIN (SELECT isupplierinvoiceid, iprojectid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid  AND i.iinputid = sip.iinputid " & _
            "LEFT JOIN (SELECT isupplierinvoiceid, iassetid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid  AND i.iinputid = sia.iinputid " & _
            "WHERE " & _
            "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
            "AND sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty) > 0"


            setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

            dgvDetalleSinRelacion.Columns(0).Visible = False
            dgvDetalleSinRelacion.Columns(1).Visible = False

            dgvDetalleSinRelacion.Columns(0).Width = 30
            dgvDetalleSinRelacion.Columns(1).Width = 30
            dgvDetalleSinRelacion.Columns(2).Width = 200
            dgvDetalleSinRelacion.Columns(3).Width = 70
            dgvDetalleSinRelacion.Columns(4).Width = 70
            dgvDetalleSinRelacion.Columns(5).Width = 70


            Dim queryRelacionados As String

            queryRelacionados = "" & _
            "SELECT * FROM ( " & _
            "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty), 2) AS Cantidad, " & _
            "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
            "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
            "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
            "LEFT JOIN assets a ON sia.iassetid = a.iassetid " & _
            "JOIN inputs i ON sii.iinputid = i.iinputid AND sia.iinputid = i.iinputid " & _
            "WHERE " & _
            "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
            "UNION " & _
            "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
            "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
            "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
            "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
            "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
            "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
            "JOIN inputs i ON sii.iinputid = i.iinputid AND sip.iinputid = i.iinputid " & _
            "WHERE " & _
            "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
            "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
            ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

            setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

            dgvDetalleConRelacion.Columns(0).Visible = False
            dgvDetalleConRelacion.Columns(1).Visible = False
            dgvDetalleConRelacion.Columns(6).Visible = False
            dgvDetalleConRelacion.Columns(8).Visible = False
            dgvDetalleConRelacion.Columns(10).Visible = False
            dgvDetalleConRelacion.Columns(11).Visible = False

            dgvDetalleConRelacion.Columns(0).ReadOnly = True
            dgvDetalleConRelacion.Columns(1).ReadOnly = True
            dgvDetalleConRelacion.Columns(2).ReadOnly = True
            dgvDetalleConRelacion.Columns(3).ReadOnly = False
            dgvDetalleConRelacion.Columns(4).ReadOnly = True
            dgvDetalleConRelacion.Columns(5).ReadOnly = True
            dgvDetalleConRelacion.Columns(6).ReadOnly = True
            dgvDetalleConRelacion.Columns(7).ReadOnly = True
            dgvDetalleConRelacion.Columns(8).ReadOnly = True
            dgvDetalleConRelacion.Columns(9).ReadOnly = True

            dgvDetalleConRelacion.Columns(0).Width = 30
            dgvDetalleConRelacion.Columns(1).Width = 30
            dgvDetalleConRelacion.Columns(2).Width = 200
            dgvDetalleConRelacion.Columns(3).Width = 70
            dgvDetalleConRelacion.Columns(4).Width = 70
            dgvDetalleConRelacion.Columns(5).Width = 70
            dgvDetalleConRelacion.Columns(6).Width = 30
            dgvDetalleConRelacion.Columns(7).Width = 200
            dgvDetalleConRelacion.Columns(8).Width = 30
            dgvDetalleConRelacion.Columns(9).Width = 300

        End If

    End Sub


    Private Sub btnBuscarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarRelacion.Click

        If cmbTipoRelacion.SelectedItem = "" Then
            Exit Sub
        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Activo" Then

            'Assets

            iselectedrelationtypeid = 0
            iselectedrelationid = 0
            iselectedinputwithrelationid = 0

            Dim ba As New BuscaActivos

            ba.susername = susername
            ba.bactive = bactive
            ba.bonline = bonline
            ba.suserfullname = suserfullname

            ba.suseremail = suseremail
            ba.susersession = susersession
            ba.susermachinename = susermachinename
            ba.suserip = suserip

            ba.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                ba.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            ba.ShowDialog(Me)
            Me.Visible = True

            If ba.DialogResult = Windows.Forms.DialogResult.OK Then

                iselectedrelationtypeid = 1
                iselectedrelationid = ba.iassetid

                Dim fecha As Integer = getMySQLDate()
                Dim hora As String = getAppTime()

                If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets VALUES (" & isupplierinvoiceid & ", " & iselectedrelationid & ", " & iselectedinputwithoutrelationid & ", 0, '', " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    iselectedinputwithrelationid = iselectedinputwithoutrelationid
                End If

            End If


        ElseIf cmbTipoRelacion.SelectedItem = "Relacionar a Proyecto" Then

            'Proyectos

            iselectedrelationtypeid = 0
            iselectedrelationid = 0
            iselectedinputwithrelationid = 0

            Dim bi As New BuscaProyectos

            bi.susername = susername
            bi.bactive = bactive
            bi.bonline = bonline
            bi.suserfullname = suserfullname

            bi.suseremail = suseremail
            bi.susersession = susersession
            bi.susermachinename = susermachinename
            bi.suserip = suserip

            bi.isEdit = False

            If Me.WindowState = FormWindowState.Maximized Then
                bi.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            bi.ShowDialog(Me)
            Me.Visible = True

            If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                iselectedrelationtypeid = 2
                iselectedrelationid = bi.iprojectid

                Dim fecha As Integer = getMySQLDate()
                Dim hora As String = getAppTime()

                If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects VALUES (" & isupplierinvoiceid & ", " & iselectedrelationid & ", " & iselectedinputwithoutrelationid & ", 0, '', " & fecha & ", '" & hora & "', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then
                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Agrego el Activo " & iselectedrelationid & " a la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
                    iselectedinputwithrelationid = iselectedinputwithoutrelationid
                End If

            End If

        End If

        Dim queryNoRelacionados As String
        queryNoRelacionados = "" & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT((sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty)) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total' " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid " & _
        "LEFT JOIN (SELECT isupplierinvoiceid, iprojectid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid  AND i.iinputid = sip.iinputid " & _
        "LEFT JOIN (SELECT isupplierinvoiceid, iassetid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid  AND i.iinputid = sia.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
        "AND sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty) > 0"


        setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

        dgvDetalleSinRelacion.Columns(0).Visible = False
        dgvDetalleSinRelacion.Columns(1).Visible = False

        dgvDetalleSinRelacion.Columns(0).Width = 30
        dgvDetalleSinRelacion.Columns(1).Width = 30
        dgvDetalleSinRelacion.Columns(2).Width = 200
        dgvDetalleSinRelacion.Columns(3).Width = 70
        dgvDetalleSinRelacion.Columns(4).Width = 70
        dgvDetalleSinRelacion.Columns(5).Width = 70


        Dim queryRelacionados As String

        queryRelacionados = "" & _
        "SELECT * FROM ( " & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty), 2) AS Cantidad, " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
        "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
        "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
        "LEFT JOIN assets a ON sia.iassetid = a.iassetid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid AND sia.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
        "UNION " & _
        "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
        "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
        "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
        "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
        "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
        "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
        "JOIN inputs i ON sii.iinputid = i.iinputid AND sip.iinputid = i.iinputid " & _
        "WHERE " & _
        "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
        "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
        ") tmpA ORDER BY iinsertiondate ASC, sinsertiontime ASC "

        setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

        dgvDetalleConRelacion.Columns(0).Visible = False
        dgvDetalleConRelacion.Columns(1).Visible = False
        dgvDetalleConRelacion.Columns(6).Visible = False
        dgvDetalleConRelacion.Columns(8).Visible = False
        dgvDetalleConRelacion.Columns(10).Visible = False
        dgvDetalleConRelacion.Columns(11).Visible = False

        dgvDetalleConRelacion.Columns(0).ReadOnly = True
        dgvDetalleConRelacion.Columns(1).ReadOnly = True
        dgvDetalleConRelacion.Columns(2).ReadOnly = True
        dgvDetalleConRelacion.Columns(3).ReadOnly = False
        dgvDetalleConRelacion.Columns(4).ReadOnly = True
        dgvDetalleConRelacion.Columns(5).ReadOnly = True
        dgvDetalleConRelacion.Columns(6).ReadOnly = True
        dgvDetalleConRelacion.Columns(7).ReadOnly = True
        dgvDetalleConRelacion.Columns(8).ReadOnly = True
        dgvDetalleConRelacion.Columns(9).ReadOnly = True

        dgvDetalleConRelacion.Columns(0).Width = 30
        dgvDetalleConRelacion.Columns(1).Width = 30
        dgvDetalleConRelacion.Columns(2).Width = 200
        dgvDetalleConRelacion.Columns(3).Width = 70
        dgvDetalleConRelacion.Columns(4).Width = 70
        dgvDetalleConRelacion.Columns(5).Width = 70
        dgvDetalleConRelacion.Columns(6).Width = 30
        dgvDetalleConRelacion.Columns(7).Width = 200
        dgvDetalleConRelacion.Columns(8).Width = 30
        dgvDetalleConRelacion.Columns(9).Width = 300

    End Sub


    'Private Sub btnRelacionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRelacionar.Click


    '    If iselectedrelationid = 0 Then

    '        Exit Sub

    '    ElseIf iselectedrelationtypeid = 1 Then

    '        'Assets


    '    Else

    '        'Projects


    '    End If


    '    Dim queryNoRelacionados As String
    'queryNoRelacionados = "" & _
    '"SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
    '"FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT((sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty)) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total' " & _
    '"FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
    '"JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
    '"JOIN inputs i ON sii.iinputid = i.iinputid " & _
    '"LEFT JOIN (SELECT isupplierinvoiceid, iprojectid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid  AND i.iinputid = sip.iinputid " & _
    '"LEFT JOIN (SELECT isupplierinvoiceid, iassetid, iinputid, SUM(dinputqty) AS dinputqty, ssupplierinvoiceextranote, iupdatedate, supdatetime, supdateusername FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & " GROUP BY iinputid) sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid  AND i.iinputid = sia.iinputid " & _
    '"WHERE " & _
    '"si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
    '"AND sii.dinputqty - IF(sia.dinputqty IS NULL, 0, sia.dinputqty) - IF(sip.dinputqty IS NULL, 0, sip.dinputqty) > 0"



    '    setDataGridView(dgvDetalleSinRelacion, queryNoRelacionados, True)

    '    dgvDetalleSinRelacion.Columns(0).Visible = False
    '    dgvDetalleSinRelacion.Columns(1).Visible = False

    '    dgvDetalleSinRelacion.Columns(0).Width = 30
    '    dgvDetalleSinRelacion.Columns(1).Width = 30
    '    dgvDetalleSinRelacion.Columns(2).Width = 200
    '    dgvDetalleSinRelacion.Columns(3).Width = 70
    '    dgvDetalleSinRelacion.Columns(4).Width = 70
    '    dgvDetalleSinRelacion.Columns(5).Width = 70


    'Dim queryRelacionados As String

    'queryRelacionados = "" & _
    '    "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty), 2) AS Cantidad, " & _
    '    "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sia.dinputqty IS NULL, 0, sia.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
    '    "1 AS 'ID Tipo de Relacion', 'A Activo' AS 'Tipo de Relacion', " & _
    '    "sia.iassetid AS irelationid, a.sassetdescription AS 'Relacionado A', sia.iinsertiondate, sia.sinsertiontime " & _
    '    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
    '    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
    '    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets sia ON si.isupplierinvoiceid = sia.isupplierinvoiceid " & _
    '    "LEFT JOIN assets a ON sia.iassetid = a.iassetid " & _
    '    "JOIN inputs i ON sii.iinputid = i.iinputid AND sia.iinputid = i.iinputid " & _
    '    "WHERE " & _
    '    "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
    '    "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) " & _
    '    "UNION " & _
    '    "SELECT si.isupplierinvoiceid, i.iinputid, i.sinputdescription AS 'Insumo', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty), 2) AS Cantidad, " & _
    '    "FORMAT(sii.dsupplierinvoiceinputunitprice, 2) AS 'Precio Unitario', FORMAT(IF(sip.dinputqty IS NULL, 0, sip.dinputqty) * sii.dsupplierinvoiceinputunitprice, 2) AS 'Importe Total', " & _
    '    "2 AS 'ID Tipo de Relacion', 'A Proyecto' AS 'Tipo de Relacion', " & _
    '    "sip.iprojectid AS irelationid, p.sprojectname AS 'Relacionado A', sip.iinsertiondate, sip.sinsertiontime " & _
    '    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si " & _
    '    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs sii ON si.isupplierinvoiceid = sii.isupplierinvoiceid " & _
    '    "LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects sip ON si.isupplierinvoiceid = sip.isupplierinvoiceid " & _
    '    "LEFT JOIN projects p ON sip.iprojectid = p.iprojectid " & _
    '    "JOIN inputs i ON sii.iinputid = i.iinputid AND sip.iinputid = i.iinputid " & _
    '    "WHERE " & _
    '    "si.isupplierinvoiceid = " & isupplierinvoiceid & " " & _
    '    "AND (EXISTS(SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid & ")) "

    'setDataGridView(dgvDetalleConRelacion, queryRelacionados, False)

    '    dgvDetalleConRelacion.Columns(0).Visible = False
    '    dgvDetalleConRelacion.Columns(1).Visible = False
    '    dgvDetalleConRelacion.Columns(6).Visible = False
    '    dgvDetalleConRelacion.Columns(8).Visible = False
    '    dgvDetalleConRelacion.Columns(10).Visible = False
    '    dgvDetalleConRelacion.Columns(11).Visible = False

    '    dgvDetalleConRelacion.Columns(0).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(1).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(2).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(4).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(5).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(6).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(7).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(8).ReadOnly = True
    '    dgvDetalleConRelacion.Columns(9).ReadOnly = True

    '    dgvDetalleConRelacion.Columns(0).Width = 30
    '    dgvDetalleConRelacion.Columns(1).Width = 30
    '    dgvDetalleConRelacion.Columns(2).Width = 200
    '    dgvDetalleConRelacion.Columns(3).Width = 70
    '    dgvDetalleConRelacion.Columns(4).Width = 70
    '    dgvDetalleConRelacion.Columns(5).Width = 70
    '    dgvDetalleConRelacion.Columns(6).Width = 30
    '    dgvDetalleConRelacion.Columns(7).Width = 100
    '    dgvDetalleConRelacion.Columns(8).Width = 30
    '    dgvDetalleConRelacion.Columns(9).Width = 200

    '    iselectedrelationid = 0
    '    txtRelacion.Text = ""

    'End Sub


    Private Function validaRelaciones(ByVal silent As Boolean) As Boolean

        Dim totalqty As Double = 0.0
        Dim projectsqty As Double = 0.0
        Dim assetsqty As Double = 0.0

        Dim resta As Double = 0.0

        totalqty = getSQLQueryAsDouble(0, "SELECT SUM(dinputqty) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)
        projectsqty = getSQLQueryAsDouble(0, "SELECT SUM(dinputqty) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects WHERE isupplierinvoiceid = " & isupplierinvoiceid)
        assetsqty = getSQLQueryAsDouble(0, "SELECT SUM(dinputqty) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        resta = totalqty - projectsqty - assetsqty

        If resta > 0.0 Then

            If silent = False Then
                MsgBox("Quedan productos por asignar...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

        ElseIf resta < 0.0 Then

            If silent = False Then
                MsgBox("Estás intentando asignar más productos de los que tiene esta Factura...", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            Return False

        End If

        'Sólo si es 0, e.g. si todos los productos de la Factura están relacionados
        Return True

    End Function


    Private Sub btnGuardarRelacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarRelacion.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaRelaciones(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(12) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(10) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(11) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
                btnRevisiones.Enabled = True
                btnRevisionesRelaciones.Enabled = True
                btnRevisionesPagos.Enabled = True

            Else

                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnGuardarRelacionYCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarRelacionYCerrar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaRelaciones(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(12) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(10) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(11) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & " para Cerrar', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnPaso3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaso3.Click

        If validaRelaciones(False) = True Then
            tcFacturas.SelectedTab = tpPagos
        End If

    End Sub


    Private Sub dgvPagos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellClick

        Try

            iselectedpaymentid = CInt(dgvPagos.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedpaymentid = 0

        End Try

    End Sub


    Private Sub dgvPagos_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellContentClick

        Try

            iselectedpaymentid = CInt(dgvPagos.Rows(e.RowIndex).Cells(0).Value())

        Catch ex As Exception

            iselectedpaymentid = 0

        End Try

    End Sub


    Private Sub dgvPagos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvPagos.SelectionChanged

        Try

            iselectedpaymentid = CInt(dgvPagos.CurrentRow.Cells(0).Value())

        Catch ex As Exception

            iselectedpaymentid = 0

        End Try

    End Sub


    Private Sub dgvPagos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellValueChanged

        If modifyPaymentPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If e.ColumnIndex = 6 Then

            dgvPagos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvPagos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments SET ssupplierinvoiceextranote = '" & dgvPagos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND ipaymentid = " & iselectedpaymentid) = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Modifico las Notas del Pago " & iselectedrelationid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            End If

            txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim dsDescuentos As DataSet
            dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim subtotal As Double = 0.0
            Dim iva As Double = 0.0
            Dim total As Double = 0.0

            subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            If dsDescuentos.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                    subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                Next i

            End If

            iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
            total = subtotal + iva

            txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvPagos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvPagos.EditingControlShowing

        If dgvPagos.CurrentCell.ColumnIndex = 6 Then

            txtObservacionesDgvPagos = CType(e.Control, TextBox)
            txtObservacionesDgvPagos_OldText = txtObservacionesDgvPagos.Text

        Else

            txtObservacionesDgvPagos = Nothing
            txtObservacionesDgvPagos_OldText = Nothing

        End If

    End Sub


    Private Sub txtObservacionesDgvPagos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtObservacionesDgvPagos.KeyUp

        txtObservacionesDgvPagos.Text = txtObservacionesDgvPagos.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden7 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden7 As Char() = strForbidden7.ToCharArray

        For fc = 0 To arrayForbidden7.Length - 1

            If txtObservacionesDgvPagos.Text.Contains(arrayForbidden7(fc)) Then
                txtObservacionesDgvPagos.Text = txtObservacionesDgvPagos.Text.Replace(arrayForbidden7(fc), "")
            End If

        Next fc

    End Sub


    Private Sub dgvPagos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPagos.CellEndEdit

        Dim querySupplierInvoicePayments As String
        querySupplierInvoicePayments = "" & _
        "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
        "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
        "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
        "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
        "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvPagos, querySupplierInvoicePayments, False)

        dgvPagos.Columns(0).Visible = False
        dgvPagos.Columns(1).Visible = False

        dgvPagos.Columns(0).ReadOnly = True
        dgvPagos.Columns(1).ReadOnly = True
        dgvPagos.Columns(2).ReadOnly = True
        dgvPagos.Columns(3).ReadOnly = True
        dgvPagos.Columns(4).ReadOnly = True
        dgvPagos.Columns(5).ReadOnly = True

        dgvPagos.Columns(0).Width = 30
        dgvPagos.Columns(1).Width = 30
        dgvPagos.Columns(2).Width = 70
        dgvPagos.Columns(3).Width = 70
        dgvPagos.Columns(4).Width = 100
        dgvPagos.Columns(5).Width = 200
        dgvPagos.Columns(6).Width = 200

    End Sub


    Private Sub dgvPagos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvPagos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deletePaymentPermission = False Then
                Exit Sub
            End If

            If MsgBox("¿Realmente deseas borrar la relación de este Pago con la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Pago y Relación Pago Factura") = MsgBoxResult.Yes Then


                If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND ipaymentid = " & iselectedpaymentid) = True Then

                    Dim fecha As Integer = getMySQLDate()
                    Dim hora As String = getAppTime()

                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Pago " & iselectedpaymentid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

                End If


                Dim querySupplierInvoicePayments As String
                querySupplierInvoicePayments = "" & _
                "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
                "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
                "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
                "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
                "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
                "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
                "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

                setDataGridView(dgvPagos, querySupplierInvoicePayments, False)

                dgvPagos.Columns(0).Visible = False
                dgvPagos.Columns(1).Visible = False

                dgvPagos.Columns(0).ReadOnly = True
                dgvPagos.Columns(1).ReadOnly = True
                dgvPagos.Columns(2).ReadOnly = True
                dgvPagos.Columns(3).ReadOnly = True
                dgvPagos.Columns(4).ReadOnly = True
                dgvPagos.Columns(5).ReadOnly = True

                dgvPagos.Columns(0).Width = 30
                dgvPagos.Columns(1).Width = 30
                dgvPagos.Columns(2).Width = 70
                dgvPagos.Columns(3).Width = 70
                dgvPagos.Columns(4).Width = 100
                dgvPagos.Columns(5).Width = 200
                dgvPagos.Columns(6).Width = 200

                txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim dsDescuentos As DataSet
                dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                Dim subtotal As Double = 0.0
                Dim iva As Double = 0.0
                Dim total As Double = 0.0

                subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

                If dsDescuentos.Tables(0).Rows.Count > 0 Then

                    For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                        subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                    Next i

                End If

                iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
                total = subtotal + iva

                txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            End If

        End If

    End Sub


    Private Sub dgvPagos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvPagos.UserAddedRow

        If insertPaymentPermission = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaPagos

        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname

        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.querystring = dgvPagos.CurrentCell.EditedFormattedValue

        bi.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then


            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments VALUES ( " & bi.ipaymentid & ", " & isupplierinvoiceid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Pago " & iselectedpaymentid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

            End If

            txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim dsDescuentos As DataSet
            dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim subtotal As Double = 0.0
            Dim iva As Double = 0.0
            Dim total As Double = 0.0

            subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            If dsDescuentos.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                    subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                Next i

            End If

            iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
            total = subtotal + iva

            txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

        'dgvPagos.EndEdit()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnAgregarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ai As New AgregarPago

        ai.susername = susername
        ai.bactive = bactive
        ai.bonline = bonline
        ai.suserfullname = suserfullname
        ai.suseremail = suseremail
        ai.susersession = susersession
        ai.susermachinename = susermachinename
        ai.suserip = suserip

        ai.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ai.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ai.ShowDialog(Me)
        Me.Visible = True

        If ai.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments VALUES ( " & ai.ipaymentid & ", " & isupplierinvoiceid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Pago " & iselectedpaymentid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

            End If

        End If

        Dim querySupplierInvoicePayments As String

        querySupplierInvoicePayments = "" & _
        "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
        "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
        "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
        "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
        "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvPagos, querySupplierInvoicePayments, False)

        dgvPagos.Columns(0).Visible = False
        dgvPagos.Columns(1).Visible = False

        dgvPagos.Columns(0).ReadOnly = True
        dgvPagos.Columns(1).ReadOnly = True
        dgvPagos.Columns(2).ReadOnly = True
        dgvPagos.Columns(3).ReadOnly = True
        dgvPagos.Columns(4).ReadOnly = True
        dgvPagos.Columns(5).ReadOnly = True

        dgvPagos.Columns(0).Width = 30
        dgvPagos.Columns(1).Width = 30
        dgvPagos.Columns(2).Width = 70
        dgvPagos.Columns(3).Width = 70
        dgvPagos.Columns(4).Width = 100
        dgvPagos.Columns(5).Width = 200
        dgvPagos.Columns(6).Width = 200

        txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        Dim subtotal As Double = 0.0
        Dim iva As Double = 0.0
        Dim total As Double = 0.0

        subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i

        End If

        iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
        total = subtotal + iva

        txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnInsertarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaPagos

        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname
        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            If executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments VALUES ( " & bi.ipaymentid & ", " & isupplierinvoiceid & ", '', " & fecha & ", '" & hora & "', '" & susername & "')") = True Then
                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Pago " & iselectedpaymentid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")
            End If

        End If

        Dim querySupplierInvoicePayments As String
        querySupplierInvoicePayments = "" & _
        "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
        "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
        "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
        "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
        "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
        "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
        "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

        setDataGridView(dgvPagos, querySupplierInvoicePayments, False)

        dgvPagos.Columns(0).Visible = False
        dgvPagos.Columns(1).Visible = False

        dgvPagos.Columns(0).ReadOnly = True
        dgvPagos.Columns(1).ReadOnly = True
        dgvPagos.Columns(2).ReadOnly = True
        dgvPagos.Columns(3).ReadOnly = True
        dgvPagos.Columns(4).ReadOnly = True
        dgvPagos.Columns(5).ReadOnly = True
        dgvPagos.Columns(6).ReadOnly = False

        dgvPagos.Columns(0).Width = 30
        dgvPagos.Columns(1).Width = 30
        dgvPagos.Columns(2).Width = 70
        dgvPagos.Columns(3).Width = 70
        dgvPagos.Columns(4).Width = 100
        dgvPagos.Columns(5).Width = 200
        dgvPagos.Columns(6).Width = 200

        txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        Dim subtotal As Double = 0.0
        Dim iva As Double = 0.0
        Dim total As Double = 0.0

        subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i

        End If

        iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
        total = subtotal + iva

        txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarPago.Click

        If MsgBox("¿Realmente deseas borrar la relación de este Pago con la Factura?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Relación Pago - Factura") = MsgBoxResult.Yes Then

            If executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND ipaymentid = " & iselectedpaymentid) = True Then

                Dim fecha As Integer = getMySQLDate()
                Dim hora As String = getAppTime()

                executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Eliminó el Pago " & iselectedpaymentid & " de la Factura de Proveedor " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')")

            End If


            Dim querySupplierInvoicePayments As String
            querySupplierInvoicePayments = "" & _
            "SELECT sipy.ipaymentid, sipy.isupplierinvoiceid, " & _
            "STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') AS Fecha, " & _
            "py.dpaymentamount AS Cantidad, pyt.spaymenttypedescription AS 'Tipo de Pago', " & _
            "py.spaymentdescription AS 'Descripcion', sipy.ssupplierinvoiceextranote AS Observaciones " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy " & _
            "JOIN payments py ON sipy.ipaymentid = py.ipaymentid " & _
            "JOIN paymenttypes pyt ON py.ipaymenttypeid = pyt.ipaymenttypeid " & _
            "WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid

            setDataGridView(dgvPagos, querySupplierInvoicePayments, False)

            dgvPagos.Columns(0).Visible = False
            dgvPagos.Columns(1).Visible = False

            dgvPagos.Columns(0).ReadOnly = True
            dgvPagos.Columns(1).ReadOnly = True
            dgvPagos.Columns(2).ReadOnly = True
            dgvPagos.Columns(3).ReadOnly = True
            dgvPagos.Columns(4).ReadOnly = True
            dgvPagos.Columns(5).ReadOnly = True

            dgvPagos.Columns(0).Width = 30
            dgvPagos.Columns(1).Width = 30
            dgvPagos.Columns(2).Width = 70
            dgvPagos.Columns(3).Width = 70
            dgvPagos.Columns(4).Width = 100
            dgvPagos.Columns(5).Width = 200
            dgvPagos.Columns(6).Width = 200

            txtSumaPagos.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS Cantidad FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim dsDescuentos As DataSet
            dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim subtotal As Double = 0.0
            Dim iva As Double = 0.0
            Dim total As Double = 0.0

            subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            If dsDescuentos.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                    subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

                Next i

            End If

            iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
            total = subtotal + iva

            txtMontoRestante.Text = FormatCurrency(getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        End If

    End Sub


    Private Function validaPagos(ByVal silent As Boolean) As Boolean

        Dim dsDescuentos As DataSet
        dsDescuentos = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        Dim subtotal As Double = 0.0
        Dim iva As Double = 0.0
        Dim total As Double = 0.0

        subtotal = getSQLQueryAsDouble(0, "SELECT FORMAT(IF(SUM(dsupplierinvoiceinputtotalprice) IS NULL, 0, SUM(dsupplierinvoiceinputtotalprice)), 2) AS subtotalFactura FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs WHERE isupplierinvoiceid = " & isupplierinvoiceid)

        If dsDescuentos.Tables(0).Rows.Count > 0 Then

            For i = 0 To dsDescuentos.Tables(0).Rows.Count - 1

                subtotal = getSQLQueryAsDouble(0, "SELECT " & subtotal & " - (" & subtotal & " * dsupplierinvoicediscountpercentage) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND isupplierinvoicediscountid = " & dsDescuentos.Tables(0).Rows(i).Item("isupplierinvoicediscountid"))

            Next i

        End If

        iva = subtotal * getSQLQueryAsDouble(0, "SELECT FORMAT(IF(si.dIVApercentage IS NULL, 0, si.dIVApercentage), 2) AS IVA FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " si WHERE si.isupplierinvoiceid = " & isupplierinvoiceid)
        total = subtotal + iva

        Dim resta As Double = 0.0

        resta = getSQLQueryAsDouble(0, "SELECT FORMAT(" & total & " - IF(SUM(py.dpaymentamount) IS NULL, 0, SUM(py.dpaymentamount)), 2) AS restante FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments sipy LEFT JOIN payments py ON sipy.ipaymentid = py.ipaymentid WHERE sipy.isupplierinvoiceid = " & isupplierinvoiceid)

        If resta > 0.0 Then

            If silent = False Then
                MsgBox("Nota: Hay saldo pendiente de esta Factura", MsgBoxStyle.OkOnly, "Información")
            End If

            Return True

        ElseIf resta < 0.0 Then

            If silent = False Then
                MsgBox("Nota: Estás dejando saldo a favor para este Proveedor", MsgBoxStyle.OkOnly, "Información")
            End If

            Return True

        End If

        Return True

    End Function


    Private Sub btnGuardarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaPagos(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(12) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(10) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(11) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
                btnRevisiones.Enabled = True
                btnRevisionesRelaciones.Enabled = True
                btnRevisionesPagos.Enabled = True

            Else

                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If

            wasCreated = True

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnGuardarPagoYCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarPagoYCerrar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaPagos(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(12) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(6) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(7) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(8) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(9) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(10) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(11) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & " para Cerrar', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click, btnRevisionesRelaciones.Click, btnRevisionesPagos.Click

        If MsgBox("Revisar una Factura automáticamente guarda sus cambios. ¿Deseas guardar esta Factura ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If validaFacturaCompleta(False) = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            Exit Sub

        Else

            Dim timesInvoiceIsOpen As Integer = 1

            timesInvoiceIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid & "'")

            If timesInvoiceIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierta la misma Factura. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesInvoiceIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%SupplierInvoice" & isupplierinvoiceid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(4) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments"
                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts"
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & " SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Inputs SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Assets SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Projects SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Payments SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid
                queriesNewId(2) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid + newIdAddition & "Discounts SET isupplierinvoiceid = " & isupplierinvoiceid + newIdAddition & " WHERE isupplierinvoiceid = " & isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    isupplierinvoiceid = isupplierinvoiceid + newIdAddition
                End If

            End If


            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim porcentajeIVA As Double = 0.0

            Try
                porcentajeIVA = CDbl(txtPorcentajeIVA.Text)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " SET isupplierid = " & isupplierid & ", iinvoicedate = " & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(0, 10).Replace("-", "").Replace("/", "").Replace("\", "") & ", sinvoicetime = '" & convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(dtFechaFactura.Value).Substring(10).Trim.Replace(".000", "") & "', isupplierinvoicetypeid = " & cmbTipoFactura.SelectedValue & ", ssupplierinvoicefolio = '" & txtFolioFacturaProveedor.Text & "', sexpensedescription = '" & txtDescripcionFactura.Text & "', sexpenselocation = '" & txtLugar.Text & "', dIVApercentage = " & porcentajeIVA / 100 & ", ipeopleid = " & ipeopleid & ", iupdatedate = " & fecha & ", supdatetime = '" & hora & "', supdateusername = '" & susername & "' WHERE isupplierinvoiceid = " & isupplierinvoiceid)

            Dim queries(20) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM supplierinvoices " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi WHERE supplierinvoices.isupplierinvoiceid = tsi.isupplierinvoiceid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii WHERE supplierinvoiceinputs.isupplierinvoiceid = tsii.isupplierinvoiceid AND supplierinvoiceinputs.iinputid = tsii.iinputid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip WHERE supplierinvoiceprojects.isupplierinvoiceid = tsip.isupplierinvoiceid AND supplierinvoiceprojects.iprojectid = tsip.iprojectid AND supplierinvoiceprojects.iinputid = tsip.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia WHERE supplierinvoiceassets.isupplierinvoiceid = tsia.isupplierinvoiceid AND supplierinvoiceassets.iassetid = tsia.iassetid AND supplierinvoiceassets.iinputid = tsia.iinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy WHERE supplierinvoicepayments.ipaymentid = tsipy.ipaymentid AND supplierinvoicepayments.isupplierinvoiceid = tsipy.isupplierinvoiceid) "

            queries(6) = "" & _
            "DELETE " & _
            "FROM supplierinvoicediscounts " & _
            "WHERE isupplierinvoiceid = " & isupplierinvoiceid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid WHERE supplierinvoicediscounts.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND supplierinvoicediscounts.isupplierinvoiceid = tsid.isupplierinvoiceid) "

            queries(7) = "" & _
            "UPDATE supplierinvoices si JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi ON tsi.isupplierinvoiceid = si.isupplierinvoiceid SET si.isupplierid = tsi.isupplierid, si.iupdatedate = tsi.iupdatedate, si.supdatetime = tsi.supdatetime, si.iinvoicedate = tsi.iinvoicedate, si.sinvoicetime = tsi.sinvoicetime, si.isupplierinvoicetypeid = tsi.isupplierinvoicetypeid, si.ssupplierinvoicefolio = tsi.ssupplierinvoicefolio, si.sexpensedescription = tsi.sexpensedescription, si.sexpenselocation = tsi.sexpenselocation, si.dIVApercentage = tsi.dIVApercentage, si.ipeopleid = tsi.ipeopleid WHERE STR_TO_DATE(CONCAT(tsi.iupdatedate, ' ', tsi.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(si.iupdatedate, ' ', si.supdatetime), '%Y%c%d %T') "

            queries(8) = "" & _
            "UPDATE supplierinvoiceinputs sii JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii ON tsii.isupplierinvoiceid = sii.isupplierinvoiceid AND tsii.iinputid = sii.iinputid SET sii.sinputextranote = tsii.sinputextranote, sii.dinputqty = tsii.dinputqty, sii.dsupplierinvoiceinputtotalprice = tsii.dsupplierinvoiceinputtotalprice, sii.dsupplierinvoiceinputunitprice = tsii.dsupplierinvoiceinputunitprice, sii.iupdatedate = tsii.iupdatedate, sii.supdatetime = tsii.supdatetime, sii.supdateusername = tsii.supdateusername WHERE STR_TO_DATE(CONCAT(tsii.iupdatedate, ' ', tsii.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sii.iupdatedate, ' ', sii.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE supplierinvoiceprojects sip JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip ON tsip.isupplierinvoiceid = sip.isupplierinvoiceid AND tsip.iprojectid = sip.iprojectid AND tsip.iinputid = sip.iinputid SET sip.dinputqty = tsip.dinputqty, sip.ssupplierinvoiceextranote = tsip.ssupplierinvoiceextranote, sip.iinsertiondate = tsip.iinsertiondate, sip.sinsertiontime = tsip.sinsertiontime, sip.iupdatedate = tsip.iupdatedate, sip.supdatetime = tsip.supdatetime, sip.supdateusername = tsip.supdateusername WHERE STR_TO_DATE(CONCAT(tsip.iupdatedate, ' ', tsip.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE supplierinvoiceassets sia JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia ON tsia.isupplierinvoiceid = sia.isupplierinvoiceid AND tsia.iassetid = sia.iassetid AND tsia.iinputid = sia.iinputid SET sia.dinputqty = tsia.dinputqty, sia.ssupplierinvoiceextranote = tsia.ssupplierinvoiceextranote, sia.iinsertiondate = tsia.iinsertiondate, sia.sinsertiontime = tsia.sinsertiontime, sia.iupdatedate = tsia.iupdatedate, sia.supdatetime = tsia.supdatetime, sia.supdateusername = tsia.supdateusername WHERE STR_TO_DATE(CONCAT(tsia.iupdatedate, ' ', tsia.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sia.iupdatedate, ' ', sia.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE supplierinvoicepayments sipy JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy ON tsipy.isupplierinvoiceid = sipy.isupplierinvoiceid AND tsipy.ipaymentid = sipy.ipaymentid SET sipy.ssupplierinvoiceextranote = tsipy.ssupplierinvoiceextranote, sipy.iupdatedate = tsipy.iupdatedate, sipy.supdatetime = tsipy.supdatetime, sipy.supdateusername = tsipy.supdateusername WHERE STR_TO_DATE(CONCAT(tsipy.iupdatedate, ' ', tsipy.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sipy.iupdatedate, ' ', sipy.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE supplierinvoicediscounts sid JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid ON tsid.isupplierinvoiceid = sid.isupplierinvoiceid AND tsid.isupplierinvoicediscountid = sid.isupplierinvoicediscountid SET sid.ssupplierinvoicediscountdescription = tsid.ssupplierinvoicediscountdescription, sid.iupdatedate = tsid.iupdatedate, sid.supdatetime = tsid.supdatetime, sid.dsupplierinvoicediscountpercentage = tsid.dsupplierinvoicediscountpercentage WHERE STR_TO_DATE(CONCAT(tsid.iupdatedate, ' ', tsid.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(sid.iupdatedate, ' ', sid.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "INSERT INTO supplierinvoices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & " tsi " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoices si WHERE si.isupplierinvoiceid = tsi.isupplierinvoiceid AND si.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(14) = "" & _
            "INSERT INTO supplierinvoiceinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Inputs tsii " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceinputs sii WHERE sii.isupplierinvoiceid = tsii.isupplierinvoiceid AND sii.iinputid = tsii.iinputid AND sii.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(15) = "" & _
            "INSERT INTO supplierinvoiceprojects " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Projects tsip " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceprojects sip WHERE sip.isupplierinvoiceid = tsip.isupplierinvoiceid AND sip.iprojectid = tsip.iprojectid AND sip.iinputid = tsip.iinputid AND sip.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(16) = "" & _
            "INSERT INTO supplierinvoiceassets " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Assets tsia " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoiceassets sia WHERE sia.isupplierinvoiceid = tsia.isupplierinvoiceid AND sia.iassetid = tsia.iassetid AND sia.iinputid = tsia.iinputid AND sia.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(17) = "" & _
            "INSERT INTO supplierinvoicepayments " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Payments tsipy " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicepayments sipy WHERE sipy.ipaymentid = tsipy.ipaymentid AND sipy.isupplierinvoiceid = tsipy.isupplierinvoiceid AND sipy.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(18) = "" & _
            "INSERT INTO supplierinvoicediscounts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "SupplierInvoice" & isupplierinvoiceid & "Discounts tsid " & _
            "WHERE NOT EXISTS (SELECT * FROM supplierinvoicediscounts sid WHERE sid.isupplierinvoiceid = tsid.isupplierinvoiceid AND sid.isupplierinvoicediscountid = tsid.isupplierinvoicediscountid AND sid.isupplierinvoiceid = " & isupplierinvoiceid & ") "

            queries(19) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó la Factura de Combustible (" & isupplierinvoiceid & ") " & txtFolioFacturaProveedor.Text.Replace("'", "").Replace("--", "") & " del Proveedor " & txtProveedor.Text.Replace("'", "").Replace("--", "") & " para iniciar Revision', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                wasCreated = True

                Dim br As New BuscaRevisiones

                br.susername = susername
                br.bactive = bactive
                br.bonline = bonline
                br.suserfullname = suserfullname
                br.suseremail = suseremail
                br.susersession = susersession
                br.susermachinename = susermachinename
                br.suserip = suserip

                br.isEdit = True

                br.srevisiondocument = "Factura de Proveedor"
                br.sid = isupplierinvoiceid

                If Me.WindowState = FormWindowState.Maximized Then
                    br.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                br.ShowDialog(Me)
                Me.Visible = True

            Else

                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class