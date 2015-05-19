<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarInsumoPreguntaReplicarUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarInsumoPreguntaReplicarUpdate))
        Me.gbReplicación = New System.Windows.Forms.GroupBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.rbProyecto = New System.Windows.Forms.RadioButton
        Me.rbModelo = New System.Windows.Forms.RadioButton
        Me.rbBase = New System.Windows.Forms.RadioButton
        Me.lblIntro = New System.Windows.Forms.Label
        Me.btnActualizar = New System.Windows.Forms.Button
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.gbReplicación.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbReplicación
        '
        Me.gbReplicación.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbReplicación.Controls.Add(Me.FlowLayoutPanel1)
        Me.gbReplicación.Controls.Add(Me.lblIntro)
        Me.gbReplicación.Controls.Add(Me.btnActualizar)
        Me.gbReplicación.Controls.Add(Me.btnCancelar)
        Me.gbReplicación.Location = New System.Drawing.Point(7, 2)
        Me.gbReplicación.Name = "gbReplicación"
        Me.gbReplicación.Size = New System.Drawing.Size(501, 258)
        Me.gbReplicación.TabIndex = 29
        Me.gbReplicación.TabStop = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbProyecto)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbModelo)
        Me.FlowLayoutPanel1.Controls.Add(Me.rbBase)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(11, 53)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(470, 150)
        Me.FlowLayoutPanel1.TabIndex = 41
        '
        'rbProyecto
        '
        Me.rbProyecto.AutoSize = True
        Me.rbProyecto.Location = New System.Drawing.Point(3, 3)
        Me.rbProyecto.MaximumSize = New System.Drawing.Size(450, 0)
        Me.rbProyecto.Name = "rbProyecto"
        Me.rbProyecto.Size = New System.Drawing.Size(308, 17)
        Me.rbProyecto.TabIndex = 34
        Me.rbProyecto.TabStop = True
        Me.rbProyecto.Text = "Actualizar Precio del Insumo para este Proyecto únicamente"
        Me.rbProyecto.UseVisualStyleBackColor = True
        '
        'rbModelo
        '
        Me.rbModelo.AutoSize = True
        Me.rbModelo.Location = New System.Drawing.Point(3, 26)
        Me.rbModelo.MaximumSize = New System.Drawing.Size(450, 0)
        Me.rbModelo.MinimumSize = New System.Drawing.Size(450, 0)
        Me.rbModelo.Name = "rbModelo"
        Me.rbModelo.Size = New System.Drawing.Size(450, 17)
        Me.rbModelo.TabIndex = 42
        Me.rbModelo.TabStop = True
        Me.rbModelo.Text = "Actualizar Precio del Insumo para este Modelo únicamente"
        Me.rbModelo.UseVisualStyleBackColor = True
        '
        'rbBase
        '
        Me.rbBase.AutoSize = True
        Me.rbBase.Location = New System.Drawing.Point(3, 49)
        Me.rbBase.MaximumSize = New System.Drawing.Size(450, 0)
        Me.rbBase.MinimumSize = New System.Drawing.Size(450, 0)
        Me.rbBase.Name = "rbBase"
        Me.rbBase.Size = New System.Drawing.Size(450, 17)
        Me.rbBase.TabIndex = 41
        Me.rbBase.TabStop = True
        Me.rbBase.Text = "Actualizar Precio del Insumo en General"
        Me.rbBase.UseVisualStyleBackColor = True
        '
        'lblIntro
        '
        Me.lblIntro.AutoSize = True
        Me.lblIntro.Location = New System.Drawing.Point(8, 16)
        Me.lblIntro.MaximumSize = New System.Drawing.Size(480, 0)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(473, 26)
        Me.lblIntro.TabIndex = 33
        Me.lblIntro.Text = resources.GetString("lblIntro.Text")
        '
        'btnActualizar
        '
        Me.btnActualizar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActualizar.Enabled = False
        Me.btnActualizar.Image = Global.Oversight.My.Resources.Resources.open24x24
        Me.btnActualizar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnActualizar.Location = New System.Drawing.Point(363, 215)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(132, 34)
        Me.btnActualizar.TabIndex = 29
        Me.btnActualizar.Text = "&Actualizar Precios!"
        Me.btnActualizar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(268, 215)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(89, 34)
        Me.btnCancelar.TabIndex = 30
        Me.btnCancelar.Text = "&Cancelar"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'AgregarInsumoPreguntaReplicarUpdate
        '
        Me.AcceptButton = Me.btnActualizar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(517, 267)
        Me.Controls.Add(Me.gbReplicación)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "AgregarInsumoPreguntaReplicarUpdate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Replicar Actualización de Precios"
        Me.gbReplicación.ResumeLayout(False)
        Me.gbReplicación.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbReplicación As System.Windows.Forms.GroupBox
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents lblIntro As System.Windows.Forms.Label
    Friend WithEvents rbProyecto As System.Windows.Forms.RadioButton
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents rbBase As System.Windows.Forms.RadioButton
    Friend WithEvents rbModelo As System.Windows.Forms.RadioButton
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
End Class
