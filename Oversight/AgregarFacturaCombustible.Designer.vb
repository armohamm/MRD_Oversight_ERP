<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarFacturaCombustible
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarFacturaCombustible))
        Me.tcFacturas = New System.Windows.Forms.TabControl
        Me.tpFactura = New System.Windows.Forms.TabPage
        Me.btnRevisiones = New System.Windows.Forms.Button
        Me.txtSubtotal = New System.Windows.Forms.TextBox
        Me.lblDescripcionFactura = New System.Windows.Forms.Label
        Me.txtDescripcionFactura = New System.Windows.Forms.TextBox
        Me.btnGuardarDatosFacturaYCerrar = New System.Windows.Forms.Button
        Me.btnCancelarFactura = New System.Windows.Forms.Button
        Me.btnGuardarDatosFactura = New System.Windows.Forms.Button
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblIVA = New System.Windows.Forms.Label
        Me.lblSubtotal = New System.Windows.Forms.Label
        Me.txtTotal = New System.Windows.Forms.TextBox
        Me.txtIVA = New System.Windows.Forms.TextBox
        Me.btnPaso2 = New System.Windows.Forms.Button
        Me.lblPorcentaje = New System.Windows.Forms.Label
        Me.btnInsertarVale = New System.Windows.Forms.Button
        Me.btnEliminarVale = New System.Windows.Forms.Button
        Me.btnNuevoVale = New System.Windows.Forms.Button
        Me.btnProveedor = New System.Windows.Forms.Button
        Me.btnPersona = New System.Windows.Forms.Button
        Me.lblNombreProveedor = New System.Windows.Forms.Label
        Me.lblPersona = New System.Windows.Forms.Label
        Me.txtFolioFacturaProveedor = New System.Windows.Forms.TextBox
        Me.txtPersona = New System.Windows.Forms.TextBox
        Me.lblFolioFactura = New System.Windows.Forms.Label
        Me.txtLugar = New System.Windows.Forms.TextBox
        Me.dgvDetalle = New System.Windows.Forms.DataGridView
        Me.txtProveedor = New System.Windows.Forms.TextBox
        Me.txtPorcentajeIVA = New System.Windows.Forms.TextBox
        Me.lblDetalleFactura = New System.Windows.Forms.Label
        Me.dtFechaFactura = New System.Windows.Forms.DateTimePicker
        Me.lblLugar = New System.Windows.Forms.Label
        Me.lblFechaFactura = New System.Windows.Forms.Label
        Me.lblPorcentajeIVA = New System.Windows.Forms.Label
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
        Me.tcFacturas.Controls.Add(Me.tpPagos)
        Me.tcFacturas.Location = New System.Drawing.Point(3, 4)
        Me.tcFacturas.Name = "tcFacturas"
        Me.tcFacturas.SelectedIndex = 0
        Me.tcFacturas.Size = New System.Drawing.Size(593, 633)
        Me.tcFacturas.TabIndex = 21
        '
        'tpFactura
        '
        Me.tpFactura.Controls.Add(Me.btnRevisiones)
        Me.tpFactura.Controls.Add(Me.txtSubtotal)
        Me.tpFactura.Controls.Add(Me.lblDescripcionFactura)
        Me.tpFactura.Controls.Add(Me.txtDescripcionFactura)
        Me.tpFactura.Controls.Add(Me.btnGuardarDatosFacturaYCerrar)
        Me.tpFactura.Controls.Add(Me.btnCancelarFactura)
        Me.tpFactura.Controls.Add(Me.btnGuardarDatosFactura)
        Me.tpFactura.Controls.Add(Me.lblTotal)
        Me.tpFactura.Controls.Add(Me.lblIVA)
        Me.tpFactura.Controls.Add(Me.lblSubtotal)
        Me.tpFactura.Controls.Add(Me.txtTotal)
        Me.tpFactura.Controls.Add(Me.txtIVA)
        Me.tpFactura.Controls.Add(Me.btnPaso2)
        Me.tpFactura.Controls.Add(Me.lblPorcentaje)
        Me.tpFactura.Controls.Add(Me.btnInsertarVale)
        Me.tpFactura.Controls.Add(Me.btnEliminarVale)
        Me.tpFactura.Controls.Add(Me.btnNuevoVale)
        Me.tpFactura.Controls.Add(Me.btnProveedor)
        Me.tpFactura.Controls.Add(Me.btnPersona)
        Me.tpFactura.Controls.Add(Me.lblNombreProveedor)
        Me.tpFactura.Controls.Add(Me.lblPersona)
        Me.tpFactura.Controls.Add(Me.txtFolioFacturaProveedor)
        Me.tpFactura.Controls.Add(Me.txtPersona)
        Me.tpFactura.Controls.Add(Me.lblFolioFactura)
        Me.tpFactura.Controls.Add(Me.txtLugar)
        Me.tpFactura.Controls.Add(Me.dgvDetalle)
        Me.tpFactura.Controls.Add(Me.txtProveedor)
        Me.tpFactura.Controls.Add(Me.txtPorcentajeIVA)
        Me.tpFactura.Controls.Add(Me.lblDetalleFactura)
        Me.tpFactura.Controls.Add(Me.dtFechaFactura)
        Me.tpFactura.Controls.Add(Me.lblLugar)
        Me.tpFactura.Controls.Add(Me.lblFechaFactura)
        Me.tpFactura.Controls.Add(Me.lblPorcentajeIVA)
        Me.tpFactura.Location = New System.Drawing.Point(4, 22)
        Me.tpFactura.Name = "tpFactura"
        Me.tpFactura.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFactura.Size = New System.Drawing.Size(585, 607)
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
        'txtSubtotal
        '
        Me.txtSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubtotal.Enabled = False
        Me.txtSubtotal.Location = New System.Drawing.Point(445, 481)
        Me.txtSubtotal.MaxLength = 20
        Me.txtSubtotal.Name = "txtSubtotal"
        Me.txtSubtotal.Size = New System.Drawing.Size(100, 20)
        Me.txtSubtotal.TabIndex = 0
        Me.txtSubtotal.TabStop = False
        '
        'lblDescripcionFactura
        '
        Me.lblDescripcionFactura.AutoSize = True
        Me.lblDescripcionFactura.Location = New System.Drawing.Point(13, 112)
        Me.lblDescripcionFactura.Name = "lblDescripcionFactura"
        Me.lblDescripcionFactura.Size = New System.Drawing.Size(131, 13)
        Me.lblDescripcionFactura.TabIndex = 96
        Me.lblDescripcionFactura.Text = "Descripción de la Factura:"
        '
        'txtDescripcionFactura
        '
        Me.txtDescripcionFactura.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionFactura.Location = New System.Drawing.Point(170, 109)
        Me.txtDescripcionFactura.MaxLength = 500
        Me.txtDescripcionFactura.Name = "txtDescripcionFactura"
        Me.txtDescripcionFactura.Size = New System.Drawing.Size(375, 20)
        Me.txtDescripcionFactura.TabIndex = 6
        '
        'btnGuardarDatosFacturaYCerrar
        '
        Me.btnGuardarDatosFacturaYCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardarDatosFacturaYCerrar.Enabled = False
        Me.btnGuardarDatosFacturaYCerrar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnGuardarDatosFacturaYCerrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardarDatosFacturaYCerrar.Location = New System.Drawing.Point(259, 562)
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
        Me.btnCancelarFactura.Location = New System.Drawing.Point(16, 562)
        Me.btnCancelarFactura.Name = "btnCancelarFactura"
        Me.btnCancelarFactura.Size = New System.Drawing.Size(107, 34)
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
        Me.btnGuardarDatosFactura.Location = New System.Drawing.Point(129, 562)
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
        Me.lblTotal.Location = New System.Drawing.Point(408, 510)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(31, 13)
        Me.lblTotal.TabIndex = 94
        Me.lblTotal.Text = "Total"
        '
        'lblIVA
        '
        Me.lblIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIVA.AutoSize = True
        Me.lblIVA.Location = New System.Drawing.Point(415, 458)
        Me.lblIVA.Name = "lblIVA"
        Me.lblIVA.Size = New System.Drawing.Size(24, 13)
        Me.lblIVA.TabIndex = 93
        Me.lblIVA.Text = "IVA"
        '
        'lblSubtotal
        '
        Me.lblSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubtotal.AutoSize = True
        Me.lblSubtotal.Location = New System.Drawing.Point(393, 484)
        Me.lblSubtotal.Name = "lblSubtotal"
        Me.lblSubtotal.Size = New System.Drawing.Size(46, 13)
        Me.lblSubtotal.TabIndex = 92
        Me.lblSubtotal.Text = "Subtotal"
        '
        'txtTotal
        '
        Me.txtTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(445, 507)
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
        Me.txtIVA.Location = New System.Drawing.Point(445, 455)
        Me.txtIVA.MaxLength = 20
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(100, 20)
        Me.txtIVA.TabIndex = 0
        Me.txtIVA.TabStop = False
        '
        'btnPaso2
        '
        Me.btnPaso2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPaso2.Enabled = False
        Me.btnPaso2.Image = Global.Oversight.My.Resources.Resources.next24x24
        Me.btnPaso2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPaso2.Location = New System.Drawing.Point(434, 562)
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
        Me.lblPorcentaje.Location = New System.Drawing.Point(258, 141)
        Me.lblPorcentaje.Name = "lblPorcentaje"
        Me.lblPorcentaje.Size = New System.Drawing.Size(15, 13)
        Me.lblPorcentaje.TabIndex = 87
        Me.lblPorcentaje.Text = "%"
        '
        'btnInsertarVale
        '
        Me.btnInsertarVale.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarVale.Enabled = False
        Me.btnInsertarVale.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarVale.Location = New System.Drawing.Point(551, 301)
        Me.btnInsertarVale.Name = "btnInsertarVale"
        Me.btnInsertarVale.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarVale.TabIndex = 13
        Me.btnInsertarVale.UseVisualStyleBackColor = True
        '
        'btnEliminarVale
        '
        Me.btnEliminarVale.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarVale.Enabled = False
        Me.btnEliminarVale.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarVale.Location = New System.Drawing.Point(551, 330)
        Me.btnEliminarVale.Name = "btnEliminarVale"
        Me.btnEliminarVale.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarVale.TabIndex = 14
        Me.btnEliminarVale.UseVisualStyleBackColor = True
        '
        'btnNuevoVale
        '
        Me.btnNuevoVale.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoVale.Enabled = False
        Me.btnNuevoVale.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoVale.Location = New System.Drawing.Point(551, 272)
        Me.btnNuevoVale.Name = "btnNuevoVale"
        Me.btnNuevoVale.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoVale.TabIndex = 12
        Me.btnNuevoVale.UseVisualStyleBackColor = True
        '
        'btnProveedor
        '
        Me.btnProveedor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProveedor.Enabled = False
        Me.btnProveedor.Image = Global.Oversight.My.Resources.Resources.empresa12x12
        Me.btnProveedor.Location = New System.Drawing.Point(410, 16)
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
        Me.btnPersona.Location = New System.Drawing.Point(404, 216)
        Me.btnPersona.Name = "btnPersona"
        Me.btnPersona.Size = New System.Drawing.Size(27, 23)
        Me.btnPersona.TabIndex = 10
        Me.btnPersona.UseVisualStyleBackColor = True
        '
        'lblNombreProveedor
        '
        Me.lblNombreProveedor.AutoSize = True
        Me.lblNombreProveedor.Location = New System.Drawing.Point(13, 21)
        Me.lblNombreProveedor.MaximumSize = New System.Drawing.Size(70, 0)
        Me.lblNombreProveedor.Name = "lblNombreProveedor"
        Me.lblNombreProveedor.Size = New System.Drawing.Size(59, 13)
        Me.lblNombreProveedor.TabIndex = 1
        Me.lblNombreProveedor.Text = "Proveedor:"
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(13, 221)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(151, 13)
        Me.lblPersona.TabIndex = 81
        Me.lblPersona.Text = "Persona que recibió la factura:"
        '
        'txtFolioFacturaProveedor
        '
        Me.txtFolioFacturaProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFolioFacturaProveedor.Location = New System.Drawing.Point(170, 77)
        Me.txtFolioFacturaProveedor.MaxLength = 15
        Me.txtFolioFacturaProveedor.Name = "txtFolioFacturaProveedor"
        Me.txtFolioFacturaProveedor.Size = New System.Drawing.Size(200, 20)
        Me.txtFolioFacturaProveedor.TabIndex = 3
        '
        'txtPersona
        '
        Me.txtPersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPersona.Enabled = False
        Me.txtPersona.Location = New System.Drawing.Point(170, 218)
        Me.txtPersona.Name = "txtPersona"
        Me.txtPersona.Size = New System.Drawing.Size(228, 20)
        Me.txtPersona.TabIndex = 0
        '
        'lblFolioFactura
        '
        Me.lblFolioFactura.AutoSize = True
        Me.lblFolioFactura.Location = New System.Drawing.Point(13, 80)
        Me.lblFolioFactura.Name = "lblFolioFactura"
        Me.lblFolioFactura.Size = New System.Drawing.Size(71, 13)
        Me.lblFolioFactura.TabIndex = 4
        Me.lblFolioFactura.Text = "Folio Factura:"
        '
        'txtLugar
        '
        Me.txtLugar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLugar.Location = New System.Drawing.Point(170, 167)
        Me.txtLugar.MaxLength = 1000
        Me.txtLugar.Multiline = True
        Me.txtLugar.Name = "txtLugar"
        Me.txtLugar.Size = New System.Drawing.Size(228, 43)
        Me.txtLugar.TabIndex = 8
        Me.txtLugar.Text = "Tuxtla Gutierrez, Chiapas"
        '
        'dgvDetalle
        '
        Me.dgvDetalle.AllowUserToAddRows = False
        Me.dgvDetalle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalle.Enabled = False
        Me.dgvDetalle.Location = New System.Drawing.Point(16, 272)
        Me.dgvDetalle.MultiSelect = False
        Me.dgvDetalle.Name = "dgvDetalle"
        Me.dgvDetalle.RowHeadersVisible = False
        Me.dgvDetalle.Size = New System.Drawing.Size(529, 177)
        Me.dgvDetalle.TabIndex = 11
        '
        'txtProveedor
        '
        Me.txtProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProveedor.Enabled = False
        Me.txtProveedor.Location = New System.Drawing.Point(170, 18)
        Me.txtProveedor.Name = "txtProveedor"
        Me.txtProveedor.Size = New System.Drawing.Size(234, 20)
        Me.txtProveedor.TabIndex = 0
        Me.txtProveedor.TabStop = False
        '
        'txtPorcentajeIVA
        '
        Me.txtPorcentajeIVA.Location = New System.Drawing.Point(170, 138)
        Me.txtPorcentajeIVA.MaxLength = 20
        Me.txtPorcentajeIVA.Name = "txtPorcentajeIVA"
        Me.txtPorcentajeIVA.Size = New System.Drawing.Size(82, 20)
        Me.txtPorcentajeIVA.TabIndex = 7
        Me.txtPorcentajeIVA.Text = "16"
        '
        'lblDetalleFactura
        '
        Me.lblDetalleFactura.AutoSize = True
        Me.lblDetalleFactura.Location = New System.Drawing.Point(13, 256)
        Me.lblDetalleFactura.Name = "lblDetalleFactura"
        Me.lblDetalleFactura.Size = New System.Drawing.Size(142, 13)
        Me.lblDetalleFactura.TabIndex = 68
        Me.lblDetalleFactura.Text = "Vales que ampara la factura:"
        '
        'dtFechaFactura
        '
        Me.dtFechaFactura.Location = New System.Drawing.Point(170, 47)
        Me.dtFechaFactura.Name = "dtFechaFactura"
        Me.dtFechaFactura.Size = New System.Drawing.Size(200, 20)
        Me.dtFechaFactura.TabIndex = 2
        '
        'lblLugar
        '
        Me.lblLugar.AutoSize = True
        Me.lblLugar.Location = New System.Drawing.Point(13, 182)
        Me.lblLugar.Name = "lblLugar"
        Me.lblLugar.Size = New System.Drawing.Size(106, 13)
        Me.lblLugar.TabIndex = 75
        Me.lblLugar.Text = "Ubicación del Gasto:"
        '
        'lblFechaFactura
        '
        Me.lblFechaFactura.AutoSize = True
        Me.lblFechaFactura.Location = New System.Drawing.Point(13, 53)
        Me.lblFechaFactura.Name = "lblFechaFactura"
        Me.lblFechaFactura.Size = New System.Drawing.Size(94, 13)
        Me.lblFechaFactura.TabIndex = 2
        Me.lblFechaFactura.Text = "Fecha de Factura:"
        '
        'lblPorcentajeIVA
        '
        Me.lblPorcentajeIVA.AutoSize = True
        Me.lblPorcentajeIVA.Location = New System.Drawing.Point(13, 141)
        Me.lblPorcentajeIVA.Name = "lblPorcentajeIVA"
        Me.lblPorcentajeIVA.Size = New System.Drawing.Size(27, 13)
        Me.lblPorcentajeIVA.TabIndex = 76
        Me.lblPorcentajeIVA.Text = "IVA:"
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
        Me.tpPagos.Size = New System.Drawing.Size(585, 607)
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
        Me.btnRevisionesPagos.Location = New System.Drawing.Point(10, 328)
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
        Me.dgvPagos.Location = New System.Drawing.Point(10, 31)
        Me.dgvPagos.MultiSelect = False
        Me.dgvPagos.Name = "dgvPagos"
        Me.dgvPagos.RowHeadersVisible = False
        Me.dgvPagos.Size = New System.Drawing.Size(525, 160)
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
        'AgregarFacturaCombustible
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(598, 639)
        Me.Controls.Add(Me.tcFacturas)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarFacturaCombustible"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Factura de Combustible (Vales)"
        Me.tcFacturas.ResumeLayout(False)
        Me.tpFactura.ResumeLayout(False)
        Me.tpFactura.PerformLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPagos.ResumeLayout(False)
        Me.tpPagos.PerformLayout()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcFacturas As System.Windows.Forms.TabControl
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
    Friend WithEvents txtSubtotal As System.Windows.Forms.TextBox
    Friend WithEvents lblDescripcionFactura As System.Windows.Forms.Label
    Friend WithEvents txtDescripcionFactura As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardarDatosFacturaYCerrar As System.Windows.Forms.Button
    Friend WithEvents btnCancelarFactura As System.Windows.Forms.Button
    Friend WithEvents btnGuardarDatosFactura As System.Windows.Forms.Button
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblIVA As System.Windows.Forms.Label
    Friend WithEvents lblSubtotal As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents btnPaso2 As System.Windows.Forms.Button
    Friend WithEvents lblPorcentaje As System.Windows.Forms.Label
    Friend WithEvents btnInsertarVale As System.Windows.Forms.Button
    Friend WithEvents btnEliminarVale As System.Windows.Forms.Button
    Friend WithEvents btnNuevoVale As System.Windows.Forms.Button
    Friend WithEvents btnProveedor As System.Windows.Forms.Button
    Friend WithEvents btnPersona As System.Windows.Forms.Button
    Friend WithEvents lblNombreProveedor As System.Windows.Forms.Label
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents txtFolioFacturaProveedor As System.Windows.Forms.TextBox
    Friend WithEvents txtPersona As System.Windows.Forms.TextBox
    Friend WithEvents lblFolioFactura As System.Windows.Forms.Label
    Friend WithEvents dgvDetalle As System.Windows.Forms.DataGridView
    Friend WithEvents txtProveedor As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajeIVA As System.Windows.Forms.TextBox
    Friend WithEvents lblDetalleFactura As System.Windows.Forms.Label
    Friend WithEvents dtFechaFactura As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblLugar As System.Windows.Forms.Label
    Friend WithEvents lblFechaFactura As System.Windows.Forms.Label
    Friend WithEvents lblPorcentajeIVA As System.Windows.Forms.Label
    Friend WithEvents btnRevisiones As System.Windows.Forms.Button
    Friend WithEvents btnRevisionesPagos As System.Windows.Forms.Button
    Friend WithEvents txtLugar As System.Windows.Forms.TextBox
End Class
