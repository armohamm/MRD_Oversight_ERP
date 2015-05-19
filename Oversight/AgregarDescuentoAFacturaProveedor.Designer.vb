<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarDescuentoAFacturaProveedor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarDescuentoAFacturaProveedor))
        Me.btnCerrarDescuentos = New System.Windows.Forms.Button
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblIVA = New System.Windows.Forms.Label
        Me.lblSubtotalTrasDescuentos = New System.Windows.Forms.Label
        Me.txtTotal = New System.Windows.Forms.TextBox
        Me.txtIVA = New System.Windows.Forms.TextBox
        Me.txtSubtotalTrasDescuentos = New System.Windows.Forms.TextBox
        Me.btnEliminarDescuento = New System.Windows.Forms.Button
        Me.btnNuevoDescuento = New System.Windows.Forms.Button
        Me.dgvDescuentos = New System.Windows.Forms.DataGridView
        Me.lblDescuentos = New System.Windows.Forms.Label
        Me.lblSubtotal = New System.Windows.Forms.Label
        Me.txtSubtotal = New System.Windows.Forms.TextBox
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        CType(Me.dgvDescuentos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCerrarDescuentos
        '
        Me.btnCerrarDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrarDescuentos.Enabled = False
        Me.btnCerrarDescuentos.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnCerrarDescuentos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCerrarDescuentos.Location = New System.Drawing.Point(341, 303)
        Me.btnCerrarDescuentos.Name = "btnCerrarDescuentos"
        Me.btnCerrarDescuentos.Size = New System.Drawing.Size(149, 34)
        Me.btnCerrarDescuentos.TabIndex = 100
        Me.btnCerrarDescuentos.Text = "Cerrar Descuentos"
        Me.btnCerrarDescuentos.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCerrarDescuentos.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(353, 265)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(31, 13)
        Me.lblTotal.TabIndex = 107
        Me.lblTotal.Text = "Total"
        '
        'lblIVA
        '
        Me.lblIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIVA.AutoSize = True
        Me.lblIVA.Location = New System.Drawing.Point(360, 239)
        Me.lblIVA.Name = "lblIVA"
        Me.lblIVA.Size = New System.Drawing.Size(24, 13)
        Me.lblIVA.TabIndex = 106
        Me.lblIVA.Text = "IVA"
        '
        'lblSubtotalTrasDescuentos
        '
        Me.lblSubtotalTrasDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubtotalTrasDescuentos.AutoSize = True
        Me.lblSubtotalTrasDescuentos.Location = New System.Drawing.Point(258, 213)
        Me.lblSubtotalTrasDescuentos.Name = "lblSubtotalTrasDescuentos"
        Me.lblSubtotalTrasDescuentos.Size = New System.Drawing.Size(126, 13)
        Me.lblSubtotalTrasDescuentos.TabIndex = 105
        Me.lblSubtotalTrasDescuentos.Text = "Subtotal tras Descuentos"
        '
        'txtTotal
        '
        Me.txtTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(390, 262)
        Me.txtTotal.MaxLength = 20
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(100, 20)
        Me.txtTotal.TabIndex = 104
        Me.txtTotal.TabStop = False
        '
        'txtIVA
        '
        Me.txtIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIVA.Enabled = False
        Me.txtIVA.Location = New System.Drawing.Point(390, 236)
        Me.txtIVA.MaxLength = 20
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(100, 20)
        Me.txtIVA.TabIndex = 103
        Me.txtIVA.TabStop = False
        '
        'txtSubtotalTrasDescuentos
        '
        Me.txtSubtotalTrasDescuentos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubtotalTrasDescuentos.Enabled = False
        Me.txtSubtotalTrasDescuentos.Location = New System.Drawing.Point(390, 210)
        Me.txtSubtotalTrasDescuentos.MaxLength = 20
        Me.txtSubtotalTrasDescuentos.Name = "txtSubtotalTrasDescuentos"
        Me.txtSubtotalTrasDescuentos.Size = New System.Drawing.Size(100, 20)
        Me.txtSubtotalTrasDescuentos.TabIndex = 102
        Me.txtSubtotalTrasDescuentos.TabStop = False
        '
        'btnEliminarDescuento
        '
        Me.btnEliminarDescuento.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarDescuento.Enabled = False
        Me.btnEliminarDescuento.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarDescuento.Location = New System.Drawing.Point(496, 88)
        Me.btnEliminarDescuento.Name = "btnEliminarDescuento"
        Me.btnEliminarDescuento.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarDescuento.TabIndex = 98
        Me.btnEliminarDescuento.UseVisualStyleBackColor = True
        '
        'btnNuevoDescuento
        '
        Me.btnNuevoDescuento.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoDescuento.Enabled = False
        Me.btnNuevoDescuento.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoDescuento.Location = New System.Drawing.Point(496, 30)
        Me.btnNuevoDescuento.Name = "btnNuevoDescuento"
        Me.btnNuevoDescuento.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoDescuento.TabIndex = 96
        Me.btnNuevoDescuento.UseVisualStyleBackColor = True
        '
        'dgvDescuentos
        '
        Me.dgvDescuentos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDescuentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDescuentos.Enabled = False
        Me.dgvDescuentos.Location = New System.Drawing.Point(15, 30)
        Me.dgvDescuentos.MultiSelect = False
        Me.dgvDescuentos.Name = "dgvDescuentos"
        Me.dgvDescuentos.RowHeadersVisible = False
        Me.dgvDescuentos.Size = New System.Drawing.Size(475, 148)
        Me.dgvDescuentos.TabIndex = 95
        '
        'lblDescuentos
        '
        Me.lblDescuentos.AutoSize = True
        Me.lblDescuentos.Location = New System.Drawing.Point(12, 9)
        Me.lblDescuentos.Name = "lblDescuentos"
        Me.lblDescuentos.Size = New System.Drawing.Size(115, 13)
        Me.lblDescuentos.TabIndex = 101
        Me.lblDescuentos.Text = "Descuentos aplicados:"
        '
        'lblSubtotal
        '
        Me.lblSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubtotal.AutoSize = True
        Me.lblSubtotal.Location = New System.Drawing.Point(338, 187)
        Me.lblSubtotal.Name = "lblSubtotal"
        Me.lblSubtotal.Size = New System.Drawing.Size(46, 13)
        Me.lblSubtotal.TabIndex = 109
        Me.lblSubtotal.Text = "Subtotal"
        '
        'txtSubtotal
        '
        Me.txtSubtotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSubtotal.Enabled = False
        Me.txtSubtotal.Location = New System.Drawing.Point(390, 184)
        Me.txtSubtotal.MaxLength = 20
        Me.txtSubtotal.Name = "txtSubtotal"
        Me.txtSubtotal.Size = New System.Drawing.Size(100, 20)
        Me.txtSubtotal.TabIndex = 108
        Me.txtSubtotal.TabStop = False
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarDescuentoAFactura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 356)
        Me.Controls.Add(Me.lblSubtotal)
        Me.Controls.Add(Me.txtSubtotal)
        Me.Controls.Add(Me.btnCerrarDescuentos)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblIVA)
        Me.Controls.Add(Me.lblSubtotalTrasDescuentos)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.txtIVA)
        Me.Controls.Add(Me.txtSubtotalTrasDescuentos)
        Me.Controls.Add(Me.btnEliminarDescuento)
        Me.Controls.Add(Me.btnNuevoDescuento)
        Me.Controls.Add(Me.dgvDescuentos)
        Me.Controls.Add(Me.lblDescuentos)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarDescuentoAFacturaProveedor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Descuento a Facturas de Proveedor"
        CType(Me.dgvDescuentos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCerrarDescuentos As System.Windows.Forms.Button
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblIVA As System.Windows.Forms.Label
    Friend WithEvents lblSubtotalTrasDescuentos As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtSubtotalTrasDescuentos As System.Windows.Forms.TextBox
    Friend WithEvents btnEliminarDescuento As System.Windows.Forms.Button
    Friend WithEvents btnNuevoDescuento As System.Windows.Forms.Button
    Friend WithEvents dgvDescuentos As System.Windows.Forms.DataGridView
    Friend WithEvents lblDescuentos As System.Windows.Forms.Label
    Friend WithEvents lblSubtotal As System.Windows.Forms.Label
    Friend WithEvents txtSubtotal As System.Windows.Forms.TextBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
