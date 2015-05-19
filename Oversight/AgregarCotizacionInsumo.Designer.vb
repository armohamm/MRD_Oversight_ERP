<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarCotizacionInsumo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarCotizacionInsumo))
        Me.gbPresupuesto = New System.Windows.Forms.GroupBox
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnElegirCotizacionGanadora = New System.Windows.Forms.Button
        Me.btnEliminarCotizacion = New System.Windows.Forms.Button
        Me.dgvCotizacionesInsumo = New System.Windows.Forms.DataGridView
        Me.lblCotizaciones = New System.Windows.Forms.Label
        Me.btnNuevaCotizacion = New System.Windows.Forms.Button
        Me.lblInsumo = New System.Windows.Forms.Label
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.txtInsumo = New System.Windows.Forms.TextBox
        Me.lblCantidad = New System.Windows.Forms.Label
        Me.txtCantidad = New System.Windows.Forms.TextBox
        Me.txtMejorSugerencia = New System.Windows.Forms.TextBox
        Me.lblMejorSugerencia = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbPresupuesto.SuspendLayout()
        CType(Me.dgvCotizacionesInsumo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbPresupuesto
        '
        Me.gbPresupuesto.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPresupuesto.Controls.Add(Me.btnCancelar)
        Me.gbPresupuesto.Controls.Add(Me.btnElegirCotizacionGanadora)
        Me.gbPresupuesto.Controls.Add(Me.btnEliminarCotizacion)
        Me.gbPresupuesto.Controls.Add(Me.dgvCotizacionesInsumo)
        Me.gbPresupuesto.Controls.Add(Me.lblCotizaciones)
        Me.gbPresupuesto.Controls.Add(Me.btnNuevaCotizacion)
        Me.gbPresupuesto.Controls.Add(Me.lblInsumo)
        Me.gbPresupuesto.Controls.Add(Me.btnGuardar)
        Me.gbPresupuesto.Controls.Add(Me.txtInsumo)
        Me.gbPresupuesto.Controls.Add(Me.lblCantidad)
        Me.gbPresupuesto.Controls.Add(Me.txtCantidad)
        Me.gbPresupuesto.Controls.Add(Me.txtMejorSugerencia)
        Me.gbPresupuesto.Controls.Add(Me.lblMejorSugerencia)
        Me.gbPresupuesto.Location = New System.Drawing.Point(3, -2)
        Me.gbPresupuesto.Name = "gbPresupuesto"
        Me.gbPresupuesto.Size = New System.Drawing.Size(613, 344)
        Me.gbPresupuesto.TabIndex = 0
        Me.gbPresupuesto.TabStop = False
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(390, 302)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 110
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnElegirCotizacionGanadora
        '
        Me.btnElegirCotizacionGanadora.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnElegirCotizacionGanadora.Enabled = False
        Me.btnElegirCotizacionGanadora.Image = Global.Oversight.My.Resources.Resources.yes_2_12x12
        Me.btnElegirCotizacionGanadora.Location = New System.Drawing.Point(580, 147)
        Me.btnElegirCotizacionGanadora.Name = "btnElegirCotizacionGanadora"
        Me.btnElegirCotizacionGanadora.Size = New System.Drawing.Size(28, 23)
        Me.btnElegirCotizacionGanadora.TabIndex = 69
        Me.btnElegirCotizacionGanadora.UseVisualStyleBackColor = True
        '
        'btnEliminarCotizacion
        '
        Me.btnEliminarCotizacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarCotizacion.Enabled = False
        Me.btnEliminarCotizacion.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarCotizacion.Location = New System.Drawing.Point(580, 118)
        Me.btnEliminarCotizacion.Name = "btnEliminarCotizacion"
        Me.btnEliminarCotizacion.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarCotizacion.TabIndex = 7
        Me.btnEliminarCotizacion.UseVisualStyleBackColor = True
        '
        'dgvCotizacionesInsumo
        '
        Me.dgvCotizacionesInsumo.AllowUserToAddRows = False
        Me.dgvCotizacionesInsumo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCotizacionesInsumo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCotizacionesInsumo.Location = New System.Drawing.Point(86, 89)
        Me.dgvCotizacionesInsumo.MultiSelect = False
        Me.dgvCotizacionesInsumo.Name = "dgvCotizacionesInsumo"
        Me.dgvCotizacionesInsumo.RowHeadersVisible = False
        Me.dgvCotizacionesInsumo.Size = New System.Drawing.Size(488, 158)
        Me.dgvCotizacionesInsumo.TabIndex = 8
        '
        'lblCotizaciones
        '
        Me.lblCotizaciones.AutoSize = True
        Me.lblCotizaciones.Location = New System.Drawing.Point(6, 89)
        Me.lblCotizaciones.Name = "lblCotizaciones"
        Me.lblCotizaciones.Size = New System.Drawing.Size(70, 13)
        Me.lblCotizaciones.TabIndex = 68
        Me.lblCotizaciones.Text = "Cotizaciones:"
        '
        'btnNuevaCotizacion
        '
        Me.btnNuevaCotizacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaCotizacion.Enabled = False
        Me.btnNuevaCotizacion.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnNuevaCotizacion.Location = New System.Drawing.Point(580, 89)
        Me.btnNuevaCotizacion.Name = "btnNuevaCotizacion"
        Me.btnNuevaCotizacion.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevaCotizacion.TabIndex = 6
        Me.btnNuevaCotizacion.UseVisualStyleBackColor = True
        '
        'lblInsumo
        '
        Me.lblInsumo.AutoSize = True
        Me.lblInsumo.Location = New System.Drawing.Point(7, 22)
        Me.lblInsumo.Name = "lblInsumo"
        Me.lblInsumo.Size = New System.Drawing.Size(44, 13)
        Me.lblInsumo.TabIndex = 1
        Me.lblInsumo.Text = "Insumo:"
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(485, 302)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(89, 34)
        Me.btnGuardar.TabIndex = 10
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'txtInsumo
        '
        Me.txtInsumo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtInsumo.Enabled = False
        Me.txtInsumo.Location = New System.Drawing.Point(86, 19)
        Me.txtInsumo.MaxLength = 500
        Me.txtInsumo.Name = "txtInsumo"
        Me.txtInsumo.Size = New System.Drawing.Size(383, 20)
        Me.txtInsumo.TabIndex = 1
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(7, 53)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(52, 13)
        Me.lblCantidad.TabIndex = 2
        Me.lblCantidad.Text = "Cantidad:"
        '
        'txtCantidad
        '
        Me.txtCantidad.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCantidad.Location = New System.Drawing.Point(86, 50)
        Me.txtCantidad.MaxLength = 1000
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(212, 20)
        Me.txtCantidad.TabIndex = 0
        Me.txtCantidad.TabStop = False
        '
        'txtMejorSugerencia
        '
        Me.txtMejorSugerencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMejorSugerencia.Enabled = False
        Me.txtMejorSugerencia.Location = New System.Drawing.Point(298, 267)
        Me.txtMejorSugerencia.MaxLength = 500
        Me.txtMejorSugerencia.Name = "txtMejorSugerencia"
        Me.txtMejorSugerencia.Size = New System.Drawing.Size(276, 20)
        Me.txtMejorSugerencia.TabIndex = 4
        '
        'lblMejorSugerencia
        '
        Me.lblMejorSugerencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMejorSugerencia.AutoSize = True
        Me.lblMejorSugerencia.Location = New System.Drawing.Point(136, 270)
        Me.lblMejorSugerencia.Name = "lblMejorSugerencia"
        Me.lblMejorSugerencia.Size = New System.Drawing.Size(156, 13)
        Me.lblMejorSugerencia.TabIndex = 4
        Me.lblMejorSugerencia.Text = "Mejor Sugerencia / Proveedor: "
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarCotizacionInsumo
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(619, 346)
        Me.Controls.Add(Me.gbPresupuesto)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarCotizacionInsumo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cotizacion de Insumo"
        Me.gbPresupuesto.ResumeLayout(False)
        Me.gbPresupuesto.PerformLayout()
        CType(Me.dgvCotizacionesInsumo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbPresupuesto As System.Windows.Forms.GroupBox
    Friend WithEvents lblInsumo As System.Windows.Forms.Label
    Friend WithEvents txtInsumo As System.Windows.Forms.TextBox
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents txtMejorSugerencia As System.Windows.Forms.TextBox
    Friend WithEvents lblMejorSugerencia As System.Windows.Forms.Label
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dgvCotizacionesInsumo As System.Windows.Forms.DataGridView
    Friend WithEvents lblCotizaciones As System.Windows.Forms.Label
    Friend WithEvents btnEliminarCotizacion As System.Windows.Forms.Button
    Friend WithEvents btnNuevaCotizacion As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnElegirCotizacionGanadora As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class
