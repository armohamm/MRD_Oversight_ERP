<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuscaInsumosConPrecioDiferente
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BuscaInsumosConPrecioDiferente))
        Me.gbBuscar = New System.Windows.Forms.GroupBox
        Me.lblInstructions2 = New System.Windows.Forms.Label
        Me.lblInstructions1 = New System.Windows.Forms.Label
        Me.dgvInsumosConPrecioDiferente = New System.Windows.Forms.DataGridView
        Me.btnIgnorar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbBuscar.SuspendLayout()
        CType(Me.dgvInsumosConPrecioDiferente, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbBuscar
        '
        Me.gbBuscar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBuscar.Controls.Add(Me.lblInstructions2)
        Me.gbBuscar.Controls.Add(Me.lblInstructions1)
        Me.gbBuscar.Controls.Add(Me.dgvInsumosConPrecioDiferente)
        Me.gbBuscar.Controls.Add(Me.btnIgnorar)
        Me.gbBuscar.Location = New System.Drawing.Point(7, 2)
        Me.gbBuscar.Name = "gbBuscar"
        Me.gbBuscar.Size = New System.Drawing.Size(544, 323)
        Me.gbBuscar.TabIndex = 29
        Me.gbBuscar.TabStop = False
        '
        'lblInstructions2
        '
        Me.lblInstructions2.AutoSize = True
        Me.lblInstructions2.Location = New System.Drawing.Point(8, 32)
        Me.lblInstructions2.MaximumSize = New System.Drawing.Size(525, 0)
        Me.lblInstructions2.Name = "lblInstructions2"
        Me.lblInstructions2.Size = New System.Drawing.Size(523, 26)
        Me.lblInstructions2.TabIndex = 33
        Me.lblInstructions2.Text = "Cuando hayas acabado con los insumos, esta ventana se cerrará. O puedes hacer cli" & _
            "ck en el botón ""Ignorar Precios Nuevos de Insumos Restantes""."
        '
        'lblInstructions1
        '
        Me.lblInstructions1.AutoSize = True
        Me.lblInstructions1.Location = New System.Drawing.Point(7, 12)
        Me.lblInstructions1.MaximumSize = New System.Drawing.Size(525, 0)
        Me.lblInstructions1.Name = "lblInstructions1"
        Me.lblInstructions1.Size = New System.Drawing.Size(481, 13)
        Me.lblInstructions1.TabIndex = 32
        Me.lblInstructions1.Text = "Haz doble click sobre los insumos que desees actualizar su precio SÓLO PARA ESTE " & _
            "PROYECTO. "
        '
        'dgvInsumosConPrecioDiferente
        '
        Me.dgvInsumosConPrecioDiferente.AllowUserToAddRows = False
        Me.dgvInsumosConPrecioDiferente.AllowUserToDeleteRows = False
        Me.dgvInsumosConPrecioDiferente.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvInsumosConPrecioDiferente.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvInsumosConPrecioDiferente.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvInsumosConPrecioDiferente.Location = New System.Drawing.Point(10, 71)
        Me.dgvInsumosConPrecioDiferente.MultiSelect = False
        Me.dgvInsumosConPrecioDiferente.Name = "dgvInsumosConPrecioDiferente"
        Me.dgvInsumosConPrecioDiferente.ReadOnly = True
        Me.dgvInsumosConPrecioDiferente.RowHeadersVisible = False
        Me.dgvInsumosConPrecioDiferente.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvInsumosConPrecioDiferente.Size = New System.Drawing.Size(527, 203)
        Me.dgvInsumosConPrecioDiferente.TabIndex = 1
        '
        'btnIgnorar
        '
        Me.btnIgnorar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnIgnorar.Image = Global.Oversight.My.Resources.Resources.open24x24
        Me.btnIgnorar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIgnorar.Location = New System.Drawing.Point(276, 280)
        Me.btnIgnorar.Name = "btnIgnorar"
        Me.btnIgnorar.Size = New System.Drawing.Size(261, 34)
        Me.btnIgnorar.TabIndex = 2
        Me.btnIgnorar.Text = "&Ignorar Precios Nuevos de Insumos Restantes"
        Me.btnIgnorar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIgnorar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'BuscaInsumosConPrecioDiferente
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 330)
        Me.Controls.Add(Me.gbBuscar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BuscaInsumosConPrecioDiferente"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Insumos con Precios Base Nuevos"
        Me.gbBuscar.ResumeLayout(False)
        Me.gbBuscar.PerformLayout()
        CType(Me.dgvInsumosConPrecioDiferente, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbBuscar As System.Windows.Forms.GroupBox
    Friend WithEvents btnIgnorar As System.Windows.Forms.Button
    Friend WithEvents dgvInsumosConPrecioDiferente As System.Windows.Forms.DataGridView
    Friend WithEvents lblInstructions2 As System.Windows.Forms.Label
    Friend WithEvents lblInstructions1 As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
