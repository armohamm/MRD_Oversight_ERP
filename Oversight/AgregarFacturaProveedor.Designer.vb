<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarFacturaProveedor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarFacturaProveedor))
        Me.tcFacturas = New System.Windows.Forms.TabControl
        Me.tpFactura = New System.Windows.Forms.TabPage
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.btnDescuentos = New System.Windows.Forms.Button
        Me.txtSubtotal = New System.Windows.Forms.TextBox
        Me.lblDescuento = New System.Windows.Forms.Label
        Me.lblDescripcionFactura = New System.Windows.Forms.Label
        Me.txtDescripcionFactura = New System.Windows.Forms.TextBox
        Me.btnTipoFactura = New System.Windows.Forms.Button
        Me.btnGuardarDatosFacturaYCerrar = New System.Windows.Forms.Button
        Me.btnCancelarFactura = New System.Windows.Forms.Button
        Me.btnGuardarDatosFactura = New System.Windows.Forms.Button
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblIVA = New System.Windows.Forms.Label
        Me.lblSubtotal = New System.Windows.Forms.Label
        Me.txtTotal = New System.Windows.Forms.TextBox
        Me.txtIVA = New System.Windows.Forms.TextBox
        Me.txtSubtotalTrasDescuentos = New System.Windows.Forms.TextBox
        Me.btnPaso2 = New System.Windows.Forms.Button
        Me.lblPorcentaje = New System.Windows.Forms.Label
        Me.btnInsertarInsumo = New System.Windows.Forms.Button
        Me.btnEliminarInsumo = New System.Windows.Forms.Button
        Me.btnNuevoInsumo = New System.Windows.Forms.Button
        Me.btnProveedor = New System.Windows.Forms.Button
        Me.btnPersona = New System.Windows.Forms.Button
        Me.lblNombreProveedor = New System.Windows.Forms.Label
        Me.lblPersona = New System.Windows.Forms.Label
        Me.txtFolioFacturaProveedor = New System.Windows.Forms.TextBox
        Me.txtPersona = New System.Windows.Forms.TextBox
        Me.lblFolioFactura = New System.Windows.Forms.Label
        Me.cmbTipoFactura = New System.Windows.Forms.ComboBox
        Me.txtLugar = New System.Windows.Forms.TextBox
        Me.dgvDetalle = New System.Windows.Forms.DataGridView
        Me.txtProveedor = New System.Windows.Forms.TextBox
        Me.txtPorcentajeIVA = New System.Windows.Forms.TextBox
        Me.lblDetalleFactura = New System.Windows.Forms.Label
        Me.lblTipoFactura = New System.Windows.Forms.Label
        Me.dtFechaFactura = New System.Windows.Forms.DateTimePicker
        Me.lblLugar = New System.Windows.Forms.Label
        Me.lblFechaFactura = New System.Windows.Forms.Label
        Me.lblPorcentajeIVA = New System.Windows.Forms.Label
        Me.tpRelaciones = New System.Windows.Forms.TabPage
        Me.btnRevisionesRelaciones = New System.Windows.Forms.Button
        Me.btnPaso3 = New System.Windows.Forms.Button
        Me.btnGuardarRelacionYCerrar = New System.Windows.Forms.Button
        Me.btnCancelarRelacion = New System.Windows.Forms.Button
        Me.btnGuardarRelacion = New System.Windows.Forms.Button
        Me.btnEliminarRelacion = New System.Windows.Forms.Button
        Me.btnBuscarRelacion = New System.Windows.Forms.Button
        Me.lblItemsYaRelacionados = New System.Windows.Forms.Label
        Me.dgvDetalleSinRelacion = New System.Windows.Forms.DataGridView
        Me.dgvDetalleConRelacion = New System.Windows.Forms.DataGridView
        Me.lblItemsSinRelacion = New System.Windows.Forms.Label
        Me.cmbTipoRelacion = New System.Windows.Forms.ComboBox
        Me.tpPagos = New System.Windows.Forms.TabPage
        Me.btnRevisionesPagos = New System.Windows.Forms.Button
        Me.btnInsertarPago = New System.Windows.Forms.Button
        Me.lblRestanteAPagar = New System.Windows.Forms.Label
        Me.lblSumaPagos = New System.Windows.Forms.Label
        Me.txtMontoRestante = New System.Windows.Forms.TextBox
        Me.txtSumaPagos = New System.Windows.Forms.TextBox
        Me.lblPagos = New System.Windows.Forms.Label
        Me.dgvPagos = New System.Windows.Forms.DataGridView
        Me.btnEliminarPago = New System.Windows.Forms.Button
        Me.btnAgregarPago = New System.Windows.Forms.Button
        Me.btnGuardarPagoYCerrar = New System.Windows.Forms.Button
        Me.btnCancelarPago = New System.Windows.Forms.Button
        Me.btnGuardarPago = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tcFacturas.SuspendLayout()
        Me.tpFactura.SuspendLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpRelaciones.SuspendLayout()
        CType(Me.dgvDetalleSinRelacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDetalleConRelacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPagos.SuspendLayout()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tcFacturas
        '
        Me.tcFacturas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcFacturas.Controls.Add(Me.tpFactura)
        Me.tcFacturas.Controls.Add(Me.tpRelaciones)
        Me.tcFacturas.Controls.Add(Me.tpPagos)
        Me.tcFacturas.Location = New System.Drawing.Point(3, 4)
        Me.tcFacturas.Name = "tcFacturas"
        Me.tcFacturas.SelectedIndex = 0
        Me.tcFacturas.Size = New System.Drawing.Size(579, 633)
        Me.tcFacturas.TabIndex = 21
        '
        'tpFactura
        '
        Me.tpFactura.Controls.Add(Me.btnRevisiones)
        Me.tpFactura.Controls.Add(Me.btnDescuentos)
        Me.tpFactura.Controls.Add(Me.txtSubtotal)
        Me.tpFactura.Controls.Add(Me.lblDescuento)
        Me.tpFactura.Controls.Add(Me.lblDescripcionFactura)
        Me.tpFactura.Controls.Add(Me.txtDescripcionFactura)
        Me.tpFactura.Controls.Add(Me.btnTipoFactura)
        Me.tpFactura.Controls.Add(Me.btnGuardarDatosFacturaYCerrar)
        Me.tpFactura.Controls.Add(Me.btnCancelarFactura)
        Me.tpFactura.Controls.Add(Me.btnGuardarDatosFactura)
        Me.tpFactura.Controls.Add(Me.lblTotal)
        Me.tpFactura.Controls.Add(Me.lblIVA)
        Me.tpFactura.Controls.Add(Me.lblSubtotal)
        Me.tpFactura.Controls.Add(Me.txtTotal)
        Me.tpFactura.Controls.Add(Me.txtIVA)
        Me.tpFactura.Controls.Add(Me.txtSubtotalTrasDescuentos)
        Me.tpFactura.Controls.Add(Me.btnPaso2)
        Me.tpFactura.Controls.Add(Me.lblPorcentaje)
        Me.tpFactura.Controls.Add(Me.btnInsertarInsumo)
        Me.tpFactura.Controls.Add(Me.btnEliminarInsumo)
        Me.tpFactura.Controls.Add(Me.btnNuevoInsumo)
        Me.tpFactura.Controls.Add(Me.btnProveedor)
        Me.tpFactura.Controls.Add(Me.btnPersona)
        Me.tpFactura.Controls.Add(Me.lblNombreProveedor)
        Me.tpFactura.Controls.Add(Me.lblPersona)
        Me.tpFactura.Controls.Add(Me.txtFolioFacturaProveedor)
        Me.tpFactura.Controls.Add(Me.txtPersona)
        Me.tpFactura.Controls.Add(Me.lblFolioFactura)
        Me.tpFactura.Controls.Add(Me.cmbTipoFactura)
        Me.tpFactura.Controls.Add(Me.txtLugar)
        Me.tpFactura.Controls.Add(Me.dgvDetalle)
        Me.tpFactura.Controls.Add(Me.txtProveedor)
        Me.tpFactura.Controls.Add(Me.txtPorcentajeIVA)
        Me.tpFactura.Controls.Add(Me.lblDetalleFactura)
        Me.tpFactura.Controls.Add(Me.lblTipoFactura)
        Me.tpFactura.Controls.Add(Me.dtFechaFactura)
        Me.tpFactura.Controls.Add(Me.lblLugar)
        Me.tpFactura.Controls.Add(Me.lblFechaFactura)
        Me.tpFactura.Controls.Add(Me.lblPorcentajeIVA)
        Me.tpFactura.Location = New System.Drawing.Point(4, 22)
        Me.tpFactura.Name = "tpFactura"
        Me.tpFactura.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFactura.Size = New System.Drawing.Size(571, 607)
        Me.tpFactura.TabIndex = 0
        Me.tpFactura.Text = "Factura"
        Me.tpFactura.UseVisualStyleBackColor = True
        '
        'btnRevisiones
        '
        Me.btnRevisiones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisiones.Enabled = False
        Me.btnRevisiones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisiones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisiones.Location = New System.Drawing.Point(16, 473)
        Me.btnRevisiones.Name = "btnRevisiones"
        Me.btnRevisiones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisiones.TabIndex = 19
        Me.btnRevisiones.Text = "Revisiones"
        Me.btnRevisiones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisiones.UseVisualStyleBackColor = True
        '
        'btnDescuentos
        '
        Me.btnDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDescuentos.Enabled = False
        Me.btnDescuentos.Image = Global.Oversight.My.Resources.Resources.percent12x12
        Me.btnDescuentos.Location = New System.Drawing.Point(266, 479)
        Me.btnDescuentos.Name = "btnDescuentos"
        Me.btnDescuentos.Size = New System.Drawing.Size(27, 23)
        Me.btnDescuentos.TabIndex = 15
        Me.btnDescuentos.UseVisualStyleBackColor = True
        '
        'txtSubtotal
        '
        Me.txtSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubtotal.Enabled = False
        Me.txtSubtotal.Location = New System.Drawing.Point(431, 455)
        Me.txtSubtotal.MaxLength = 20
        Me.txtSubtotal.Name = "txtSubtotal"
        Me.txtSubtotal.Size = New System.Drawing.Size(100, 20)
        Me.txtSubtotal.TabIndex = 0
        Me.txtSubtotal.TabStop = False
        '
        'lblDescuento
        '
        Me.lblDescuento.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDescuento.AutoSize = True
        Me.lblDescuento.Location = New System.Drawing.Point(299, 484)
        Me.lblDescuento.Name = "lblDescuento"
        Me.lblDescuento.Size = New System.Drawing.Size(126, 13)
        Me.lblDescuento.TabIndex = 97
        Me.lblDescuento.Text = "Subtotal tras Descuentos"
        '
        'lblDescripcionFactura
        '
        Me.lblDescripcionFactura.AutoSize = True
        Me.lblDescripcionFactura.Location = New System.Drawing.Point(13, 141)
        Me.lblDescripcionFactura.Name = "lblDescripcionFactura"
        Me.lblDescripcionFactura.Size = New System.Drawing.Size(131, 13)
        Me.lblDescripcionFactura.TabIndex = 96
        Me.lblDescripcionFactura.Text = "Descripción de la Factura:"
        '
        'txtDescripcionFactura
        '
        Me.txtDescripcionFactura.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionFactura.Location = New System.Drawing.Point(170, 138)
        Me.txtDescripcionFactura.MaxLength = 500
        Me.txtDescripcionFactura.Name = "txtDescripcionFactura"
        Me.txtDescripcionFactura.Size = New System.Drawing.Size(361, 20)
        Me.txtDescripcionFactura.TabIndex = 6
        '
        'btnTipoFactura
        '
        Me.btnTipoFactura.Enabled = False
        Me.btnTipoFactura.Image = Global.Oversight.My.Resources.Resources.invoiceFinance12x12
        Me.btnTipoFactura.Location = New System.Drawing.Point(376, 106)
        Me.btnTipoFactura.Name = "btnTipoFactura"
        Me.btnTipoFactura.Size = New System.Drawing.Size(28, 23)
        Me.btnTipoFactura.TabIndex = 5
        Me.btnTipoFactura.UseVisualStyleBackColor = True
        '
        'btnGuardarDatosFacturaYCerrar
        '
        Me.btnGuardarDatosFacturaYCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarDatosFacturaYCerrar.Enabled = False
        Me.btnGuardarDatosFacturaYCerrar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnGuardarDatosFacturaYCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarDatosFacturaYCerrar.Location = New System.Drawing.Point(279, 564)
        Me.btnGuardarDatosFacturaYCerrar.Name = "btnGuardarDatosFacturaYCerrar"
        Me.btnGuardarDatosFacturaYCerrar.Size = New System.Drawing.Size(168, 34)
        Me.btnGuardarDatosFacturaYCerrar.TabIndex = 18
        Me.btnGuardarDatosFacturaYCerrar.Text = "G&uardar Factura y Cerrar"
        Me.btnGuardarDatosFacturaYCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarDatosFacturaYCerrar.UseVisualStyleBackColor = True
        '
        'btnCancelarFactura
        '
        Me.btnCancelarFactura.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarFactura.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarFactura.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarFactura.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarFactura.Location = New System.Drawing.Point(54, 564)
        Me.btnCancelarFactura.Name = "btnCancelarFactura"
        Me.btnCancelarFactura.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelarFactura.TabIndex = 16
        Me.btnCancelarFactura.Text = "&Cancelar"
        Me.btnCancelarFactura.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarFactura.UseVisualStyleBackColor = True
        '
        'btnGuardarDatosFactura
        '
        Me.btnGuardarDatosFactura.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarDatosFactura.Enabled = False
        Me.btnGuardarDatosFactura.Image = Global.Oversight.My.Resources.Resources.invoiceFinance24x24
        Me.btnGuardarDatosFactura.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarDatosFactura.Location = New System.Drawing.Point(149, 564)
        Me.btnGuardarDatosFactura.Name = "btnGuardarDatosFactura"
        Me.btnGuardarDatosFactura.Size = New System.Drawing.Size(124, 34)
        Me.btnGuardarDatosFactura.TabIndex = 17
        Me.btnGuardarDatosFactura.Text = "&Guardar Factura"
        Me.btnGuardarDatosFactura.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarDatosFactura.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(394, 536)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(31, 13)
        Me.lblTotal.TabIndex = 94
        Me.lblTotal.Text = "Total"
        '
        'lblIVA
        '
        Me.lblIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIVA.AutoSize = True
        Me.lblIVA.Location = New System.Drawing.Point(401, 510)
        Me.lblIVA.Name = "lblIVA"
        Me.lblIVA.Size = New System.Drawing.Size(24, 13)
        Me.lblIVA.TabIndex = 93
        Me.lblIVA.Text = "IVA"
        '
        'lblSubtotal
        '
        Me.lblSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubtotal.AutoSize = True
        Me.lblSubtotal.Location = New System.Drawing.Point(379, 458)
        Me.lblSubtotal.Name = "lblSubtotal"
        Me.lblSubtotal.Size = New System.Drawing.Size(46, 13)
        Me.lblSubtotal.TabIndex = 92
        Me.lblSubtotal.Text = "Subtotal"
        '
        'txtTotal
        '
        Me.txtTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(431, 533)
        Me.txtTotal.MaxLength = 20
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(100, 20)
        Me.txtTotal.TabIndex = 0
        Me.txtTotal.TabStop = False
        '
        'txtIVA
        '
        Me.txtIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIVA.Enabled = False
        Me.txtIVA.Location = New System.Drawing.Point(431, 507)
        Me.txtIVA.MaxLength = 20
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(100, 20)
        Me.txtIVA.TabIndex = 0
        Me.txtIVA.TabStop = False
        '
        'txtSubtotalTrasDescuentos
        '
        Me.txtSubtotalTrasDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubtotalTrasDescuentos.Enabled = False
        Me.txtSubtotalTrasDescuentos.Location = New System.Drawing.Point(431, 481)
        Me.txtSubtotalTrasDescuentos.MaxLength = 20
        Me.txtSubtotalTrasDescuentos.Name = "txtSubtotalTrasDescuentos"
        Me.txtSubtotalTrasDescuentos.Size = New System.Drawing.Size(100, 20)
        Me.txtSubtotalTrasDescuentos.TabIndex = 0
        Me.txtSubtotalTrasDescuentos.TabStop = False
        '
        'btnPaso2
        '
        Me.btnPaso2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPaso2.Enabled = False
        Me.btnPaso2.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso2.Location = New System.Drawing.Point(454, 564)
        Me.btnPaso2.Name = "btnPaso2"
        Me.btnPaso2.Size = New System.Drawing.Size(111, 34)
        Me.btnPaso2.TabIndex = 20
        Me.btnPaso2.Text = "&Siguiente Paso"
        Me.btnPaso2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaso2.UseVisualStyleBackColor = True
        '
        'lblPorcentaje
        '
        Me.lblPorcentaje.AutoSize = True
        Me.lblPorcentaje.Location = New System.Drawing.Point(258, 170)
        Me.lblPorcentaje.Name = "lblPorcentaje"
        Me.lblPorcentaje.Size = New System.Drawing.Size(15, 13)
        Me.lblPorcentaje.TabIndex = 87
        Me.lblPorcentaje.Text = "%"
        '
        'btnInsertarInsumo
        '
        Me.btnInsertarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarInsumo.Enabled = False
        Me.btnInsertarInsumo.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarInsumo.Location = New System.Drawing.Point(537, 329)
        Me.btnInsertarInsumo.Name = "btnInsertarInsumo"
        Me.btnInsertarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarInsumo.TabIndex = 13
        Me.btnInsertarInsumo.UseVisualStyleBackColor = True
        '
        'btnEliminarInsumo
        '
        Me.btnEliminarInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarInsumo.Enabled = False
        Me.btnEliminarInsumo.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarInsumo.Location = New System.Drawing.Point(537, 358)
        Me.btnEliminarInsumo.Name = "btnEliminarInsumo"
        Me.btnEliminarInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarInsumo.TabIndex = 14
        Me.btnEliminarInsumo.UseVisualStyleBackColor = True
        '
        'btnNuevoInsumo
        '
        Me.btnNuevoInsumo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoInsumo.Enabled = False
        Me.btnNuevoInsumo.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoInsumo.Location = New System.Drawing.Point(537, 300)
        Me.btnNuevoInsumo.Name = "btnNuevoInsumo"
        Me.btnNuevoInsumo.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoInsumo.TabIndex = 12
        Me.btnNuevoInsumo.UseVisualStyleBackColor = True
        '
        'btnProveedor
        '
        Me.btnProveedor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProveedor.Enabled = False
        Me.btnProveedor.Image = Global.Oversight.My.Resources.Resources.empresa12x12
        Me.btnProveedor.Location = New System.Drawing.Point(396, 15)
        Me.btnProveedor.Name = "btnProveedor"
        Me.btnProveedor.Size = New System.Drawing.Size(27, 23)
        Me.btnProveedor.TabIndex = 1
        Me.btnProveedor.UseVisualStyleBackColor = True
        '
        'btnPersona
        '
        Me.btnPersona.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersona.Enabled = False
        Me.btnPersona.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersona.Location = New System.Drawing.Point(390, 245)
        Me.btnPersona.Name = "btnPersona"
        Me.btnPersona.Size = New System.Drawing.Size(27, 23)
        Me.btnPersona.TabIndex = 10
        Me.btnPersona.UseVisualStyleBackColor = True
        '
        'lblNombreProveedor
        '
        Me.lblNombreProveedor.AutoSize = True
        Me.lblNombreProveedor.Location = New System.Drawing.Point(13, 20)
        Me.lblNombreProveedor.MaximumSize = New System.Drawing.Size(70, 0)
        Me.lblNombreProveedor.Name = "lblNombreProveedor"
        Me.lblNombreProveedor.Size = New System.Drawing.Size(59, 13)
        Me.lblNombreProveedor.TabIndex = 1
        Me.lblNombreProveedor.Text = "Proveedor:"
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(13, 250)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(151, 13)
        Me.lblPersona.TabIndex = 81
        Me.lblPersona.Text = "Persona que recibió la factura:"
        '
        'txtFolioFacturaProveedor
        '
        Me.txtFolioFacturaProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFolioFacturaProveedor.Location = New System.Drawing.Point(170, 80)
        Me.txtFolioFacturaProveedor.MaxLength = 15
        Me.txtFolioFacturaProveedor.Name = "txtFolioFacturaProveedor"
        Me.txtFolioFacturaProveedor.Size = New System.Drawing.Size(186, 20)
        Me.txtFolioFacturaProveedor.TabIndex = 3
        '
        'txtPersona
        '
        Me.txtPersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPersona.Enabled = False
        Me.txtPersona.Location = New System.Drawing.Point(170, 247)
        Me.txtPersona.Name = "txtPersona"
        Me.txtPersona.Size = New System.Drawing.Size(214, 20)
        Me.txtPersona.TabIndex = 0
        '
        'lblFolioFactura
        '
        Me.lblFolioFactura.AutoSize = True
        Me.lblFolioFactura.Location = New System.Drawing.Point(13, 83)
        Me.lblFolioFactura.Name = "lblFolioFactura"
        Me.lblFolioFactura.Size = New System.Drawing.Size(71, 13)
        Me.lblFolioFactura.TabIndex = 4
        Me.lblFolioFactura.Text = "Folio Factura:"
        '
        'cmbTipoFactura
        '
        Me.cmbTipoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoFactura.FormattingEnabled = True
        Me.cmbTipoFactura.Location = New System.Drawing.Point(170, 108)
        Me.cmbTipoFactura.Name = "cmbTipoFactura"
        Me.cmbTipoFactura.Size = New System.Drawing.Size(200, 21)
        Me.cmbTipoFactura.TabIndex = 4
        '
        'txtLugar
        '
        Me.txtLugar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLugar.Location = New System.Drawing.Point(170, 196)
        Me.txtLugar.MaxLength = 1000
        Me.txtLugar.Multiline = True
        Me.txtLugar.Name = "txtLugar"
        Me.txtLugar.Size = New System.Drawing.Size(214, 43)
        Me.txtLugar.TabIndex = 8
        Me.txtLugar.Text = "Tuxtla Gutierrez, Chiapas"
        '
        'dgvDetalle
        '
        Me.dgvDetalle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalle.Enabled = False
        Me.dgvDetalle.Location = New System.Drawing.Point(16, 300)
        Me.dgvDetalle.MultiSelect = False
        Me.dgvDetalle.Name = "dgvDetalle"
        Me.dgvDetalle.RowHeadersVisible = False
        Me.dgvDetalle.Size = New System.Drawing.Size(515, 149)
        Me.dgvDetalle.TabIndex = 11
        '
        'txtProveedor
        '
        Me.txtProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProveedor.Enabled = False
        Me.txtProveedor.Location = New System.Drawing.Point(170, 17)
        Me.txtProveedor.Name = "txtProveedor"
        Me.txtProveedor.Size = New System.Drawing.Size(220, 20)
        Me.txtProveedor.TabIndex = 0
        Me.txtProveedor.TabStop = False
        '
        'txtPorcentajeIVA
        '
        Me.txtPorcentajeIVA.Location = New System.Drawing.Point(170, 167)
        Me.txtPorcentajeIVA.MaxLength = 20
        Me.txtPorcentajeIVA.Name = "txtPorcentajeIVA"
        Me.txtPorcentajeIVA.Size = New System.Drawing.Size(82, 20)
        Me.txtPorcentajeIVA.TabIndex = 7
        Me.txtPorcentajeIVA.Text = "16"
        '
        'lblDetalleFactura
        '
        Me.lblDetalleFactura.AutoSize = True
        Me.lblDetalleFactura.Location = New System.Drawing.Point(13, 279)
        Me.lblDetalleFactura.Name = "lblDetalleFactura"
        Me.lblDetalleFactura.Size = New System.Drawing.Size(97, 13)
        Me.lblDetalleFactura.TabIndex = 68
        Me.lblDetalleFactura.Text = "Detalle de Factura:"
        '
        'lblTipoFactura
        '
        Me.lblTipoFactura.AutoSize = True
        Me.lblTipoFactura.Location = New System.Drawing.Point(13, 111)
        Me.lblTipoFactura.Name = "lblTipoFactura"
        Me.lblTipoFactura.Size = New System.Drawing.Size(85, 13)
        Me.lblTipoFactura.TabIndex = 6
        Me.lblTipoFactura.Text = "Tipo de Factura:"
        '
        'dtFechaFactura
        '
        Me.dtFechaFactura.Location = New System.Drawing.Point(170, 50)
        Me.dtFechaFactura.Name = "dtFechaFactura"
        Me.dtFechaFactura.Size = New System.Drawing.Size(200, 20)
        Me.dtFechaFactura.TabIndex = 2
        '
        'lblLugar
        '
        Me.lblLugar.AutoSize = True
        Me.lblLugar.Location = New System.Drawing.Point(13, 211)
        Me.lblLugar.Name = "lblLugar"
        Me.lblLugar.Size = New System.Drawing.Size(106, 13)
        Me.lblLugar.TabIndex = 75
        Me.lblLugar.Text = "Ubicación del Gasto:"
        '
        'lblFechaFactura
        '
        Me.lblFechaFactura.AutoSize = True
        Me.lblFechaFactura.Location = New System.Drawing.Point(13, 56)
        Me.lblFechaFactura.Name = "lblFechaFactura"
        Me.lblFechaFactura.Size = New System.Drawing.Size(94, 13)
        Me.lblFechaFactura.TabIndex = 2
        Me.lblFechaFactura.Text = "Fecha de Factura:"
        '
        'lblPorcentajeIVA
        '
        Me.lblPorcentajeIVA.AutoSize = True
        Me.lblPorcentajeIVA.Location = New System.Drawing.Point(13, 170)
        Me.lblPorcentajeIVA.Name = "lblPorcentajeIVA"
        Me.lblPorcentajeIVA.Size = New System.Drawing.Size(27, 13)
        Me.lblPorcentajeIVA.TabIndex = 76
        Me.lblPorcentajeIVA.Text = "IVA:"
        '
        'tpRelaciones
        '
        Me.tpRelaciones.Controls.Add(Me.btnRevisionesRelaciones)
        Me.tpRelaciones.Controls.Add(Me.btnPaso3)
        Me.tpRelaciones.Controls.Add(Me.btnGuardarRelacionYCerrar)
        Me.tpRelaciones.Controls.Add(Me.btnCancelarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnGuardarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnEliminarRelacion)
        Me.tpRelaciones.Controls.Add(Me.btnBuscarRelacion)
        Me.tpRelaciones.Controls.Add(Me.lblItemsYaRelacionados)
        Me.tpRelaciones.Controls.Add(Me.dgvDetalleSinRelacion)
        Me.tpRelaciones.Controls.Add(Me.dgvDetalleConRelacion)
        Me.tpRelaciones.Controls.Add(Me.lblItemsSinRelacion)
        Me.tpRelaciones.Controls.Add(Me.cmbTipoRelacion)
        Me.tpRelaciones.Location = New System.Drawing.Point(4, 22)
        Me.tpRelaciones.Name = "tpRelaciones"
        Me.tpRelaciones.Padding = New System.Windows.Forms.Padding(3)
        Me.tpRelaciones.Size = New System.Drawing.Size(571, 607)
        Me.tpRelaciones.TabIndex = 1
        Me.tpRelaciones.Text = "Relacionar Factura"
        Me.tpRelaciones.UseVisualStyleBackColor = True
        '
        'btnRevisionesRelaciones
        '
        Me.btnRevisionesRelaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRevisionesRelaciones.Enabled = False
        Me.btnRevisionesRelaciones.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisionesRelaciones.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisionesRelaciones.Location = New System.Drawing.Point(415, 209)
        Me.btnRevisionesRelaciones.Name = "btnRevisionesRelaciones"
        Me.btnRevisionesRelaciones.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisionesRelaciones.TabIndex = 31
        Me.btnRevisionesRelaciones.Text = "Revisiones"
        Me.btnRevisionesRelaciones.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisionesRelaciones.UseVisualStyleBackColor = True
        '
        'btnPaso3
        '
        Me.btnPaso3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPaso3.Enabled = False
        Me.btnPaso3.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso3.Location = New System.Drawing.Point(458, 516)
        Me.btnPaso3.Name = "btnPaso3"
        Me.btnPaso3.Size = New System.Drawing.Size(111, 34)
        Me.btnPaso3.TabIndex = 30
        Me.btnPaso3.Text = "S&iguiente Paso"
        Me.btnPaso3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaso3.UseVisualStyleBackColor = True
        '
        'btnGuardarRelacionYCerrar
        '
        Me.btnGuardarRelacionYCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarRelacionYCerrar.Enabled = False
        Me.btnGuardarRelacionYCerrar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnGuardarRelacionYCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarRelacionYCerrar.Location = New System.Drawing.Point(287, 516)
        Me.btnGuardarRelacionYCerrar.Name = "btnGuardarRelacionYCerrar"
        Me.btnGuardarRelacionYCerrar.Size = New System.Drawing.Size(168, 34)
        Me.btnGuardarRelacionYCerrar.TabIndex = 29
        Me.btnGuardarRelacionYCerrar.Text = "G&uardar Factura y Cerrar"
        Me.btnGuardarRelacionYCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarRelacionYCerrar.UseVisualStyleBackColor = True
        '
        'btnCancelarRelacion
        '
        Me.btnCancelarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarRelacion.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarRelacion.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarRelacion.Location = New System.Drawing.Point(62, 516)
        Me.btnCancelarRelacion.Name = "btnCancelarRelacion"
        Me.btnCancelarRelacion.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelarRelacion.TabIndex = 27
        Me.btnCancelarRelacion.Text = "&Cancelar"
        Me.btnCancelarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarRelacion.UseVisualStyleBackColor = True
        '
        'btnGuardarRelacion
        '
        Me.btnGuardarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarRelacion.Enabled = False
        Me.btnGuardarRelacion.Image = Global.Oversight.My.Resources.Resources.invoiceFinance24x24
        Me.btnGuardarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarRelacion.Location = New System.Drawing.Point(157, 516)
        Me.btnGuardarRelacion.Name = "btnGuardarRelacion"
        Me.btnGuardarRelacion.Size = New System.Drawing.Size(124, 34)
        Me.btnGuardarRelacion.TabIndex = 28
        Me.btnGuardarRelacion.Text = "&Guardar Factura"
        Me.btnGuardarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarRelacion.UseVisualStyleBackColor = True
        '
        'btnEliminarRelacion
        '
        Me.btnEliminarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarRelacion.Enabled = False
        Me.btnEliminarRelacion.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarRelacion.Location = New System.Drawing.Point(532, 263)
        Me.btnEliminarRelacion.Name = "btnEliminarRelacion"
        Me.btnEliminarRelacion.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarRelacion.TabIndex = 26
        Me.btnEliminarRelacion.UseVisualStyleBackColor = True
        '
        'btnBuscarRelacion
        '
        Me.btnBuscarRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBuscarRelacion.Enabled = False
        Me.btnBuscarRelacion.Image = Global.Oversight.My.Resources.Resources.search12x12
        Me.btnBuscarRelacion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBuscarRelacion.Location = New System.Drawing.Point(217, 207)
        Me.btnBuscarRelacion.Name = "btnBuscarRelacion"
        Me.btnBuscarRelacion.Size = New System.Drawing.Size(130, 23)
        Me.btnBuscarRelacion.TabIndex = 24
        Me.btnBuscarRelacion.Text = "Buscar y Relacionar"
        Me.btnBuscarRelacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBuscarRelacion.UseVisualStyleBackColor = True
        '
        'lblItemsYaRelacionados
        '
        Me.lblItemsYaRelacionados.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblItemsYaRelacionados.AutoSize = True
        Me.lblItemsYaRelacionados.Location = New System.Drawing.Point(7, 247)
        Me.lblItemsYaRelacionados.Name = "lblItemsYaRelacionados"
        Me.lblItemsYaRelacionados.Size = New System.Drawing.Size(115, 13)
        Me.lblItemsYaRelacionados.TabIndex = 17
        Me.lblItemsYaRelacionados.Text = "Items YA relacionados:"
        '
        'dgvDetalleSinRelacion
        '
        Me.dgvDetalleSinRelacion.AllowUserToAddRows = False
        Me.dgvDetalleSinRelacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDetalleSinRelacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalleSinRelacion.Location = New System.Drawing.Point(6, 31)
        Me.dgvDetalleSinRelacion.MultiSelect = False
        Me.dgvDetalleSinRelacion.Name = "dgvDetalleSinRelacion"
        Me.dgvDetalleSinRelacion.RowHeadersVisible = False
        Me.dgvDetalleSinRelacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDetalleSinRelacion.Size = New System.Drawing.Size(520, 160)
        Me.dgvDetalleSinRelacion.TabIndex = 22
        '
        'dgvDetalleConRelacion
        '
        Me.dgvDetalleConRelacion.AllowUserToAddRows = False
        Me.dgvDetalleConRelacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDetalleConRelacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalleConRelacion.Location = New System.Drawing.Point(6, 263)
        Me.dgvDetalleConRelacion.MultiSelect = False
        Me.dgvDetalleConRelacion.Name = "dgvDetalleConRelacion"
        Me.dgvDetalleConRelacion.RowHeadersVisible = False
        Me.dgvDetalleConRelacion.Size = New System.Drawing.Size(520, 160)
        Me.dgvDetalleConRelacion.TabIndex = 25
        Me.dgvDetalleConRelacion.Visible = False
        '
        'lblItemsSinRelacion
        '
        Me.lblItemsSinRelacion.AutoSize = True
        Me.lblItemsSinRelacion.Location = New System.Drawing.Point(7, 15)
        Me.lblItemsSinRelacion.Name = "lblItemsSinRelacion"
        Me.lblItemsSinRelacion.Size = New System.Drawing.Size(204, 13)
        Me.lblItemsSinRelacion.TabIndex = 11
        Me.lblItemsSinRelacion.Text = "Items de la factura SIN relación (todavía):"
        '
        'cmbTipoRelacion
        '
        Me.cmbTipoRelacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbTipoRelacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoRelacion.FormattingEnabled = True
        Me.cmbTipoRelacion.Items.AddRange(New Object() {"Relacionar a Activo", "Relacionar a Proyecto"})
        Me.cmbTipoRelacion.Location = New System.Drawing.Point(6, 209)
        Me.cmbTipoRelacion.Name = "cmbTipoRelacion"
        Me.cmbTipoRelacion.Size = New System.Drawing.Size(205, 21)
        Me.cmbTipoRelacion.TabIndex = 23
        '
        'tpPagos
        '
        Me.tpPagos.Controls.Add(Me.btnRevisionesPagos)
        Me.tpPagos.Controls.Add(Me.btnInsertarPago)
        Me.tpPagos.Controls.Add(Me.lblRestanteAPagar)
        Me.tpPagos.Controls.Add(Me.lblSumaPagos)
        Me.tpPagos.Controls.Add(Me.txtMontoRestante)
        Me.tpPagos.Controls.Add(Me.txtSumaPagos)
        Me.tpPagos.Controls.Add(Me.lblPagos)
        Me.tpPagos.Controls.Add(Me.dgvPagos)
        Me.tpPagos.Controls.Add(Me.btnEliminarPago)
        Me.tpPagos.Controls.Add(Me.btnAgregarPago)
        Me.tpPagos.Controls.Add(Me.btnGuardarPagoYCerrar)
        Me.tpPagos.Controls.Add(Me.btnCancelarPago)
        Me.tpPagos.Controls.Add(Me.btnGuardarPago)
        Me.tpPagos.Location = New System.Drawing.Point(4, 22)
        Me.tpPagos.Name = "tpPagos"
        Me.tpPagos.Size = New System.Drawing.Size(571, 607)
        Me.tpPagos.TabIndex = 2
        Me.tpPagos.Text = "Pagos aplicables a la Factura"
        Me.tpPagos.UseVisualStyleBackColor = True
        '
        'btnRevisionesPagos
        '
        Me.btnRevisionesPagos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRevisionesPagos.Enabled = False
        Me.btnRevisionesPagos.Image = Global.Oversight.My.Resources.Resources.yes24x24
        Me.btnRevisionesPagos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevisionesPagos.Location = New System.Drawing.Point(6, 328)
        Me.btnRevisionesPagos.Name = "btnRevisionesPagos"
        Me.btnRevisionesPagos.Size = New System.Drawing.Size(111, 34)
        Me.btnRevisionesPagos.TabIndex = 110
        Me.btnRevisionesPagos.Text = "Revisiones"
        Me.btnRevisionesPagos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevisionesPagos.UseVisualStyleBackColor = True
        '
        'btnInsertarPago
        '
        Me.btnInsertarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarPago.Enabled = False
        Me.btnInsertarPago.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarPago.Location = New System.Drawing.Point(541, 60)
        Me.btnInsertarPago.Name = "btnInsertarPago"
        Me.btnInsertarPago.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarPago.TabIndex = 33
        Me.btnInsertarPago.UseVisualStyleBackColor = True
        '
        'lblRestanteAPagar
        '
        Me.lblRestanteAPagar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRestanteAPagar.AutoSize = True
        Me.lblRestanteAPagar.Location = New System.Drawing.Point(346, 226)
        Me.lblRestanteAPagar.Name = "lblRestanteAPagar"
        Me.lblRestanteAPagar.Size = New System.Drawing.Size(83, 13)
        Me.lblRestanteAPagar.TabIndex = 109
        Me.lblRestanteAPagar.Text = "Monto Restante"
        Me.lblRestanteAPagar.Visible = False
        '
        'lblSumaPagos
        '
        Me.lblSumaPagos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSumaPagos.AutoSize = True
        Me.lblSumaPagos.Location = New System.Drawing.Point(337, 200)
        Me.lblSumaPagos.Name = "lblSumaPagos"
        Me.lblSumaPagos.Size = New System.Drawing.Size(82, 13)
        Me.lblSumaPagos.TabIndex = 108
        Me.lblSumaPagos.Text = "Suma de Pagos"
        Me.lblSumaPagos.Visible = False
        '
        'txtMontoRestante
        '
        Me.txtMontoRestante.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMontoRestante.Enabled = False
        Me.txtMontoRestante.Location = New System.Drawing.Point(435, 223)
        Me.txtMontoRestante.Name = "txtMontoRestante"
        Me.txtMontoRestante.Size = New System.Drawing.Size(100, 20)
        Me.txtMontoRestante.TabIndex = 0
        Me.txtMontoRestante.TabStop = False
        Me.txtMontoRestante.Visible = False
        '
        'txtSumaPagos
        '
        Me.txtSumaPagos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSumaPagos.Enabled = False
        Me.txtSumaPagos.Location = New System.Drawing.Point(435, 197)
        Me.txtSumaPagos.Name = "txtSumaPagos"
        Me.txtSumaPagos.Size = New System.Drawing.Size(100, 20)
        Me.txtSumaPagos.TabIndex = 0
        Me.txtSumaPagos.TabStop = False
        Me.txtSumaPagos.Visible = False
        '
        'lblPagos
        '
        Me.lblPagos.AutoSize = True
        Me.lblPagos.Location = New System.Drawing.Point(7, 11)
        Me.lblPagos.Name = "lblPagos"
        Me.lblPagos.Size = New System.Drawing.Size(158, 13)
        Me.lblPagos.TabIndex = 102
        Me.lblPagos.Text = "Pagos aplicables a esta factura:"
        '
        'dgvPagos
        '
        Me.dgvPagos.AllowUserToAddRows = False
        Me.dgvPagos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPagos.Enabled = False
        Me.dgvPagos.Location = New System.Drawing.Point(6, 31)
        Me.dgvPagos.MultiSelect = False
        Me.dgvPagos.Name = "dgvPagos"
        Me.dgvPagos.RowHeadersVisible = False
        Me.dgvPagos.Size = New System.Drawing.Size(529, 160)
        Me.dgvPagos.TabIndex = 31
        '
        'btnEliminarPago
        '
        Me.btnEliminarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarPago.Enabled = False
        Me.btnEliminarPago.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarPago.Location = New System.Drawing.Point(541, 89)
        Me.btnEliminarPago.Name = "btnEliminarPago"
        Me.btnEliminarPago.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarPago.TabIndex = 34
        Me.btnEliminarPago.UseVisualStyleBackColor = True
        '
        'btnAgregarPago
        '
        Me.btnAgregarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAgregarPago.Enabled = False
        Me.btnAgregarPago.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnAgregarPago.Location = New System.Drawing.Point(541, 31)
        Me.btnAgregarPago.Name = "btnAgregarPago"
        Me.btnAgregarPago.Size = New System.Drawing.Size(28, 23)
        Me.btnAgregarPago.TabIndex = 32
        Me.btnAgregarPago.UseVisualStyleBackColor = True
        '
        'btnGuardarPagoYCerrar
        '
        Me.btnGuardarPagoYCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarPagoYCerrar.Enabled = False
        Me.btnGuardarPagoYCerrar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnGuardarPagoYCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarPagoYCerrar.Location = New System.Drawing.Point(367, 328)
        Me.btnGuardarPagoYCerrar.Name = "btnGuardarPagoYCerrar"
        Me.btnGuardarPagoYCerrar.Size = New System.Drawing.Size(168, 34)
        Me.btnGuardarPagoYCerrar.TabIndex = 37
        Me.btnGuardarPagoYCerrar.Text = "G&uardar Factura y Cerrar"
        Me.btnGuardarPagoYCerrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarPagoYCerrar.UseVisualStyleBackColor = True
        '
        'btnCancelarPago
        '
        Me.btnCancelarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarPago.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelarPago.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelarPago.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelarPago.Location = New System.Drawing.Point(142, 328)
        Me.btnCancelarPago.Name = "btnCancelarPago"
        Me.btnCancelarPago.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelarPago.TabIndex = 35
        Me.btnCancelarPago.Text = "&Cancelar"
        Me.btnCancelarPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelarPago.UseVisualStyleBackColor = True
        '
        'btnGuardarPago
        '
        Me.btnGuardarPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarPago.Enabled = False
        Me.btnGuardarPago.Image = Global.Oversight.My.Resources.Resources.invoiceFinance24x24
        Me.btnGuardarPago.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarPago.Location = New System.Drawing.Point(237, 328)
        Me.btnGuardarPago.Name = "btnGuardarPago"
        Me.btnGuardarPago.Size = New System.Drawing.Size(124, 34)
        Me.btnGuardarPago.TabIndex = 36
        Me.btnGuardarPago.Text = "&Guardar Factura"
        Me.btnGuardarPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardarPago.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarFacturaProveedor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 639)
        Me.Controls.Add(Me.tcFacturas)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarFacturaProveedor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Factura"
        Me.tcFacturas.ResumeLayout(False)
        Me.tpFactura.ResumeLayout(False)
        Me.tpFactura.PerformLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpRelaciones.ResumeLayout(False)
        Me.tpRelaciones.PerformLayout()
        CType(Me.dgvDetalleSinRelacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDetalleConRelacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPagos.ResumeLayout(False)
        Me.tpPagos.PerformLayout()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcFacturas As System.Windows.Forms.TabControl
    Friend WithEvents tpRelaciones As System.Windows.Forms.TabPage
    Friend WithEvents cmbTipoRelacion As System.Windows.Forms.ComboBox
    Friend WithEvents dgvDetalleSinRelacion As System.Windows.Forms.DataGridView
    Friend WithEvents lblItemsSinRelacion As System.Windows.Forms.Label
    Friend WithEvents dgvDetalleConRelacion As System.Windows.Forms.DataGridView
    Friend WithEvents lblItemsYaRelacionados As System.Windows.Forms.Label
    Friend WithEvents btnBuscarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnEliminarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnGuardarRelacionYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnCancelarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnGuardarRelacion As System.Windows.Forms.Button
    Friend WithEvents btnPaso3 As System.Windows.Forms.Button
    Friend WithEvents tpPagos As System.Windows.Forms.TabPage
    Friend WithEvents btnGuardarPagoYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnCancelarPago As System.Windows.Forms.Button
    Friend WithEvents btnGuardarPago As System.Windows.Forms.Button
    Friend WithEvents lblPagos As System.Windows.Forms.Label
    Friend WithEvents dgvPagos As System.Windows.Forms.DataGridView
    Friend WithEvents btnEliminarPago As System.Windows.Forms.Button
    Friend WithEvents btnAgregarPago As System.Windows.Forms.Button
    Friend WithEvents lblRestanteAPagar As System.Windows.Forms.Label
    Friend WithEvents lblSumaPagos As System.Windows.Forms.Label
    Friend WithEvents txtMontoRestante As System.Windows.Forms.TextBox
    Friend WithEvents txtSumaPagos As System.Windows.Forms.TextBox
    Friend WithEvents btnInsertarPago As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents tpFactura As System.Windows.Forms.TabPage
    Friend WithEvents btnDescuentos As System.Windows.Forms.Button
    Friend WithEvents txtSubtotal As System.Windows.Forms.TextBox
    Friend WithEvents lblDescuento As System.Windows.Forms.Label
    Friend WithEvents lblDescripcionFactura As System.Windows.Forms.Label
    Friend WithEvents txtDescripcionFactura As System.Windows.Forms.TextBox
    Friend WithEvents btnTipoFactura As System.Windows.Forms.Button
    Friend WithEvents btnGuardarDatosFacturaYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnCancelarFactura As System.Windows.Forms.Button
    Friend WithEvents btnGuardarDatosFactura As System.Windows.Forms.Button
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblIVA As System.Windows.Forms.Label
    Friend WithEvents lblSubtotal As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtSubtotalTrasDescuentos As System.Windows.Forms.TextBox
    Friend WithEvents btnPaso2 As System.Windows.Forms.Button
    Friend WithEvents lblPorcentaje As System.Windows.Forms.Label
    Friend WithEvents btnInsertarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnEliminarInsumo As System.Windows.Forms.Button
    Friend WithEvents btnNuevoInsumo As System.Windows.Forms.Button
    Friend WithEvents btnProveedor As System.Windows.Forms.Button
    Friend WithEvents btnPersona As System.Windows.Forms.Button
    Friend WithEvents lblNombreProveedor As System.Windows.Forms.Label
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents txtFolioFacturaProveedor As System.Windows.Forms.TextBox
    Friend WithEvents txtPersona As System.Windows.Forms.TextBox
    Friend WithEvents lblFolioFactura As System.Windows.Forms.Label
    Friend WithEvents cmbTipoFactura As System.Windows.Forms.ComboBox
    Friend WithEvents txtLugar As System.Windows.Forms.TextBox
    Friend WithEvents dgvDetalle As System.Windows.Forms.DataGridView
    Friend WithEvents txtProveedor As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajeIVA As System.Windows.Forms.TextBox
    Friend WithEvents lblDetalleFactura As System.Windows.Forms.Label
    Friend WithEvents lblTipoFactura As System.Windows.Forms.Label
    Friend WithEvents dtFechaFactura As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblLugar As System.Windows.Forms.Label
    Friend WithEvents lblFechaFactura As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIVA As System.Windows.Forms.Label
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesRelaciones As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesPagos As System.Windows.Forms.Button
End Class
