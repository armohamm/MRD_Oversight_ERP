<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaDirecciones
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaDirecciones))
        Me.gbDireccion = New System.Windows.Forms.GroupBox
        Me.dgvZipcodes = New System.Windows.Forms.DataGridView
        Me.lblBuscar = New System.Windows.Forms.Label
        Me.txtBuscar = New System.Windows.Forms.TextBox
        Me.lblDireccionFinal = New System.Windows.Forms.Label
        Me.txtDireccionFinal = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.lblIntro = New System.Windows.Forms.Label
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbDireccion.SuspendLayout()
        CType(Me.dgvZipcodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbDireccion
        '
        Me.gbDireccion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDireccion.Controls.Add(Me.dgvZipcodes)
        Me.gbDireccion.Controls.Add(Me.lblBuscar)
        Me.gbDireccion.Controls.Add(Me.txtBuscar)
        Me.gbDireccion.Controls.Add(Me.lblDireccionFinal)
        Me.gbDireccion.Controls.Add(Me.txtDireccionFinal)
        Me.gbDireccion.Controls.Add(Me.btnGuardar)
        Me.gbDireccion.Controls.Add(Me.btnCancelar)
        Me.gbDireccion.Controls.Add(Me.lblIntro)
        Me.gbDireccion.Location = New System.Drawing.Point(4, 0)
        Me.gbDireccion.Name = "gbDireccion"
        Me.gbDireccion.Size = New System.Drawing.Size(791, 437)
        Me.gbDireccion.TabIndex = 57
        Me.gbDireccion.TabStop = False
        '
        'dgvZipcodes
        '
        Me.dgvZipcodes.AllowUserToAddRows = False
        Me.dgvZipcodes.AllowUserToDeleteRows = False
        Me.dgvZipcodes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvZipcodes.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvZipcodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvZipcodes.Location = New System.Drawing.Point(12, 76)
        Me.dgvZipcodes.MultiSelect = False
        Me.dgvZipcodes.Name = "dgvZipcodes"
        Me.dgvZipcodes.ReadOnly = True
        Me.dgvZipcodes.RowHeadersVisible = False
        Me.dgvZipcodes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvZipcodes.Size = New System.Drawing.Size(765, 168)
        Me.dgvZipcodes.TabIndex = 2
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(9, 47)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(43, 13)
        Me.lblBuscar.TabIndex = 67
        Me.lblBuscar.Text = "Buscar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Location = New System.Drawing.Point(58, 44)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(719, 20)
        Me.txtBuscar.TabIndex = 1
        '
        'lblDireccionFinal
        '
        Me.lblDireccionFinal.AutoSize = True
        Me.lblDireccionFinal.Location = New System.Drawing.Point(9, 257)
        Me.lblDireccionFinal.Name = "lblDireccionFinal"
        Me.lblDireccionFinal.Size = New System.Drawing.Size(80, 13)
        Me.lblDireccionFinal.TabIndex = 64
        Me.lblDireccionFinal.Text = "Dirección Final:"
        '
        'txtDireccionFinal
        '
        Me.txtDireccionFinal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDireccionFinal.Location = New System.Drawing.Point(12, 281)
        Me.txtDireccionFinal.MaxLength = 1000
        Me.txtDireccionFinal.Multiline = True
        Me.txtDireccionFinal.Name = "txtDireccionFinal"
        Me.txtDireccionFinal.Size = New System.Drawing.Size(765, 88)
        Me.txtDireccionFinal.TabIndex = 3
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.save24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(502, 384)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(275, 34)
        Me.btnGuardar.TabIndex = 5
        Me.btnGuardar.Text = "&Guardar Dirección y Regresar a Ventana Anterior"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(407, 384)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 4
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.Location = New System.Drawing.Point(9, 16)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(602, 13)
        Me.lblIntro.TabIndex = 35
        Me.lblIntro.Text = "Esta ventana te permitirá identificar rápidamente un lugar con el Código Postal, " & _
            "pero la Dirección final a guardar es tu elección."
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaDirecciones
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(801, 442)
        Me.Controls.Add(Me.gbDireccion)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BuscaDirecciones"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Buscar Dirección"
        Me.gbDireccion.ResumeLayout(False)
        Me.gbDireccion.PerformLayout()
        CType(Me.dgvZipcodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbDireccion As System.Windows.Forms.GroupBox
    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblDireccionFinal As System.Windows.Forms.Label
    Friend WithEvents txtDireccionFinal As System.Windows.Forms.TextBox
    Friend WithEvents dgvZipcodes As System.Windows.Forms.DataGridView
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
