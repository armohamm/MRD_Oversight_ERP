<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarPoliza
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarPoliza))
        Me.gbPoliza = New System.Windows.Forms.GroupBox
        Me.cmbTipo = New System.Windows.Forms.ComboBox
        Me.dtFechaPoliza = New System.Windows.Forms.DateTimePicker
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.btnEliminar = New System.Windows.Forms.Button
        Me.dgvMovimientos = New System.Windows.Forms.DataGridView
        Me.lblIncluye = New System.Windows.Forms.Label
        Me.btnInsertar = New System.Windows.Forms.Button
        Me.lblInsumo = New System.Windows.Forms.Label
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.txtConcepto = New System.Windows.Forms.TextBox
        Me.lblCantidad = New System.Windows.Forms.Label
        Me.txtTotal = New System.Windows.Forms.TextBox
        Me.lblTotal = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbPoliza.SuspendLayout()
        CType(Me.dgvMovimientos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbPoliza
        '
        Me.gbPoliza.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPoliza.Controls.Add(Me.cmbTipo)
        Me.gbPoliza.Controls.Add(Me.dtFechaPoliza)
        Me.gbPoliza.Controls.Add(Me.btnCancelar)
        Me.gbPoliza.Controls.Add(Me.btnEliminar)
        Me.gbPoliza.Controls.Add(Me.dgvMovimientos)
        Me.gbPoliza.Controls.Add(Me.lblIncluye)
        Me.gbPoliza.Controls.Add(Me.btnInsertar)
        Me.gbPoliza.Controls.Add(Me.lblInsumo)
        Me.gbPoliza.Controls.Add(Me.btnGuardar)
        Me.gbPoliza.Controls.Add(Me.txtConcepto)
        Me.gbPoliza.Controls.Add(Me.lblCantidad)
        Me.gbPoliza.Controls.Add(Me.txtTotal)
        Me.gbPoliza.Controls.Add(Me.lblTotal)
        Me.gbPoliza.Location = New System.Drawing.Point(3, -2)
        Me.gbPoliza.Name = "gbPoliza"
        Me.gbPoliza.Size = New System.Drawing.Size(929, 472)
        Me.gbPoliza.TabIndex = 0
        Me.gbPoliza.TabStop = False
        '
        'cmbTipo
        '
        Me.cmbTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Items.AddRange(New Object() {"Ingresos", "Facturas de Combustible (Vales)", "Facturas de Proveedores", "Gastos por Casetas", "Vales", "Nominas", "Pagos"})
        Me.cmbTipo.Location = New System.Drawing.Point(689, 79)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(201, 21)
        Me.cmbTipo.TabIndex = 112
        '
        'dtFechaPoliza
        '
        Me.dtFechaPoliza.Location = New System.Drawing.Point(86, 53)
        Me.dtFechaPoliza.Name = "dtFechaPoliza"
        Me.dtFechaPoliza.Size = New System.Drawing.Size(200, 20)
        Me.dtFechaPoliza.TabIndex = 111
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(666, 422)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 110
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminar.Enabled = False
        Me.btnEliminar.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminar.Location = New System.Drawing.Point(896, 106)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminar.TabIndex = 7
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'dgvMovimientos
        '
        Me.dgvMovimientos.AllowUserToAddRows = False
        Me.dgvMovimientos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMovimientos.Location = New System.Drawing.Point(10, 106)
        Me.dgvMovimientos.MultiSelect = False
        Me.dgvMovimientos.Name = "dgvMovimientos"
        Me.dgvMovimientos.RowHeadersVisible = False
        Me.dgvMovimientos.Size = New System.Drawing.Size(880, 269)
        Me.dgvMovimientos.TabIndex = 8
        '
        'lblIncluye
        '
        Me.lblIncluye.AutoSize = True
        Me.lblIncluye.Location = New System.Drawing.Point(7, 82)
        Me.lblIncluye.Name = "lblIncluye"
        Me.lblIncluye.Size = New System.Drawing.Size(44, 13)
        Me.lblIncluye.TabIndex = 68
        Me.lblIncluye.Text = "Incluye:"
        '
        'btnInsertar
        '
        Me.btnInsertar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertar.Enabled = False
        Me.btnInsertar.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertar.Location = New System.Drawing.Point(896, 77)
        Me.btnInsertar.Name = "btnInsertar"
        Me.btnInsertar.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertar.TabIndex = 6
        Me.btnInsertar.UseVisualStyleBackColor = True
        '
        'lblInsumo
        '
        Me.lblInsumo.AutoSize = True
        Me.lblInsumo.Location = New System.Drawing.Point(7, 22)
        Me.lblInsumo.Name = "lblInsumo"
        Me.lblInsumo.Size = New System.Drawing.Size(56, 13)
        Me.lblInsumo.TabIndex = 1
        Me.lblInsumo.Text = "Concepto:"
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.saveclose33x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(761, 422)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(129, 34)
        Me.btnGuardar.TabIndex = 10
        Me.btnGuardar.Text = "Guardar Póliza"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'txtConcepto
        '
        Me.txtConcepto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConcepto.Location = New System.Drawing.Point(86, 19)
        Me.txtConcepto.MaxLength = 500
        Me.txtConcepto.Name = "txtConcepto"
        Me.txtConcepto.Size = New System.Drawing.Size(804, 20)
        Me.txtConcepto.TabIndex = 1
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(7, 53)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(40, 13)
        Me.lblCantidad.TabIndex = 2
        Me.lblCantidad.Text = "Fecha:"
        '
        'txtTotal
        '
        Me.txtTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(770, 381)
        Me.txtTotal.MaxLength = 500
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(120, 20)
        Me.txtTotal.TabIndex = 4
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(727, 384)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(37, 13)
        Me.lblTotal.TabIndex = 4
        Me.lblTotal.Text = "Total: "
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarPoliza
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(935, 474)
        Me.Controls.Add(Me.gbPoliza)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarPoliza"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Poliza"
        Me.gbPoliza.ResumeLayout(False)
        Me.gbPoliza.PerformLayout()
        CType(Me.dgvMovimientos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbPoliza As System.Windows.Forms.GroupBox
    Friend WithEvents lblInsumo As System.Windows.Forms.Label
    Friend WithEvents txtConcepto As System.Windows.Forms.TextBox
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents dgvMovimientos As System.Windows.Forms.DataGridView
    Friend WithEvents lblIncluye As System.Windows.Forms.Label
    Friend WithEvents btnEliminar As System.Windows.Forms.Button
    Friend WithEvents btnInsertar As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents dtFechaPoliza As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
End Class
