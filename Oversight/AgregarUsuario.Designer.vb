<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgregarUsuario
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AgregarUsuario))
        Me.dgvPermisosUsuario = New System.Windows.Forms.DataGridView
        Me.gbDatosTarjeta = New System.Windows.Forms.GroupBox
        Me.tcUsuario = New System.Windows.Forms.TabControl
        Me.tpAgregarUsuario = New System.Windows.Forms.TabPage
        Me.btnPersonas = New System.Windows.Forms.Button
        Me.chkOnline = New System.Windows.Forms.CheckBox
        Me.txtNombrePersona = New System.Windows.Forms.TextBox
        Me.chkActivo = New System.Windows.Forms.CheckBox
        Me.lblRespuesta = New System.Windows.Forms.Label
        Me.txtRespuesta = New System.Windows.Forms.TextBox
        Me.lblConfirmarPassword = New System.Windows.Forms.Label
        Me.txtConfirmarPassword = New System.Windows.Forms.TextBox
        Me.btnEliminarPermiso = New System.Windows.Forms.Button
        Me.btnInsertarPermiso = New System.Windows.Forms.Button
        Me.btnNuevoPermiso = New System.Windows.Forms.Button
        Me.lblDisponibilidad = New System.Windows.Forms.Label
        Me.lblUltimaModificacion = New System.Windows.Forms.Label
        Me.txtPreguntaRescate = New System.Windows.Forms.TextBox
        Me.lblPreguntaRescate = New System.Windows.Forms.Label
        Me.txtNombreUsuario = New System.Windows.Forms.TextBox
        Me.lblNombreUsuario = New System.Windows.Forms.Label
        Me.txtUltimaModificacion = New System.Windows.Forms.TextBox
        Me.lblPermisosUsuario = New System.Windows.Forms.Label
        Me.lblPersona = New System.Windows.Forms.Label
        Me.lblPassword = New System.Windows.Forms.Label
        Me.btnCancelar = New System.Windows.Forms.Button
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.btnGuardar = New System.Windows.Forms.Button
        Me.tpSesionesRestricciones = New System.Windows.Forms.TabPage
        Me.chkSesionesHistoricas = New System.Windows.Forms.CheckBox
        Me.dgvSesionesAbiertas = New System.Windows.Forms.DataGridView
        Me.btnKickout = New System.Windows.Forms.Button
        Me.btnLockout = New System.Windows.Forms.Button
        Me.tpTablaAccionesUsuario = New System.Windows.Forms.TabPage
        Me.lblBuscarLog = New System.Windows.Forms.Label
        Me.txtBuscarLog = New System.Windows.Forms.TextBox
        Me.dgvLogs = New System.Windows.Forms.DataGridView
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dgvPermisosUsuario, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDatosTarjeta.SuspendLayout()
        Me.tcUsuario.SuspendLayout()
        Me.tpAgregarUsuario.SuspendLayout()
        Me.tpSesionesRestricciones.SuspendLayout()
        CType(Me.dgvSesionesAbiertas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpTablaAccionesUsuario.SuspendLayout()
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvPermisosUsuario
        '
        Me.dgvPermisosUsuario.AllowUserToAddRows = False
        Me.dgvPermisosUsuario.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPermisosUsuario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPermisosUsuario.Enabled = False
        Me.dgvPermisosUsuario.Location = New System.Drawing.Point(15, 347)
        Me.dgvPermisosUsuario.MultiSelect = False
        Me.dgvPermisosUsuario.Name = "dgvPermisosUsuario"
        Me.dgvPermisosUsuario.RowHeadersVisible = False
        Me.dgvPermisosUsuario.Size = New System.Drawing.Size(431, 210)
        Me.dgvPermisosUsuario.TabIndex = 11
        '
        'gbDatosTarjeta
        '
        Me.gbDatosTarjeta.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDatosTarjeta.Controls.Add(Me.tcUsuario)
        Me.gbDatosTarjeta.Location = New System.Drawing.Point(5, 0)
        Me.gbDatosTarjeta.Name = "gbDatosTarjeta"
        Me.gbDatosTarjeta.Size = New System.Drawing.Size(518, 676)
        Me.gbDatosTarjeta.TabIndex = 1
        Me.gbDatosTarjeta.TabStop = False
        '
        'tcUsuario
        '
        Me.tcUsuario.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcUsuario.Controls.Add(Me.tpAgregarUsuario)
        Me.tcUsuario.Controls.Add(Me.tpSesionesRestricciones)
        Me.tcUsuario.Controls.Add(Me.tpTablaAccionesUsuario)
        Me.tcUsuario.Location = New System.Drawing.Point(6, 12)
        Me.tcUsuario.Name = "tcUsuario"
        Me.tcUsuario.SelectedIndex = 0
        Me.tcUsuario.Size = New System.Drawing.Size(506, 655)
        Me.tcUsuario.TabIndex = 14
        '
        'tpAgregarUsuario
        '
        Me.tpAgregarUsuario.Controls.Add(Me.btnPersonas)
        Me.tpAgregarUsuario.Controls.Add(Me.chkOnline)
        Me.tpAgregarUsuario.Controls.Add(Me.txtNombrePersona)
        Me.tpAgregarUsuario.Controls.Add(Me.chkActivo)
        Me.tpAgregarUsuario.Controls.Add(Me.lblRespuesta)
        Me.tpAgregarUsuario.Controls.Add(Me.txtRespuesta)
        Me.tpAgregarUsuario.Controls.Add(Me.lblConfirmarPassword)
        Me.tpAgregarUsuario.Controls.Add(Me.txtConfirmarPassword)
        Me.tpAgregarUsuario.Controls.Add(Me.btnEliminarPermiso)
        Me.tpAgregarUsuario.Controls.Add(Me.btnInsertarPermiso)
        Me.tpAgregarUsuario.Controls.Add(Me.btnNuevoPermiso)
        Me.tpAgregarUsuario.Controls.Add(Me.lblDisponibilidad)
        Me.tpAgregarUsuario.Controls.Add(Me.lblUltimaModificacion)
        Me.tpAgregarUsuario.Controls.Add(Me.dgvPermisosUsuario)
        Me.tpAgregarUsuario.Controls.Add(Me.txtPreguntaRescate)
        Me.tpAgregarUsuario.Controls.Add(Me.lblPreguntaRescate)
        Me.tpAgregarUsuario.Controls.Add(Me.txtNombreUsuario)
        Me.tpAgregarUsuario.Controls.Add(Me.lblNombreUsuario)
        Me.tpAgregarUsuario.Controls.Add(Me.txtUltimaModificacion)
        Me.tpAgregarUsuario.Controls.Add(Me.lblPermisosUsuario)
        Me.tpAgregarUsuario.Controls.Add(Me.lblPersona)
        Me.tpAgregarUsuario.Controls.Add(Me.lblPassword)
        Me.tpAgregarUsuario.Controls.Add(Me.btnCancelar)
        Me.tpAgregarUsuario.Controls.Add(Me.txtPassword)
        Me.tpAgregarUsuario.Controls.Add(Me.btnGuardar)
        Me.tpAgregarUsuario.Location = New System.Drawing.Point(4, 22)
        Me.tpAgregarUsuario.Name = "tpAgregarUsuario"
        Me.tpAgregarUsuario.Size = New System.Drawing.Size(498, 629)
        Me.tpAgregarUsuario.TabIndex = 2
        Me.tpAgregarUsuario.Text = "Datos del Usuario"
        Me.tpAgregarUsuario.UseVisualStyleBackColor = True
        '
        'btnPersonas
        '
        Me.btnPersonas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPersonas.Enabled = False
        Me.btnPersonas.Image = Global.Oversight.My.Resources.Resources.clients12x12
        Me.btnPersonas.Location = New System.Drawing.Point(391, 37)
        Me.btnPersonas.Name = "btnPersonas"
        Me.btnPersonas.Size = New System.Drawing.Size(27, 23)
        Me.btnPersonas.TabIndex = 1
        Me.btnPersonas.UseVisualStyleBackColor = True
        '
        'chkOnline
        '
        Me.chkOnline.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkOnline.AutoSize = True
        Me.chkOnline.Enabled = False
        Me.chkOnline.Location = New System.Drawing.Point(391, 14)
        Me.chkOnline.Name = "chkOnline"
        Me.chkOnline.Size = New System.Drawing.Size(70, 17)
        Me.chkOnline.TabIndex = 0
        Me.chkOnline.TabStop = False
        Me.chkOnline.Text = "En Línea"
        Me.chkOnline.UseVisualStyleBackColor = True
        '
        'txtNombrePersona
        '
        Me.txtNombrePersona.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombrePersona.Enabled = False
        Me.txtNombrePersona.Location = New System.Drawing.Point(137, 39)
        Me.txtNombrePersona.Name = "txtNombrePersona"
        Me.txtNombrePersona.Size = New System.Drawing.Size(248, 20)
        Me.txtNombrePersona.TabIndex = 0
        Me.txtNombrePersona.TabStop = False
        '
        'chkActivo
        '
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Location = New System.Drawing.Point(137, 280)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(95, 17)
        Me.chkActivo.TabIndex = 7
        Me.chkActivo.Text = "Usuario Activo"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'lblRespuesta
        '
        Me.lblRespuesta.AutoSize = True
        Me.lblRespuesta.Location = New System.Drawing.Point(12, 256)
        Me.lblRespuesta.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblRespuesta.Name = "lblRespuesta"
        Me.lblRespuesta.Size = New System.Drawing.Size(61, 13)
        Me.lblRespuesta.TabIndex = 77
        Me.lblRespuesta.Text = "Respuesta:"
        '
        'txtRespuesta
        '
        Me.txtRespuesta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRespuesta.Location = New System.Drawing.Point(137, 253)
        Me.txtRespuesta.MaxLength = 150
        Me.txtRespuesta.Name = "txtRespuesta"
        Me.txtRespuesta.Size = New System.Drawing.Size(248, 20)
        Me.txtRespuesta.TabIndex = 6
        '
        'lblConfirmarPassword
        '
        Me.lblConfirmarPassword.AutoSize = True
        Me.lblConfirmarPassword.Location = New System.Drawing.Point(13, 122)
        Me.lblConfirmarPassword.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblConfirmarPassword.Name = "lblConfirmarPassword"
        Me.lblConfirmarPassword.Size = New System.Drawing.Size(101, 13)
        Me.lblConfirmarPassword.TabIndex = 75
        Me.lblConfirmarPassword.Text = "Repetir Contraseña:"
        '
        'txtConfirmarPassword
        '
        Me.txtConfirmarPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConfirmarPassword.Location = New System.Drawing.Point(138, 119)
        Me.txtConfirmarPassword.MaxLength = 300
        Me.txtConfirmarPassword.Name = "txtConfirmarPassword"
        Me.txtConfirmarPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtConfirmarPassword.Size = New System.Drawing.Size(248, 20)
        Me.txtConfirmarPassword.TabIndex = 4
        Me.txtConfirmarPassword.UseSystemPasswordChar = True
        '
        'btnEliminarPermiso
        '
        Me.btnEliminarPermiso.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarPermiso.Enabled = False
        Me.btnEliminarPermiso.Image = Global.Oversight.My.Resources.Resources.delete12x12
        Me.btnEliminarPermiso.Location = New System.Drawing.Point(452, 405)
        Me.btnEliminarPermiso.Name = "btnEliminarPermiso"
        Me.btnEliminarPermiso.Size = New System.Drawing.Size(28, 23)
        Me.btnEliminarPermiso.TabIndex = 10
        Me.btnEliminarPermiso.UseVisualStyleBackColor = True
        '
        'btnInsertarPermiso
        '
        Me.btnInsertarPermiso.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInsertarPermiso.Enabled = False
        Me.btnInsertarPermiso.Image = Global.Oversight.My.Resources.Resources.insertcard12x12
        Me.btnInsertarPermiso.Location = New System.Drawing.Point(452, 376)
        Me.btnInsertarPermiso.Name = "btnInsertarPermiso"
        Me.btnInsertarPermiso.Size = New System.Drawing.Size(28, 23)
        Me.btnInsertarPermiso.TabIndex = 9
        Me.btnInsertarPermiso.UseVisualStyleBackColor = True
        '
        'btnNuevoPermiso
        '
        Me.btnNuevoPermiso.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevoPermiso.Enabled = False
        Me.btnNuevoPermiso.Image = Global.Oversight.My.Resources.Resources.newcard12x12
        Me.btnNuevoPermiso.Location = New System.Drawing.Point(452, 347)
        Me.btnNuevoPermiso.Name = "btnNuevoPermiso"
        Me.btnNuevoPermiso.Size = New System.Drawing.Size(28, 23)
        Me.btnNuevoPermiso.TabIndex = 8
        Me.btnNuevoPermiso.UseVisualStyleBackColor = True
        '
        'lblDisponibilidad
        '
        Me.lblDisponibilidad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDisponibilidad.AutoSize = True
        Me.lblDisponibilidad.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblDisponibilidad.Location = New System.Drawing.Point(392, 73)
        Me.lblDisponibilidad.MaximumSize = New System.Drawing.Size(80, 13)
        Me.lblDisponibilidad.MinimumSize = New System.Drawing.Size(56, 13)
        Me.lblDisponibilidad.Name = "lblDisponibilidad"
        Me.lblDisponibilidad.Size = New System.Drawing.Size(56, 13)
        Me.lblDisponibilidad.TabIndex = 70
        Me.lblDisponibilidad.Text = "Disponible"
        Me.lblDisponibilidad.Visible = False
        '
        'lblUltimaModificacion
        '
        Me.lblUltimaModificacion.AutoSize = True
        Me.lblUltimaModificacion.Location = New System.Drawing.Point(12, 16)
        Me.lblUltimaModificacion.Name = "lblUltimaModificacion"
        Me.lblUltimaModificacion.Size = New System.Drawing.Size(105, 13)
        Me.lblUltimaModificacion.TabIndex = 61
        Me.lblUltimaModificacion.Text = "Ultima Modificación: "
        '
        'txtPreguntaRescate
        '
        Me.txtPreguntaRescate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPreguntaRescate.Location = New System.Drawing.Point(138, 145)
        Me.txtPreguntaRescate.MaxLength = 150
        Me.txtPreguntaRescate.Multiline = True
        Me.txtPreguntaRescate.Name = "txtPreguntaRescate"
        Me.txtPreguntaRescate.Size = New System.Drawing.Size(342, 102)
        Me.txtPreguntaRescate.TabIndex = 5
        '
        'lblPreguntaRescate
        '
        Me.lblPreguntaRescate.AutoSize = True
        Me.lblPreguntaRescate.Location = New System.Drawing.Point(13, 148)
        Me.lblPreguntaRescate.Name = "lblPreguntaRescate"
        Me.lblPreguntaRescate.Size = New System.Drawing.Size(111, 13)
        Me.lblPreguntaRescate.TabIndex = 21
        Me.lblPreguntaRescate.Text = "Pregunta de Rescate:"
        '
        'txtNombreUsuario
        '
        Me.txtNombreUsuario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreUsuario.Location = New System.Drawing.Point(138, 67)
        Me.txtNombreUsuario.MaxLength = 100
        Me.txtNombreUsuario.Name = "txtNombreUsuario"
        Me.txtNombreUsuario.Size = New System.Drawing.Size(247, 20)
        Me.txtNombreUsuario.TabIndex = 2
        '
        'lblNombreUsuario
        '
        Me.lblNombreUsuario.AutoSize = True
        Me.lblNombreUsuario.Location = New System.Drawing.Point(12, 70)
        Me.lblNombreUsuario.Name = "lblNombreUsuario"
        Me.lblNombreUsuario.Size = New System.Drawing.Size(101, 13)
        Me.lblNombreUsuario.TabIndex = 23
        Me.lblNombreUsuario.Text = "Nombre de Usuario:"
        '
        'txtUltimaModificacion
        '
        Me.txtUltimaModificacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUltimaModificacion.Enabled = False
        Me.txtUltimaModificacion.Location = New System.Drawing.Point(137, 13)
        Me.txtUltimaModificacion.Name = "txtUltimaModificacion"
        Me.txtUltimaModificacion.Size = New System.Drawing.Size(248, 20)
        Me.txtUltimaModificacion.TabIndex = 0
        Me.txtUltimaModificacion.TabStop = False
        '
        'lblPermisosUsuario
        '
        Me.lblPermisosUsuario.AutoSize = True
        Me.lblPermisosUsuario.Location = New System.Drawing.Point(13, 315)
        Me.lblPermisosUsuario.MaximumSize = New System.Drawing.Size(110, 0)
        Me.lblPermisosUsuario.Name = "lblPermisosUsuario"
        Me.lblPermisosUsuario.Size = New System.Drawing.Size(108, 13)
        Me.lblPermisosUsuario.TabIndex = 24
        Me.lblPermisosUsuario.Text = "Permisos del Usuario:"
        '
        'lblPersona
        '
        Me.lblPersona.AutoSize = True
        Me.lblPersona.Location = New System.Drawing.Point(12, 42)
        Me.lblPersona.Name = "lblPersona"
        Me.lblPersona.Size = New System.Drawing.Size(121, 13)
        Me.lblPersona.TabIndex = 60
        Me.lblPersona.Text = "Relacionado a Persona:"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(12, 96)
        Me.lblPassword.MaximumSize = New System.Drawing.Size(130, 0)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(64, 13)
        Me.lblPassword.TabIndex = 46
        Me.lblPassword.Text = "Contraseña:"
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Image = Global.Oversight.My.Resources.Resources.cancel24x24
        Me.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelar.Location = New System.Drawing.Point(109, 583)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(199, 34)
        Me.btnCancelar.TabIndex = 12
        Me.btnCancelar.Text = "&Cancelar / Regresar sin Cambios"
        Me.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtPassword
        '
        Me.txtPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.Location = New System.Drawing.Point(137, 93)
        Me.txtPassword.MaxLength = 300
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(248, 20)
        Me.txtPassword.TabIndex = 3
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Enabled = False
        Me.btnGuardar.Image = Global.Oversight.My.Resources.Resources.user24x24
        Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGuardar.Location = New System.Drawing.Point(312, 583)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(168, 34)
        Me.btnGuardar.TabIndex = 13
        Me.btnGuardar.Text = "&Guardar Datos y Regresar"
        Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'tpSesionesRestricciones
        '
        Me.tpSesionesRestricciones.Controls.Add(Me.chkSesionesHistoricas)
        Me.tpSesionesRestricciones.Controls.Add(Me.dgvSesionesAbiertas)
        Me.tpSesionesRestricciones.Controls.Add(Me.btnKickout)
        Me.tpSesionesRestricciones.Controls.Add(Me.btnLockout)
        Me.tpSesionesRestricciones.Location = New System.Drawing.Point(4, 22)
        Me.tpSesionesRestricciones.Name = "tpSesionesRestricciones"
        Me.tpSesionesRestricciones.Size = New System.Drawing.Size(505, 629)
        Me.tpSesionesRestricciones.TabIndex = 3
        Me.tpSesionesRestricciones.Text = "Sesiones y Restricciones"
        Me.tpSesionesRestricciones.UseVisualStyleBackColor = True
        '
        'chkSesionesHistoricas
        '
        Me.chkSesionesHistoricas.AutoSize = True
        Me.chkSesionesHistoricas.Location = New System.Drawing.Point(3, 13)
        Me.chkSesionesHistoricas.Name = "chkSesionesHistoricas"
        Me.chkSesionesHistoricas.Size = New System.Drawing.Size(166, 17)
        Me.chkSesionesHistoricas.TabIndex = 20
        Me.chkSesionesHistoricas.Text = "Mostrar Sesiones ya Cerradas"
        Me.chkSesionesHistoricas.UseVisualStyleBackColor = True
        '
        'dgvSesionesAbiertas
        '
        Me.dgvSesionesAbiertas.AllowUserToAddRows = False
        Me.dgvSesionesAbiertas.AllowUserToDeleteRows = False
        Me.dgvSesionesAbiertas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSesionesAbiertas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSesionesAbiertas.Location = New System.Drawing.Point(3, 36)
        Me.dgvSesionesAbiertas.Name = "dgvSesionesAbiertas"
        Me.dgvSesionesAbiertas.ReadOnly = True
        Me.dgvSesionesAbiertas.RowHeadersVisible = False
        Me.dgvSesionesAbiertas.Size = New System.Drawing.Size(411, 590)
        Me.dgvSesionesAbiertas.TabIndex = 19
        Me.dgvSesionesAbiertas.Visible = False
        '
        'btnKickout
        '
        Me.btnKickout.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnKickout.Enabled = False
        Me.btnKickout.Location = New System.Drawing.Point(420, 36)
        Me.btnKickout.Name = "btnKickout"
        Me.btnKickout.Size = New System.Drawing.Size(78, 53)
        Me.btnKickout.TabIndex = 1
        Me.btnKickout.Text = "Sacar del Sistema esta Sesión"
        Me.btnKickout.UseVisualStyleBackColor = True
        '
        'btnLockout
        '
        Me.btnLockout.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLockout.Enabled = False
        Me.btnLockout.Location = New System.Drawing.Point(420, 95)
        Me.btnLockout.Name = "btnLockout"
        Me.btnLockout.Size = New System.Drawing.Size(78, 79)
        Me.btnLockout.TabIndex = 0
        Me.btnLockout.Text = "Impedir Nuevos Accesos de este IP/Maquina"
        Me.btnLockout.UseVisualStyleBackColor = True
        '
        'tpTablaAccionesUsuario
        '
        Me.tpTablaAccionesUsuario.Controls.Add(Me.lblBuscarLog)
        Me.tpTablaAccionesUsuario.Controls.Add(Me.txtBuscarLog)
        Me.tpTablaAccionesUsuario.Controls.Add(Me.dgvLogs)
        Me.tpTablaAccionesUsuario.Location = New System.Drawing.Point(4, 22)
        Me.tpTablaAccionesUsuario.Name = "tpTablaAccionesUsuario"
        Me.tpTablaAccionesUsuario.Padding = New System.Windows.Forms.Padding(3)
        Me.tpTablaAccionesUsuario.Size = New System.Drawing.Size(505, 629)
        Me.tpTablaAccionesUsuario.TabIndex = 0
        Me.tpTablaAccionesUsuario.Text = "Acciones del Usuario"
        Me.tpTablaAccionesUsuario.UseVisualStyleBackColor = True
        '
        'lblBuscarLog
        '
        Me.lblBuscarLog.AutoSize = True
        Me.lblBuscarLog.Location = New System.Drawing.Point(6, 16)
        Me.lblBuscarLog.Name = "lblBuscarLog"
        Me.lblBuscarLog.Size = New System.Drawing.Size(43, 13)
        Me.lblBuscarLog.TabIndex = 27
        Me.lblBuscarLog.Text = "Buscar:"
        '
        'txtBuscarLog
        '
        Me.txtBuscarLog.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscarLog.Location = New System.Drawing.Point(55, 13)
        Me.txtBuscarLog.Name = "txtBuscarLog"
        Me.txtBuscarLog.Size = New System.Drawing.Size(444, 20)
        Me.txtBuscarLog.TabIndex = 26
        '
        'dgvLogs
        '
        Me.dgvLogs.AllowUserToAddRows = False
        Me.dgvLogs.AllowUserToDeleteRows = False
        Me.dgvLogs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLogs.Location = New System.Drawing.Point(3, 39)
        Me.dgvLogs.Name = "dgvLogs"
        Me.dgvLogs.RowHeadersVisible = False
        Me.dgvLogs.Size = New System.Drawing.Size(496, 584)
        Me.dgvLogs.TabIndex = 18
        Me.dgvLogs.Visible = False
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Oversight"
        '
        'Timer1
        '
        Me.Timer1.Interval = 5000
        '
        'AgregarUsuario
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(528, 679)
        Me.Controls.Add(Me.gbDatosTarjeta)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AgregarUsuario"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Usuario"
        CType(Me.dgvPermisosUsuario, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDatosTarjeta.ResumeLayout(False)
        Me.tcUsuario.ResumeLayout(False)
        Me.tpAgregarUsuario.ResumeLayout(False)
        Me.tpAgregarUsuario.PerformLayout()
        Me.tpSesionesRestricciones.ResumeLayout(False)
        Me.tpSesionesRestricciones.PerformLayout()
        CType(Me.dgvSesionesAbiertas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpTablaAccionesUsuario.ResumeLayout(False)
        Me.tpTablaAccionesUsuario.PerformLayout()
        CType(Me.dgvLogs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvPermisosUsuario As System.Windows.Forms.DataGridView
    Friend WithEvents gbDatosTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblNombreUsuario As System.Windows.Forms.Label
    Friend WithEvents txtNombreUsuario As System.Windows.Forms.TextBox
    Friend WithEvents lblPreguntaRescate As System.Windows.Forms.Label
    Friend WithEvents txtPreguntaRescate As System.Windows.Forms.TextBox
    Friend WithEvents lblPermisosUsuario As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents dgvLogs As System.Windows.Forms.DataGridView
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents lblPersona As System.Windows.Forms.Label
    Friend WithEvents lblUltimaModificacion As System.Windows.Forms.Label
    Friend WithEvents txtUltimaModificacion As System.Windows.Forms.TextBox
    Friend WithEvents tcUsuario As System.Windows.Forms.TabControl
    Friend WithEvents tpTablaAccionesUsuario As System.Windows.Forms.TabPage
    Friend WithEvents tpAgregarUsuario As System.Windows.Forms.TabPage
    Friend WithEvents lblDisponibilidad As System.Windows.Forms.Label
    Friend WithEvents btnEliminarPermiso As System.Windows.Forms.Button
    Friend WithEvents btnInsertarPermiso As System.Windows.Forms.Button
    Friend WithEvents btnNuevoPermiso As System.Windows.Forms.Button
    Friend WithEvents chkOnline As System.Windows.Forms.CheckBox
    Friend WithEvents chkActivo As System.Windows.Forms.CheckBox
    Friend WithEvents lblRespuesta As System.Windows.Forms.Label
    Friend WithEvents txtRespuesta As System.Windows.Forms.TextBox
    Friend WithEvents lblConfirmarPassword As System.Windows.Forms.Label
    Friend WithEvents txtConfirmarPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnPersonas As System.Windows.Forms.Button
    Friend WithEvents txtNombrePersona As System.Windows.Forms.TextBox
    Friend WithEvents tpSesionesRestricciones As System.Windows.Forms.TabPage
    Friend WithEvents btnKickout As System.Windows.Forms.Button
    Friend WithEvents lblBuscarLog As System.Windows.Forms.Label
    Friend WithEvents txtBuscarLog As System.Windows.Forms.TextBox
    Friend WithEvents dgvSesionesAbiertas As System.Windows.Forms.DataGridView
    Friend WithEvents chkSesionesHistoricas As System.Windows.Forms.CheckBox
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnLockout As System.Windows.Forms.Button
End Class
