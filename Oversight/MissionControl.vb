Imports System.Windows.Forms

Public Class MissionControl

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

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


    Private Sub getFilesIncorrectlyClosed()

        Dim conteoGeneral As Integer = 0
        Dim pt As Point

        conteoGeneral = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM ( SELECT * FROM recentlyopenedfiles WHERE susername = '" & susername & "' GROUP BY sdocumenttype DESC, sid ASC ) h1 WHERE sdocumentstatus = '1' AND susername IN (SELECT susername FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL ORDER BY ilogindate DESC, slogintime DESC)")

        If conteoGeneral > 0 Then

            Dim banc As New BuscaArchivosCerradosIncorrectamente

            banc.susername = susername
            banc.bactive = bactive
            banc.bonline = bonline
            banc.suserfullname = suserfullname
            banc.suseremail = suseremail
            banc.susersession = susersession
            banc.susermachinename = susermachinename
            banc.suserip = suserip

            banc.StartPosition = FormStartPosition.Manual

            Dim tamañoPantalla As Integer = Screen.GetWorkingArea(Me).Height

            Dim tmpPt1 As Point = New Point(Me.Location.X, (tamañoPantalla - Me.Size.Height - banc.Size.Height) / 2) 'msg window
            Dim tmpPt2 As Point = New Point(Me.Location.X, tmpPt1.Y + banc.Size.Height) 'me

            If tmpPt1.Y > Screen.GetWorkingArea(Me).Location.Y Then

                pt = New Point(Me.Location.X, tmpPt1.Y)
                Me.Location = New Point(Me.Location.X, tmpPt2.Y)

            Else

                pt = New Point(Me.Location.X, Me.Location.Y)

            End If

            banc.Location = pt

            banc.Show(Me)

        End If

    End Sub


    Private Sub closeTimedOutConnections()

        Dim queryTimedOutConnections As String = ""
        Dim queriesLogout(2) As String

        Dim dsTimedOutConnections As DataSet

        queryTimedOutConnections = "SELECT s.* " & _
        "FROM sessions s " & _
        "JOIN (SELECT * FROM (SELECT * FROM logs ORDER BY iupdatedate DESC, supdatetime DESC, supdateusername ASC, susersession ASC) l GROUP BY supdateusername) l ON s.susername = l.supdateusername " & _
        "WHERE s.ilogoutdate IS NULL " & _
        "AND TIMEDIFF(STR_TO_DATE(CONCAT(" & getMySQLDate() & ", ' ', '" & getAppTime() & "'), '%Y%c%d %T'), STR_TO_DATE(CONCAT(l.iupdatedate, ' ', l.supdatetime), '%Y%c%d %T')) > '00:30:00' "

        dsTimedOutConnections = getSQLQueryAsDataset(0, queryTimedOutConnections)

        Try
            If dsTimedOutConnections Is System.DBNull.Value Then
                Exit Sub
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        If dsTimedOutConnections.Tables(0).Rows.Count > 0 Then

            For i = 0 To dsTimedOutConnections.Tables(0).Rows.Count - 1

                Try

                    If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM sessions WHERE susername = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("susername") & "' AND ilogoutdate IS NULL" = 1) Then
                        queriesLogout(0) = "UPDATE users SET bonline = 0 WHERE susername = '" & susername & "'"
                    End If

                    queriesLogout(1) = "UPDATE sessions SET btimedout = 1, ilogoutdate = " & getMySQLDate() & ", slogouttime = '" & getAppTime() & "' WHERE susername = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("susername") & "' AND susersession = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("susersession") & "'"

                    executeTransactedSQLCommand(0, queriesLogout)

                Catch ex As Exception

                End Try

            Next i

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


    Private Sub dropIrrecoverableTempTables()

        Dim dsUsuarios As DataSet

        dsUsuarios = getSQLQueryAsDataset(0, "SELECT susername FROM users")

        For i = 0 To dsUsuarios.Tables(0).Rows.Count - 1

            If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM sessions WHERE susername = '" & dsUsuarios.Tables(0).Rows(i).Item(0) & "' AND ilogoutdate IS NULL") = 0 Then

                Dim dsCandidatesTempTablesToBeDropped As DataSet

                dsCandidatesTempTablesToBeDropped = getSQLQueryAsDataset(0, "SELECT TABLE_NAME " & _
                "FROM information_schema.TABLES " & _
                "WHERE TABLE_NAME LIKE '%" & dsUsuarios.Tables(0).Rows(i).Item(0) & "%Type%' " & _
                "OR TABLE_NAME LIKE '%" & dsUsuarios.Tables(0).Rows(i).Item(0) & "%LegacyCategory%' " & _
                "OR TABLE_NAME LIKE '%" & dsUsuarios.Tables(0).Rows(i).Item(0) & "%ToUnit%' ")

                If dsCandidatesTempTablesToBeDropped.Tables(0).Rows.Count > 0 Then

                    For j = 0 To dsCandidatesTempTablesToBeDropped.Tables(0).Rows.Count - 1
                        executeSQLCommand(0, "DROP TABLE IF EXISTS " & dsCandidatesTempTablesToBeDropped.Tables(0).Rows(j).Item(0))
                    Next j

                End If

            End If

        Next i

    End Sub


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Proyectos" Then
                    tsbProyectos.Visible = True
                End If

                If permission = "Proyectos - Ver" Then
                    tsmiVerProyectos.Visible = True
                End If

                If permission = "Proyectos - Nuevo" Then
                    tsmiNuevoProyecto.Visible = True
                End If

                If permission = "Proyectos - Nuevo Desde Modelo" Then
                    tsmiNuevoProyectoDesdeModelo.Visible = True
                End If

                If permission = "Proyectos - Duplicar" Then
                    tsmiDuplicarProyecto.Visible = True
                End If

                If permission = "Proyectos - Eliminar" Then
                    tsmiEliminarProyecto.Visible = True
                End If

                If permission = "Modelos" Then
                    tsbModelos.Visible = True
                End If

                If permission = "Modelos - Ver" Then
                    tsmiVerModelos.Visible = True
                End If

                If permission = "Modelos - Nuevo" Then
                    tsmiNuevoModelo.Visible = True
                End If

                If permission = "Modelos - Eliminar" Then
                    tsmiEliminarModelo.Visible = True
                End If

                If permission = "Presupuestos Base" Then
                    tsbPresupuestosBase.Visible = True
                End If

                If permission = "Presupuestos Base - Ver" Then
                    tsmiVerPresupuestosBase.Visible = True
                End If

                If permission = "Presupuestos Base - Nuevo" Then
                    tsmiNuevoPresupuestoBase.Visible = True
                End If

                If permission = "Presupuestos Base - Eliminar" Then
                    tsmiEliminarPresupuestoBase.Visible = True
                End If

                If permission = "Materiales" Then
                    tsbMateriales.Visible = True
                End If

                If permission = "Materiales - Ver" Then
                    tsmiVerMaterialesPrecios.Visible = True
                End If

                If permission = "Materiales - Nuevo" Then
                    tsmiNuevoMaterial.Visible = True
                End If

                If permission = "Materiales - Eliminar" Then
                    tsmiEliminarMaterial.Visible = True
                End If

                If permission = "Cotizaciones" Then
                    tsbCotizacionesMateriales.Visible = True
                End If

                If permission = "Cotizaciones - Ver" Then
                    tsmiVerCotizacionesDeMateriales.Visible = True
                End If

                If permission = "Cotizaciones - Nueva" Then
                    tsmiNuevaCotizacionDeMateriales.Visible = True
                End If

                If permission = "Ordenes" Then
                    tsbPedidos.Visible = True
                End If

                If permission = "Ordenes - Ver" Then
                    tsmiVerPedidosDeMateriales.Visible = True
                End If

                If permission = "Ordenes - Nueva" Then
                    tsmiNuevoPedidoDeMaterial.Visible = True
                End If

                If permission = "Ordenes - Eliminar" Then
                    tsmiEliminarPedidosDeMaterial.Visible = True
                End If

                If permission = "Envios" Then
                    tsbEnvios.Visible = True
                End If

                If permission = "Envios - Ver" Then
                    tsmiVerEnvíosDeMaterial.Visible = True
                End If

                If permission = "Envios - Nuevo" Then
                    tsmiNuevoEnvíoDeMaterial.Visible = True
                End If

                If permission = "Envios - Eliminar" Then
                    tsmiEliminarEnvíoDeMaterial.Visible = True
                End If

                If permission = "Vales" Then
                    tsbVales.Visible = True
                End If

                If permission = "Vales - Ver" Then
                    tsmiVerValesDeGasolina.Visible = True
                End If

                If permission = "Vales - Nuevo" Then
                    tsmiNuevoValeDeGasolina.Visible = True
                End If

                If permission = "Vales - Eliminar" Then
                    tsmiEliminarValeDeGasolina.Visible = True
                End If

                If permission = "Vales - Ver Facturas" Then
                    tsmiVerFacturaCombustibleVales.Visible = True
                End If

                If permission = "Vales - Nueva Factura" Then
                    tsmiNuevaFacturaCombustibleVales.Visible = True
                End If

                If permission = "Vales - Eliminar Factura" Then
                    tsmiEliminarFacturaCombustibleVales.Visible = True
                End If

                If permission = "Proveedores" Then
                    tsbProveedores.Visible = True
                End If

                'If permission = "Proveedores - Ver" Then
                '    tsmiVerProveedoresDeudas.Visible = True
                'End If

                'If permission = "Proveedores - Nuevo" Then
                '    tsmiNuevoProveedor.Visible = True
                'End If

                'If permission = "Proveedores - Eliminar" Then
                '    tsmiEliminarProveedor.Visible = True
                'End If

                If permission = "Proveedores - Ver Facturas" Then
                    tsmiVerFacturasDeProveedores.Visible = True
                End If

                If permission = "Proveedores - Revisar Facturas" Then
                    tsmiRevisarFacturasDeProveedores.Visible = True
                End If

                If permission = "Proveedores - Nueva Factura" Then
                    tsmiNuevaFacturaDeProveedor.Visible = True
                End If

                If permission = "Proveedores - Eliminar Factura" Then
                    tsmiEliminarFacturaDeProveedor.Visible = True
                End If

                If permission = "Nominas" Then
                    tsbNominas.Visible = True
                End If

                If permission = "Nominas - Ver" Then
                    tsmiVerNominas.Visible = True
                End If

                If permission = "Nominas - Nueva" Then
                    tsmiNuevaNomina.Visible = True
                End If

                If permission = "Nominas - Eliminar" Then
                    tsmiEliminarNomina.Visible = True
                End If

                'If permission = "Personas" Then
                '    tsbPersonas.Visible = True
                'End If

                'If permission = "Personas - Ver" Then
                '    tsmiVerPersonas.Visible = True
                'End If

                'If permission = "Personas - Nueva" Then
                '    tsmiNuevaPersona.Visible = True
                'End If

                'If permission = "Personas - Eliminar" Then
                '    tsmiEliminarPersona.Visible = True
                'End If

                If permission = "Directorio" Then
                    tsbDirectorio.Visible = True
                End If

                If permission = "Directorio - Ver" Then
                    tsmiVerDirectorio.Visible = True
                End If

                If permission = "Directorio - Nueva Persona/Proveedor" Then
                    tsmiNuevaPersonaProveedor.Visible = True
                End If

                If permission = "Directorio - Eliminar Persona/Proveedor" Then
                    tsmiEliminarPersonaProveedor.Visible = True
                End If

                If permission = "Facturas" Then
                    tsbFacturasMRD.Visible = True
                End If

                If permission = "Facturas - Ver" Then
                    tsmiVerFacturasEmitidas.Visible = True
                End If

                If permission = "Facturas - Nueva" Then
                    tsmiNuevaFactura.Visible = True
                End If

                If permission = "Facturas - Eliminar" Then
                    tsmiEliminarFactura.Visible = True
                End If

                If permission = "Ingresos" Then
                    tsbIngresos.Visible = True
                End If

                If permission = "Ingresos - Ver" Then
                    tsmiVerIngresos.Visible = True
                End If

                If permission = "Ingresos - Nuevo" Then
                    tsmiNuevoIngreso.Visible = True
                End If

                If permission = "Ingresos - Eliminar" Then
                    tsmiEliminarIngreso.Visible = True
                End If

                If permission = "Pagos - Ver" Then
                    tsmiVerPagos.Visible = True
                End If

                If permission = "Pagos - Nuevo" Then
                    tsmiNuevoPago.Visible = True
                End If

                If permission = "Pagos - Eliminar" Then
                    tsmiEliminarPago.Visible = True
                End If

                If permission = "Inventario Materiales" Then
                    tsbInventarios.Visible = True
                End If

                If permission = "Inventario Materiales - Ver" Then
                    tsmiVerInventarioDeMateriales.Visible = True
                End If

                If permission = "Inventario Activos - Ver" Then
                    tsmiVerInventarioDeActivos.Visible = True
                End If

                If permission = "Activos" Then
                    tsbActivos.Visible = True
                End If

                If permission = "Activos - Ver" Then
                    tsmiVerActivos.Visible = True
                End If

                If permission = "Activos - Nuevo" Then
                    tsmiNuevoActivo.Visible = True
                End If

                If permission = "Activos - Eliminar" Then
                    tsmiEliminarActivo.Visible = True
                End If

                If permission = "Cuentas" Then
                    tsbCuentas.Visible = True
                End If

                If permission = "Cuentas - Ver" Then
                    tsmiVerCuentas.Visible = True
                End If

                If permission = "Cuentas - Nueva" Then
                    tsmiNuevaCuenta.Visible = True
                End If

                If permission = "Cuentas - Eliminar" Then
                    tsmiEliminarCuenta.Visible = True
                End If

                If permission = "Cuentas - Ver Saldos" Then
                    tsmiVerSaldoEnCuentas.Visible = True
                End If

                If permission = "Usuarios" Then
                    tsbUsuarios.Visible = True
                End If

                If permission = "Usuarios - Ver" Then
                    tsmiVerUsuarios.Visible = True
                End If

                If permission = "Usuarios - Nuevo" Then
                    tsmiNuevoUsuario.Visible = True
                End If

                If permission = "Usuarios - Eliminar" Then
                    tsmiEliminarUsuario.Visible = True
                End If

                If permission = "Unidades" Then
                    tsbUnidades.Visible = True
                End If

                If permission = "Unidades - Ver" Then
                    tsmiVerUnidades.Visible = True
                End If

                If permission = "Unidades - Nueva" Then
                    tsmiNuevaUnidad.Visible = True
                End If

                If permission = "Unidades - Eliminar" Then
                    tsmiEliminarUnidad.Visible = True
                End If

                If permission = "Mensajes" Then
                    tsbMensajes.Visible = True
                End If

                If permission = "Mensajes - Ver" Then
                    tsmiVerMensajes.Visible = True
                End If

                If permission = "Mensajes - Nuevo" Then
                    tsmiNuevoMensaje.Visible = True
                End If

                If permission = "Polizas" Then
                    tsbPolizas.Visible = True
                End If

                If permission = "Polizas - Ver" Then
                    tsmiVerPolizas.Visible = True
                End If

                If permission = "Polizas - Nueva" Then
                    tsmiNuevaPoliza.Visible = True
                End If

                If permission = "Polizas - Eliminar" Then
                    tsmiEliminarPoliza.Visible = True
                End If

                If permission = "Reportes" Then
                    tsbReportes.Visible = True
                End If

                If permission = "Reportes - Ver" Then
                    tsmiVerReportes.Visible = True
                End If

                'If permission = "Unidades - Ver Equivalencias" Then
                '    tsmiVerEquivalencias.Visible = True
                'End If

                'If permission = "Unidades - Nueva Equivalencia" Then
                '    tsmiNuevaEquivalencia.Visible = True
                'End If

                'If permission = "Unidades - Eliminar Equivalencia" Then
                '    tsmiEliminarEquivalencia.Visible = True
                'End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


    End Sub


    Private Sub MissionControl_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim queriesLogout(2) As String

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL") = 1 Then
            queriesLogout(0) = "UPDATE users SET bonline = 0 WHERE susername = '" & susername & "'"
        End If

        queriesLogout(1) = "UPDATE sessions SET ilogoutdate = " & getMySQLDate() & ", slogouttime = '" & getAppTime() & "' WHERE susername = '" & susername & "' AND susersession = '" & susersession & "'"

        executeTransactedSQLCommand(0, queriesLogout)

        closeTimedOutConnections()

    End Sub


    Private Sub MissionControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub MissionControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        If defineDBServer() = False Then

            MsgBox("¿Está prendida la máquina con la base de datos? No logro conectarme. ¿O quizás esté mal la configuración?", MsgBoxStyle.OkOnly, "Error")

            Dim op As New Opciones
            op.ShowDialog(Me)

            If op.DialogResult = Windows.Forms.DialogResult.OK Then
                If defineDBServer() = False Then
                    MsgBox("No encontré conexión con la base de datos de Oversight. Cerrando aplicación...", MsgBoxStyle.OkOnly, "Error")
                    System.Environment.Exit(0)
                End If
            Else
                System.Environment.Exit(0)
            End If

        End If

        setControlsByPermissions(Me.Name, susername)
        closeTimedOutConnections()
        dropIrrecoverableTempTables()
        getFilesIncorrectlyClosed()
        checkMessages(susername, Me.Location.X, Me.Location.Y)

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM basecardcompoundinputs") = 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Acabo de descubrir que se vació la tabla de Insumos Compuestos', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Acabo de descubrir que se vació la tabla de Insumos Compuestos. ¿Podrías revisar?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("Error grave: Pérdida de datos, requiere investigación de causas. Cerrando aplicación...", MsgBoxStyle.Exclamation, "Error Grave")

            Me.Close()
            Exit Sub

        End If

        'Si el hash de la licencia no coincide con el generado o no tiene, mostrar la ventana de licencia con boton seguir desactivado por 4 segundos estilo winrar

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


    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFiletsmiOpen.Click, tsGeneraltsbAbrir.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            ' TODO: Add code here to open the file.
        End If
    End Sub


    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFiletsmiSave.Click, tsGeneraltsbGuardar.Click
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: Add code here to save the current contents of the form to a file.
        End If
    End Sub


    Private Sub mnuFiletsmiPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFiletsmiPrint.Click

    End Sub


    Private Sub mnuFiletsmiPrintPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFiletsmiPrintPreview.Click

    End Sub


    Private Sub mnuFiletsmiPrintSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFiletsmiPrintSetup.Click

    End Sub


    Private Sub mnuFiletsmiLogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFiletsmiLogout.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queriesLogout(2) As String

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM sessions WHERE susername = '" & susername & "' AND ilogoutdate IS NULL") = 1 Then
            queriesLogout(0) = "UPDATE users SET bonline = 0 WHERE susername = '" & susername & "'"
        End If

        queriesLogout(1) = "UPDATE sessions SET ilogoutdate = " & getMySQLDate() & ", slogouttime = '" & getAppTime() & "' WHERE susername = '" & susername & "' AND susersession = '" & susersession & "'"

        If executeTransactedSQLCommand(0, queriesLogout) = True Then

            susername = ""
            bactive = False
            bonline = False
            suserfullname = ""
            suseremail = ""
            susersession = 0
            susermachinename = ""
            suserip = "0.0.0.0"

            Dim l As New Login
            l.Show()

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFiletsmiExit.Click
        Me.Close()
    End Sub


    Private Sub mnuEdittsmiUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdittsmiUndo.Click

    End Sub


    Private Sub mnuEdittsmiRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdittsmiRedo.Click

    End Sub


    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuEdittsmiCut.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub


    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuEdittsmiCopy.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub


    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuEdittsmiPaste.Click
        'Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.
    End Sub


    Private Sub mnuEdittsmiSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEdittsmiSelectAll.Click

    End Sub


    Private Sub mnuViewtsmiGeneralBar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewtsmiGeneralBar.Click
        Me.tsGeneral.Visible = Me.mnuViewtsmiGeneralBar.Checked
    End Sub


    Private Sub mnuViewtsmiStatusBar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewtsmiStatusBar.Click
        Me.ssMain.Visible = Me.mnuViewtsmiStatusBar.Checked
    End Sub


    Private Sub mnuToolstsmiModifyMyUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolstsmiModifyMyUser.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim au As New AgregarUsuario

        au.susername = susername
        au.bactive = bactive
        au.bonline = bonline
        au.suserfullname = suserfullname
        au.suseremail = suseremail
        au.susersession = susersession
        au.susermachinename = susermachinename
        au.suserip = suserip

        au.sselectedusername = susername
        au.sselecteduserfullname = suserfullname

        au.IsEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            au.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        au.ShowDialog(Me)
        Me.Visible = True

        If au.DialogResult = Windows.Forms.DialogResult.OK Then

            setControlsByPermissions(Me.Name, susername)

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub mnuToolstsmiOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolstsmiOptions.Click

        Dim op As New Opciones

        op.ShowDialog(Me)

    End Sub


    Private Sub mnuWindowtsmiCascade_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuWindowtsmiCascade.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub


    Private Sub mnuWindowtsmiTileVertical_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuWindowtsmiTileVertical.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub


    Private Sub mnuWindowtsmiTileHorizontal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuWindowtsmiTileHorizontal.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub


    Private Sub mnuWindowtsmiArrangeIcons_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuWindowtsmiArrangeIcons.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub


    Private Sub mnuWindowtsmiCloseAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuWindowtsmiCloseAll.Click

        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next

    End Sub


    Private Sub mnuHelptsmiContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelptsmiContents.Click

    End Sub


    Private Sub mnuHelptsmiIndex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelptsmiIndex.Click

    End Sub


    Private Sub mnuHelptsmiSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelptsmiSearch.Click

    End Sub


    Private Sub mnuHelptsmiAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelptsmiAbout.Click

        Dim so As New SobreOversight
        so.ShowDialog(Me)

    End Sub


    Private Sub tsmiVerPresupuestosBase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerPresupuestosBase.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bb As New BuscaBases

        bb.susername = susername
        bb.bactive = bactive
        bb.bonline = bonline
        bb.suserfullname = suserfullname
        bb.suseremail = suseremail
        bb.susersession = susersession
        bb.susermachinename = susermachinename
        bb.suserip = suserip

        bb.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bb.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bb.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoPresupuestoBase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoPresupuestoBase.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ab As New AgregarBase

        ab.susername = susername
        ab.bactive = bactive
        ab.bonline = bonline
        ab.suserfullname = suserfullname
        ab.suseremail = suseremail
        ab.susersession = susersession
        ab.susermachinename = susermachinename
        ab.suserip = suserip

        ab.isEdit = False
        ab.isHistoric = False

        If Me.WindowState = FormWindowState.Maximized Then
            ab.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ab.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarPresupuestoBase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarPresupuestoBase.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bb As New BuscaBases

        bb.susername = susername
        bb.bactive = bactive
        bb.bonline = bonline
        bb.suserfullname = suserfullname
        bb.suseremail = suseremail
        bb.susersession = susersession
        bb.susermachinename = susermachinename
        bb.suserip = suserip

        bb.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bb.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bb.ShowDialog(Me)
        Me.Visible = True

        If bb.DialogResult = Windows.Forms.DialogResult.OK Then

            If MsgBox("¿Realmente deseas eliminar este Presupuesto Base?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Presupuesto Base") = MsgBoxResult.Yes Then

                Dim queriesDelete(7) As String

                queriesDelete(0) = "DELETE FROM base WHERE ibaseid = " & bb.iprojectid
                queriesDelete(1) = "DELETE FROM baseprices WHERE ibaseid = " & bb.iprojectid
                queriesDelete(2) = "DELETE FROM basecards WHERE ibaseid = " & bb.iprojectid
                queriesDelete(3) = "DELETE FROM basecardinputs WHERE ibaseid = " & bb.iprojectid
                queriesDelete(4) = "DELETE FROM basecardcompoundinputs WHERE ibaseid = " & bb.iprojectid
                queriesDelete(5) = "DELETE FROM baseindirectcosts WHERE ibaseid = " & bb.iprojectid
                queriesDelete(6) = "DELETE FROM basetimber WHERE ibaseid = " & bb.iprojectid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If


            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerModelos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerModelos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bm As New BuscaModelos

        bm.susername = susername
        bm.bactive = bactive
        bm.bonline = bonline
        bm.suserfullname = suserfullname
        bm.suseremail = suseremail
        bm.susersession = susersession
        bm.susermachinename = susermachinename
        bm.suserip = suserip

        bm.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bm.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bm.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoModelo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoModelo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim am As New AgregarModelo

        am.susername = susername
        am.bactive = bactive
        am.bonline = bonline
        am.suserfullname = suserfullname
        am.suseremail = suseremail
        am.susersession = susersession
        am.susermachinename = susermachinename
        am.suserip = suserip

        am.isEdit = False
        am.isHistoric = False

        If Me.WindowState = FormWindowState.Maximized Then
            am.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        am.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarModelo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarModelo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bm As New BuscaModelos

        bm.susername = susername
        bm.bactive = bactive
        bm.bonline = bonline
        bm.suserfullname = suserfullname
        bm.suseremail = suseremail
        bm.susersession = susersession
        bm.susermachinename = susermachinename
        bm.suserip = suserip

        bm.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bm.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bm.ShowDialog(Me)
        Me.Visible = True

        If bm.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim mensaje As String = ""

            Dim contadorUso As Integer = 0
            contadorUso = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM projects WHERE slastmodelapplied = '" & getSQLQueryAsString(0, "SELECT smodelname FROM models WHERE imodelid = " & bm.imodelid) & "'")

            If contadorUso > 0 Then
                mensaje = "¿Estás seguro de que deseas eliminar este modelo? Lo has utilizado antes..."
            Else
                mensaje = "¿Estás seguro de que deseas eliminar este modelo?"
            End If

            If MsgBox(mensaje, MsgBoxStyle.YesNo, "Confirmación Eliminación de Modelo") = MsgBoxResult.Yes Then

                Dim queriesDelete(7) As String

                queriesDelete(0) = "DELETE FROM models WHERE imodelid = " & bm.imodelid
                queriesDelete(1) = "DELETE FROM modelindirectcosts WHERE imodelid = " & bm.imodelid
                queriesDelete(2) = "DELETE FROM modelprices WHERE imodelid = " & bm.imodelid
                queriesDelete(3) = "DELETE FROM modelcardcompoundinputs WHERE imodelid = " & bm.imodelid
                queriesDelete(4) = "DELETE FROM modelcardinputs WHERE imodelid = " & bm.imodelid
                queriesDelete(5) = "DELETE FROM modelcards WHERE imodelid = " & bm.imodelid
                queriesDelete(6) = "DELETE FROM modeltimber WHERE imodelid = " & bm.imodelid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If


            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerProyectos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerProyectos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaProyectos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoProyecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoProyecto.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ap As New AgregarProyecto

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.isEdit = False
        ap.isHistoric = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoProyectoDesdeModelo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoProyectoDesdeModelo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bm As New BuscaModelos

        bm.susername = susername
        bm.bactive = bactive
        bm.bonline = bonline
        bm.suserfullname = suserfullname
        bm.suseremail = suseremail
        bm.susersession = susersession
        bm.susermachinename = susermachinename
        bm.suserip = suserip

        bm.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bm.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bm.ShowDialog(Me)
        Me.Visible = True

        If bm.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queryCopiaModelo As String = ""
            Dim queryCopiaModeloIndirectos As String = ""
            Dim queryCopiaPreciosBase As String = ""
            Dim queryCopiaModeloInsumosCompuestos As String = ""
            Dim queryCopiaModeloInsumos As String = ""
            Dim queryCopiaModeloTarjetas As String = ""
            Dim queryCopiaModeloTimber As String = ""

            Dim newprojectid As Integer = 0

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"
            Dim baseid As Integer = 0

            fecha = getMySQLDate()
            hora = getAppTime()
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            newprojectid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iprojectid) + 1 IS NULL, 1, MAX(iprojectid) + 1) AS iprojectid FROM projects")

            queryCopiaModelo = "" & _
            "INSERT INTO projects " & _
            "SELECT " & newprojectid & ", " & fecha & ", '" & hora & "', smodelname, smodeltype, " & _
            "0, '', dmodellength, dmodelwidth, 0, 0, '', '', smodelname, 0, 0, 0, 0, 0, 0, " & _
            "dmodelIVApercentage, 0, '00:00:00', 0, '00:00:00', 0, '00:00:00', 1, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM models " & _
            "WHERE imodelid = " & bm.imodelid

            queryCopiaModeloIndirectos = "" & _
            "INSERT INTO projectindirectcosts SELECT " & newprojectid & ", icostid, " & _
            "smodelcostname, dmodelcost, dcompanyprojectedearnings, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM modelindirectcosts WHERE imodelid = " & bm.imodelid

            queryCopiaPreciosBase = "" & _
            "INSERT INTO projectprices SELECT " & newprojectid & ", iinputid, " & _
            "dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"

            queryCopiaModeloInsumosCompuestos = "" & _
            "INSERT INTO projectcardcompoundinputs SELECT " & newprojectid & ", icardid, iinputid, icompoundinputid, " & _
            "scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM modelcardcompoundinputs WHERE imodelid = " & bm.imodelid

            queryCopiaModeloInsumos = "" & _
            "INSERT INTO projectcardinputs SELECT " & newprojectid & ", icardid, iinputid, " & _
            "scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM modelcardinputs WHERE imodelid = " & bm.imodelid

            queryCopiaModeloTarjetas = "" & _
            "INSERT INTO projectcards SELECT " & newprojectid & ", icardid, " & _
            "scardlegacycategoryid, scardlegacyid, scarddescription, " & _
            "scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM modelcards WHERE imodelid = " & bm.imodelid

            queryCopiaModeloTimber = "" & _
            "INSERT INTO projecttimber SELECT " & newprojectid & ", iinputid, dinputtimberespesor, dinputtimberancho, " & _
            "dinputtimberlargo, dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM modeltimber WHERE imodelid = " & bm.imodelid

            Dim queries(7) As String

            queries(0) = queryCopiaModelo
            queries(1) = queryCopiaModeloIndirectos
            queries(2) = queryCopiaPreciosBase
            queries(3) = queryCopiaModeloInsumosCompuestos
            queries(4) = queryCopiaModeloInsumos
            queries(5) = queryCopiaModeloTarjetas
            queries(6) = queryCopiaModeloTimber

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Proyecto creado Exitosamente. Abriendo proyecto para edición...", MsgBoxStyle.OkOnly, "")

                Dim ap As New AgregarProyecto

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isEdit = True
                ap.isHistoric = False

                ap.iprojectid = newprojectid

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True

            Else

                MsgBox("Error al crear un Proyecto desde un Modelo", MsgBoxStyle.OkOnly, "")

            End If



        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiDuplicarProyecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiDuplicarProyecto.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaProyectos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queryCopiaProyecto As String = ""
            Dim queryCopiaProyectoIndirectos As String = ""
            Dim queryCopiaProyectoTarjetas As String = ""
            Dim queryCopiaProyectoInsumos As String = ""
            Dim queryCopiaProyectoInsumosCompuestos As String = ""
            Dim queryCopiaPreciosBase As String = ""
            Dim queryCopiaProyectoExplosion As String = ""
            Dim queryCopiaProyectoTimber As String = ""
            Dim queryCopiaProyectoGastosAdmin As String = ""

            Dim newprojectid As Integer = 0

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"
            Dim baseid As Integer = 0

            fecha = getMySQLDate()
            hora = getAppTime()
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            newprojectid = getSQLQueryAsInteger(0, "SELECT IF(MAX(iprojectid) + 1 IS NULL, 1, MAX(iprojectid) + 1) AS iprojectid FROM projects")

            queryCopiaProyecto = "" & _
            "INSERT INTO projects " & _
            "SELECT " & newprojectid & ", " & fecha & ", '" & hora & "', CONCAT(sprojectname, ' copia') AS sprojectname, sprojecttype, " & _
            "ipeopleid, sprojectcompanyname, dprojectlength, dprojectwidth, dterrainlength, dterrainwidth, " & _
            "sterrainlocation, sprojectfileslocation, slastmodelapplied, dprojectindirectpercentagedefault, " & _
            "dprojectgainpercentagedefault, dprojectbuildingcommission, dprojectclosingcommission, " & _
            "dprojectrealbuildingcommission, dprojectrealclosingcommission, dprojectIVApercentage, " & _
            "iprojectstarteddate, sprojectstartedtime, iprojectforecastedclosingdate, sprojectforecastedclosingtime, " & _
            "iprojectrealclosingdate, sprojectrealclosingtime, sprojectTaxApplicable, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projects " & _
            "WHERE iprojectid = " & bp.iprojectid

            queryCopiaProyectoIndirectos = "" & _
            "INSERT INTO projectindirectcosts SELECT " & newprojectid & ", icostid, " & _
            "sprojectcostname, dprojectcost, dcompanyprojectedearnings, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projectindirectcosts WHERE iprojectid = " & bp.iprojectid

            queryCopiaProyectoTarjetas = "" & _
            "INSERT INTO projectcards SELECT " & newprojectid & ", icardid, " & _
            "scardlegacycategoryid, scardlegacyid, scarddescription, " & _
            "scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projectcards WHERE iprojectid = " & bp.iprojectid

            queryCopiaProyectoInsumos = "" & _
            "INSERT INTO projectcardinputs SELECT " & newprojectid & ", icardid, iinputid, " & _
            "scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projectcardinputs WHERE iprojectid = " & bp.iprojectid

            queryCopiaProyectoInsumosCompuestos = "" & _
            "INSERT INTO projectcardcompoundinputs SELECT " & newprojectid & ", icardid, iinputid, icompoundinputid, " & _
            "scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projectcardcompoundinputs WHERE iprojectid = " & bp.iprojectid

            queryCopiaPreciosBase = "" & _
            "INSERT INTO projectprices SELECT " & newprojectid & ", iinputid, " & _
            "dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"

            queryCopiaProyectoExplosion = "" & _
            "INSERT INTO projectexplosion SELECT " & newprojectid & ", iinputid, dinputrealqty, " & _
            fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projectexplosion WHERE iprojectid = " & bp.iprojectid

            queryCopiaProyectoTimber = "" & _
            "INSERT INTO projecttimber SELECT " & newprojectid & ", iinputid, dinputtimberespesor, dinputtimberancho, " & _
            "dinputtimberlargo, dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projecttimber WHERE iprojectid = " & bp.iprojectid

            queryCopiaProyectoGastosAdmin = "" & _
            "INSERT INTO projectadmincosts SELECT " & newprojectid & ", iadmincostid, " & _
            "sprojectadmincostname, dprojectadmincost, " & fecha & ", '" & hora & "', '" & susername & "' " & _
            "FROM projectadmincosts WHERE iprojectid = " & bp.iprojectid

            Dim queries(9) As String

            queries(0) = queryCopiaProyecto
            queries(1) = queryCopiaProyectoIndirectos
            queries(2) = queryCopiaProyectoTarjetas
            queries(3) = queryCopiaProyectoInsumos
            queries(4) = queryCopiaProyectoInsumosCompuestos
            queries(5) = queryCopiaPreciosBase
            queries(6) = queryCopiaProyectoExplosion
            queries(7) = queryCopiaProyectoTimber
            queries(8) = queryCopiaProyectoGastosAdmin

            If executeTransactedSQLCommand(0, queries) = True Then

                MsgBox("Duplicación Exitosa. Abriendo proyecto duplicado para edición...", MsgBoxStyle.OkOnly, "")

                Dim ap As New AgregarProyecto

                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isEdit = True
                ap.isHistoric = False

                ap.iprojectid = newprojectid

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True

            Else

                MsgBox("Error de Copiado. Probablemente un error de red. Intente nuevamente.", MsgBoxStyle.OkOnly, "")

            End If


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarProyecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarProyecto.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaProyectos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            If MsgBox("¿Estás seguro de que deseas eliminar este proyecto?", MsgBoxStyle.YesNo, "Eliminar Proyecto") = MsgBoxResult.Yes Then

                Dim queriesDelete(11) As String

                queriesDelete(0) = "DELETE FROM projects WHERE iprojectid = " & bp.iprojectid
                queriesDelete(1) = "DELETE FROM projectindirectcosts WHERE iprojectid = " & bp.iprojectid
                queriesDelete(2) = "DELETE FROM projectprices WHERE iprojectid = " & bp.iprojectid
                queriesDelete(3) = "DELETE FROM projectcardcompoundinputs WHERE iprojectid = " & bp.iprojectid
                queriesDelete(4) = "DELETE FROM projectcardinputs WHERE iprojectid = " & bp.iprojectid
                queriesDelete(5) = "DELETE FROM projectcards WHERE iprojectid = " & bp.iprojectid
                queriesDelete(6) = "DELETE FROM projecttimber WHERE iprojectid = " & bp.iprojectid
                queriesDelete(7) = "DELETE FROM projectexplosion WHERE iprojectid = " & bp.iprojectid
                queriesDelete(8) = "DELETE FROM projectadmincosts WHERE iprojectid = " & bp.iprojectid
                queriesDelete(9) = "DELETE FROM supplierinvoiceprojects WHERE iprojectid = " & bp.iprojectid
                queriesDelete(10) = "DELETE FROM incomeprojects WHERE iprojectid = " & bp.iprojectid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerMaterialesPrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerMaterialesPrecios.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim dsPermisosVerInsumo As DataSet = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & susername & "' AND swindowname = 'AgregarInsumo'")

        Dim permisoVerInsumo As Boolean = False

        For i = 0 To dsPermisosVerInsumo.Tables(0).Rows.Count - 1

            If dsPermisosVerInsumo.Tables(0).Rows(i).Item("spermission") = "Ver" Then
                permisoVerInsumo = True
            End If

        Next i

        If permisoVerInsumo = False Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            MsgBox("No tienes los permisos suficientes para realizar esta acción", MsgBoxStyle.Information, "Permisos")
            Exit Sub

        End If


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

        bi.IsEdit = True

        'Drop-Create ALL base tables

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

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

        'Show Search Window

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        'I need to do the save method of the AddBase here.

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

        queriesSave(3) = "" & _
        "DELETE " & _
        "FROM basecards " & _
        "WHERE ibaseid = " & baseid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

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

        queriesSave(12) = "" & _
        "DELETE " & _
        "FROM baseprices " & _
        "WHERE ibaseid = " & baseid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        queriesSave(13) = "" & _
        "DELETE " & _
        "FROM basetimber " & _
        "WHERE ibaseid = " & baseid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

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
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

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

        queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a Materiales directamente (Insumo ID: " & bi.iinputid & ", Desc: " & bi.sinputdescription & "', 'OK')"

        If executeTransactedSQLCommand(0, queriesSave) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


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

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoMaterial.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bipni As New BuscaInsumosPreguntaTipoNuevoInsumo
        bipni.susername = susername
        bipni.bactive = bactive
        bipni.bonline = bonline
        bipni.suserfullname = suserfullname
        bipni.suseremail = suseremail
        bipni.susersession = susersession
        bipni.susermachinename = susermachinename
        bipni.suserip = suserip

        Me.Visible = False
        bipni.ShowDialog(Me)
        Me.Visible = True

        If bipni.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsPermisosNuevoInsumoyGuardarInsumo As DataSet = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & susername & "' AND swindowname = 'AgregarInsumo'")

            Dim permisoVerInsumo As Boolean = False
            Dim permisoGuardarInsumo As Boolean = False

            For i = 0 To dsPermisosNuevoInsumoyGuardarInsumo.Tables(0).Rows.Count - 1

                If dsPermisosNuevoInsumoyGuardarInsumo.Tables(0).Rows(i).Item("spermission") = "Modificar" Then
                    permisoGuardarInsumo = True
                End If

                If dsPermisosNuevoInsumoyGuardarInsumo.Tables(0).Rows(i).Item("spermission") = "Ver" Then
                    permisoVerInsumo = True
                End If

            Next i

            If permisoGuardarInsumo = True And permisoVerInsumo = True Then
                'Continue
            Else

                MsgBox("No tienes los permisos suficientes para realizar esta acción", MsgBoxStyle.Information, "Permisos")
                Exit Sub

            End If

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            If bipni.iselectedoption = 1 Then

                'Nuevo Insumo Normal

                Dim ai As New AgregarInsumo
                ai.susername = susername
                ai.bactive = bactive
                ai.bonline = bonline
                ai.suserfullname = suserfullname
                ai.suseremail = suseremail
                ai.susersession = susersession
                ai.susermachinename = susermachinename
                ai.suserip = suserip

                ai.iprojectid = baseid

                ai.IsEdit = False
                ai.IsHistoric = False
                ai.IsModel = False
                ai.IsBase = True

                'Drop-Create ALL base tables

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

                'Show Search Window

                If Me.WindowState = FormWindowState.Maximized Then
                    ai.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ai.ShowDialog(Me)
                Me.Visible = True

                'I need to do the save method of the AddBase here.

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

                queriesSave(3) = "" & _
                "DELETE " & _
                "FROM basecards " & _
                "WHERE ibaseid = " & baseid & " AND " & _
                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

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

                queriesSave(12) = "" & _
                "DELETE " & _
                "FROM baseprices " & _
                "WHERE ibaseid = " & baseid & " AND " & _
                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

                queriesSave(13) = "" & _
                "DELETE " & _
                "FROM basetimber " & _
                "WHERE ibaseid = " & baseid & " AND " & _
                "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

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
                "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

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

                queriesSave(28) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios a nuevo Material directamente (Insumo ID: " & ai.iinputid & "', 'OK')"

                If executeTransactedSQLCommand(0, queriesSave) = True Then
                    MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
                Else
                    MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If


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


            ElseIf bipni.iselectedoption = 2 Then

                MsgBox("Si quieres crear un Insumo Compuesto, te sugiero que vayas a Presupuestos Base y lo crees desde una tarjeta", MsgBoxStyle.OkOnly, "Sugerencias")
                Exit Sub

                'Dim aic As New AgregarInsumoCompuesto
                'aic.susername = susername
                'aic.bactive = bactive
                'aic.bonline = bonline
                'aic.suserfullname = suserfullname
                'aic.suseremail = suseremail
                'aic.susersession = susersession
                'aic.susermachinename = susermachinename
                'aic.suserip = suserip

                'aic.iprojectid = baseid

                'aic.IsEdit = False

                'aic.IsHistoric = False
                'aic.IsModel = False
                'aic.IsBase = True

                ''Drop-Create ALL base tables

                'Dim queriesCreation(12) As String

                'queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid
                'queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                'queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts"
                'queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts" & " ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                'queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards"
                'queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                'queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs"
                'queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                'queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs"
                'queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs" & " ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                'queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices"
                'queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices" & " ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                'executeTransactedSQLCommand(0, queriesCreation)

                ''Insert Info from Base

                'Dim queriesInsert(6) As String

                'queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " SELECT * FROM base WHERE ibaseid = " & baseid
                'queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts SELECT * FROM baseindirectcosts WHERE ibaseid = " & baseid
                'queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards SELECT * FROM basecards WHERE ibaseid = " & baseid
                'queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs SELECT * FROM basecardinputs WHERE ibaseid = " & baseid
                'queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs SELECT * FROM basecardcompoundinputs WHERE ibaseid = " & baseid
                'queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices SELECT * FROM baseprices WHERE ibaseid = " & baseid

                'executeTransactedSQLCommand(0, queriesInsert)

                ''Show Search Window

                'Me.Visible = False
                'aic.ShowDialog(Me)
                'Me.Visible = True

                ''I need to do the save method of the AddBase here.

                'Dim queries(19) As String

                'queries(0) = "" & _
                '"DELETE " & _
                '"FROM base " & _
                '"WHERE ibaseid = " & baseid & " AND " & _
                '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tb WHERE base.ibaseid = tb.ibaseid) "

                'queries(1) = "" & _
                '"DELETE " & _
                '"FROM baseindirectcosts " & _
                '"WHERE ibaseid = " & baseid & " AND " & _
                '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

                'queries(2) = "" & _
                '"DELETE " & _
                '"FROM basecards " & _
                '"WHERE ibaseid = " & baseid & " AND " & _
                '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

                'queries(3) = "" & _
                '"DELETE " & _
                '"FROM basecardinputs " & _
                '"WHERE ibaseid = " & baseid & " AND " & _
                '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

                'queries(4) = "" & _
                '"DELETE " & _
                '"FROM basecardcompoundinputs " & _
                '"WHERE ibaseid = " & baseid & " AND " & _
                '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

                'queries(5) = "" & _
                '"DELETE " & _
                '"FROM baseprices " & _
                '"WHERE ibaseid = " & baseid & " AND " & _
                '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

                'queries(6) = "" & _
                '"UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

                'queries(7) = "" & _
                '"UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

                'queries(8) = "" & _
                '"UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

                'queries(9) = "" & _
                '"UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

                'queries(10) = "" & _
                '"UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

                'queries(11) = "" & _
                '"UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

                'queries(12) = "" & _
                '"INSERT INTO base " & _
                '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & " tb " & _
                '"WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

                'queries(13) = "" & _
                '"INSERT INTO baseindirectcosts " & _
                '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts tbic " & _
                '"WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

                'queries(14) = "" & _
                '"INSERT INTO basecards " & _
                '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards tbc " & _
                '"WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

                'queries(15) = "" & _
                '"INSERT INTO basecardinputs " & _
                '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs tbci " & _
                '"WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

                'queries(16) = "" & _
                '"INSERT INTO basecardcompoundinputs " & _
                '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs tbcci " & _
                '"WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

                'queries(17) = "" & _
                '"INSERT INTO baseprices " & _
                '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices tbp " & _
                '"WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

                'queries(18) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Insumo (directo de MissionControl) : " & aic.iinputid & "', 'OK')"

                'executeTransactedSQLCommand(0, queries)

                ''Drop ALL base tables

                'Dim queriesDrop(6) As String

                'queriesDrop(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid
                'queriesDrop(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "IndirectCosts"
                'queriesDrop(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Cards"
                'queriesDrop(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardInputs"
                'queriesDrop(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "CardCompoundInputs"
                'queriesDrop(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & baseid & "Prices"
                'executeTransactedSQLCommand(0, queriesDrop)

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarMaterial.Click

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

        bi.IsEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        If bi.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queryBaseCardInputs As String = ""
            Dim queryModelCardInputs As String = ""
            Dim queryProjectCardInputs As String = ""
            Dim queryShipmentInputs As String = ""
            Dim queryOrderInputs As String = ""
            Dim querySupplierInvoiceInputs As String = ""
            Dim queryInputsPhysicalInventory As String = ""

            Dim conteoBaseCardInputs As Integer = 0
            Dim conteoModelCardInputs As Integer = 0
            Dim conteoProjectCardInputs As Integer = 0
            Dim conteoShipmentInputs As Integer = 0
            Dim conteoOrderInputs As Integer = 0
            Dim conteoSupplierInvoiceInputs As Integer = 0
            Dim conteoInputsPhysicalInventory As Integer = 0

            queryBaseCardInputs = "" & _
            "SELECT COUNT(*) " & _
            "FROM basecardinputs " & _
            "WHERE iinputid = " & bi.iinputid

            queryModelCardInputs = "" & _
            "SELECT COUNT(*) " & _
            "FROM modelcardinputs " & _
            "WHERE iinputid = " & bi.iinputid

            queryProjectCardInputs = "" & _
            "SELECT COUNT(*) " & _
            "FROM projectcardinputs " & _
            "WHERE iinputid = " & bi.iinputid

            queryShipmentInputs = "" & _
            "SELECT COUNT(*) " & _
            "FROM shipmentinputs " & _
            "WHERE iinputid = " & bi.iinputid

            queryOrderInputs = "" & _
            "SELECT COUNT(*) " & _
            "FROM orderinputs " & _
            "WHERE iinputid = " & bi.iinputid

            querySupplierInvoiceInputs = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoiceinputs " & _
            "WHERE iinputid = " & bi.iinputid

            queryInputsPhysicalInventory = "" & _
            "SELECT COUNT(*) " & _
            "FROM inputsphysicalinventory " & _
            "WHERE iinputid = " & bi.iinputid


            conteoBaseCardInputs = getSQLQueryAsInteger(0, queryBaseCardInputs)
            conteoModelCardInputs = getSQLQueryAsInteger(0, queryModelCardInputs)
            conteoProjectCardInputs = getSQLQueryAsInteger(0, queryProjectCardInputs)
            conteoShipmentInputs = getSQLQueryAsInteger(0, queryShipmentInputs)
            conteoOrderInputs = getSQLQueryAsInteger(0, queryOrderInputs)
            conteoSupplierInvoiceInputs = getSQLQueryAsInteger(0, querySupplierInvoiceInputs)
            conteoInputsPhysicalInventory = getSQLQueryAsInteger(0, queryInputsPhysicalInventory)

            If conteoBaseCardInputs + conteoModelCardInputs + conteoProjectCardInputs + conteoShipmentInputs + conteoOrderInputs + conteoSupplierInvoiceInputs + conteoInputsPhysicalInventory = 0 Then

                If MsgBox("¿Estás seguro de que deseas eliminar este insumo? Ya no estará disponible para las Tarjetas o para Proyectos", MsgBoxStyle.YesNo, "Eliminar Insumo") = MsgBoxResult.Yes Then

                    If executeSQLCommand(0, "DELETE FROM inputs WHERE iinputid = " & bi.iinputid) Then
                        MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                    Else
                        MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                    End If

                End If

            Else

                Dim lugares As String = "("

                If conteoBaseCardInputs > 0 Then
                    lugares = lugares & "Presupuesto Base, "
                End If

                If conteoModelCardInputs > 0 Then
                    lugares = lugares & "Modelos, "
                End If

                If conteoProjectCardInputs > 0 Then
                    lugares = lugares & "Proyectos, "
                End If

                If conteoShipmentInputs > 0 Then
                    lugares = lugares & "Envíos, "
                End If

                If conteoOrderInputs > 0 Then
                    lugares = lugares & "Órdenes, "
                End If

                If conteoSupplierInvoiceInputs > 0 Then
                    lugares = lugares & "Facturas, "
                End If

                If conteoInputsPhysicalInventory > 0 Then
                    lugares = lugares & "Inventario, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No pude eliminar este Insumo debido a que está utilizado en operaciones históricas de la compañía " & lugares, MsgBoxStyle.YesNo, "Error Eliminación Insumo")

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerIngresos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerIngresos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaIngresos

        bi.susername = susername
        bi.bactive = bactive
        bi.bonline = bonline
        bi.suserfullname = suserfullname
        bi.suseremail = suseremail
        bi.susersession = susersession
        bi.susermachinename = susermachinename
        bi.suserip = suserip

        bi.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bi.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bi.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoIngreso.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ai As New AgregarIngreso

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

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarIngreso.Click

        If MsgBox("Si deseas reasignar un Ingreso a otro proyecto, intenta editandolo. Eliminar un Ingreso es permanentemente eliminarlo de la base de datos. ¿Deseas continuar eliminando un ingreso?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bi As New BuscaIngresos

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

            Dim queryAssets As String
            Dim queryProjects As String

            Dim conteoAssets As Integer = 0
            Dim conteoProjects As Integer = 0

            queryAssets = "" & _
            "SELECT COUNT(*) " & _
            "FROM incomeassets " & _
            "WHERE iincomeid = " & bi.iincomeid

            queryProjects = "" & _
            "SELECT COUNT(*) " & _
            "FROM incomeprojects " & _
            "WHERE iincomeid = " & bi.iincomeid

            conteoAssets = getSQLQueryAsInteger(0, queryAssets)
            conteoProjects = getSQLQueryAsInteger(0, queryProjects)

            If conteoAssets + conteoProjects > 0 Then

                Dim lugares As String = "("

                If conteoAssets > 0 Then
                    lugares = lugares & "Activos, "
                End If

                If conteoProjects > 0 Then
                    lugares = lugares & "Proyectos, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar este Ingreso, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM incomes WHERE iincomeid = " & bi.iincomeid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerInventarioDeMateriales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerInventarioDeMateriales.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim inv As New InventarioDeInsumos
        inv.susername = susername
        inv.bactive = bactive
        inv.bonline = bonline
        inv.suserfullname = suserfullname
        inv.suseremail = suseremail
        inv.susersession = susersession
        inv.susermachinename = susermachinename
        inv.suserip = suserip

        inv.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            inv.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        inv.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerActivos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerActivos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ba As New BuscaActivos

        ba.susername = susername
        ba.bactive = bactive
        ba.bonline = bonline
        ba.suserfullname = suserfullname
        ba.suseremail = suseremail
        ba.susersession = susersession
        ba.susermachinename = susermachinename
        ba.suserip = suserip

        ba.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            ba.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ba.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoActivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoActivo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim aa As New AgregarActivo

        aa.susername = susername
        aa.bactive = bactive
        aa.bonline = bonline
        aa.suserfullname = suserfullname
        aa.suseremail = suseremail
        aa.susersession = susersession
        aa.susermachinename = susermachinename
        aa.suserip = suserip

        aa.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            aa.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        aa.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarActivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarActivo.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

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

            Dim querySupplierInvoiceAssets As String
            Dim queryShipmentCarUsed As String
            Dim queryIncomeAssets As String
            Dim queryAssetsPhysicalInventory As String

            Dim conteoSupplierInvoiceAssets As Integer = 0
            Dim conteoshipmentcarused As Integer = 0
            Dim conteoIncomeAssets As Integer = 0
            Dim conteoAssetsPhysicalInventory As Integer = 0

            querySupplierInvoiceAssets = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoiceassets " & _
            "WHERE iassetid = " & ba.iassetid

            queryShipmentCarUsed = "" & _
            "SELECT COUNT(*) " & _
            "FROM shipmentcarused " & _
            "WHERE iassetid = " & ba.iassetid

            queryIncomeAssets = "" & _
            "SELECT COUNT(*) " & _
            "FROM incomeassets " & _
            "WHERE iassetid = " & ba.iassetid

            queryAssetsPhysicalInventory = "" & _
            "SELECT COUNT(*) " & _
            "FROM assetsphysicalinventory " & _
            "WHERE iassetid = " & ba.iassetid

            conteoSupplierInvoiceAssets = getSQLQueryAsInteger(0, querySupplierInvoiceAssets)
            conteoshipmentcarused = getSQLQueryAsInteger(0, queryShipmentCarUsed)
            conteoIncomeAssets = getSQLQueryAsInteger(0, queryIncomeAssets)
            conteoAssetsPhysicalInventory = getSQLQueryAsInteger(0, queryAssetsPhysicalInventory)

            If conteoSupplierInvoiceAssets + conteoshipmentcarused + conteoIncomeAssets + conteoAssetsPhysicalInventory > 0 Then

                Dim lugares As String = "("

                If conteoSupplierInvoiceAssets > 0 Then
                    lugares = lugares & "Facturas de Proveedores, "
                End If

                If conteoshipmentcarused > 0 Then
                    lugares = lugares & "Envios, "
                End If

                If conteoIncomeAssets > 0 Then
                    lugares = lugares & "Ingresos, "
                End If

                If conteoAssetsPhysicalInventory > 0 Then
                    lugares = lugares & "Inventario, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar este Activo, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM assets WHERE iassetid = " & ba.iassetid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerCuentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerCuentas.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bcc As New BuscaCuentasCompania

        bcc.susername = susername
        bcc.bactive = bactive
        bcc.bonline = bonline
        bcc.suserfullname = suserfullname
        bcc.suseremail = suseremail
        bcc.susersession = susersession
        bcc.susermachinename = susermachinename
        bcc.suserip = suserip

        bcc.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bcc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bcc.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaCuenta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim acc As New AgregarCuentaCompania

        acc.susername = susername
        acc.bactive = bactive
        acc.bonline = bonline
        acc.suserfullname = suserfullname
        acc.suseremail = suseremail
        acc.susersession = susersession
        acc.susermachinename = susermachinename
        acc.suserip = suserip

        acc.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            acc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        acc.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarCuenta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bcc As New BuscaCuentasCompania

        bcc.susername = susername
        bcc.bactive = bactive
        bcc.bonline = bonline
        bcc.suserfullname = suserfullname
        bcc.suseremail = suseremail
        bcc.susersession = susersession
        bcc.susermachinename = susermachinename
        bcc.suserip = suserip

        bcc.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bcc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bcc.ShowDialog(Me)
        Me.Visible = True

        If bcc.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queryPayments As String
            Dim queryIncomes As String

            Dim conteoPayments As Integer = 0
            Dim conteoIncomes As Integer = 0

            queryPayments = "" & _
            "SELECT COUNT(*) " & _
            "FROM payments " & _
            "WHERE ioriginaccountid = " & bcc.iaccountid

            queryIncomes = "" & _
            "SELECT COUNT(*) " & _
            "FROM incomes " & _
            "WHERE idestinationaccountid = " & bcc.iaccountid

            conteoPayments = getSQLQueryAsInteger(0, queryPayments)
            conteoIncomes = getSQLQueryAsInteger(0, queryIncomes)

            If conteoPayments + conteoIncomes > 0 Then

                Dim lugares As String = "("

                If conteoPayments > 0 Then
                    lugares = lugares & "Pagos, "
                End If

                If conteoIncomes > 0 Then
                    lugares = lugares & "Ingresos, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar esta Cuenta, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM companyaccounts WHERE iaccountid = " & bcc.iaccountid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerSaldoEnCuentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerSaldoEnCuentas.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bcc As New SaldoEnCuentas

        bcc.susername = susername
        bcc.bactive = bactive
        bcc.bonline = bonline
        bcc.suserfullname = suserfullname
        bcc.suseremail = suseremail
        bcc.susersession = susersession
        bcc.susermachinename = susermachinename
        bcc.suserip = suserip

        bcc.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bcc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bcc.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    'Private Sub tsmiVerÓrdenesDeMateriales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerOrdenesDeMateriales.Click

    'End Sub

    'Private Sub tsmiNuevaOrdenDeMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaOrdenDeMaterial.Click

    'End Sub

    'Private Sub tsmiEliminarOrdenesDeMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarOrdenesDeMaterial.Click

    'End Sub

    'Private Sub tsmiVerEnvíosDeMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerEnvíosDeMaterial.Click

    'End Sub

    'Private Sub tsmiNuevoEnvíoDeMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoEnvíoDeMaterial.Click

    'End Sub

    'Private Sub tsmiEliminarEnvíoDeMaterial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarEnvíoDeMaterial.Click

    'End Sub


    Private Sub tsmiVerValesDeGasolina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerValesDeGasolina.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bv As New BuscaVales

        bv.susername = susername
        bv.bactive = bactive
        bv.bonline = bonline
        bv.suserfullname = suserfullname
        bv.suseremail = suseremail
        bv.susersession = susersession
        bv.susermachinename = susermachinename
        bv.suserip = suserip

        bv.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bv.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bv.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoValeDeGasolina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoValeDeGasolina.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim av As New AgregarVale

        av.susername = susername
        av.bactive = bactive
        av.bonline = bonline
        av.suserfullname = suserfullname
        av.suseremail = suseremail
        av.susersession = susersession
        av.susermachinename = susermachinename
        av.suserip = suserip

        av.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            av.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        av.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarValeDeGasolina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarValeDeGasolina.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bv As New BuscaVales

        bv.susername = susername
        bv.bactive = bactive
        bv.bonline = bonline
        bv.suserfullname = suserfullname
        bv.suseremail = suseremail
        bv.susersession = susersession
        bv.susermachinename = susermachinename
        bv.suserip = suserip

        bv.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bv.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bv.ShowDialog(Me)
        Me.Visible = True

        If bv.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queriesDelete(3) As String

            queriesDelete(0) = "DELETE FROM gasvouchers WHERE igasvoucherid = " & bv.igasvoucherid

            If executeTransactedSQLCommand(0, queriesDelete) = True Then
                MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
            Else
                MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerDirectorio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerDirectorio.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bed As New BuscaEnDirectorio

        bed.susername = susername
        bed.bactive = bactive
        bed.bonline = bonline
        bed.suserfullname = suserfullname
        bed.suseremail = suseremail
        bed.susersession = susersession
        bed.susermachinename = susermachinename
        bed.suserip = suserip

        bed.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bed.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bed.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaPersonaProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaPersonaProveedor.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bedpn As New BuscaEnDirectorioPreguntaNuevo
        bedpn.susername = susername
        bedpn.bactive = bactive
        bedpn.bonline = bonline
        bedpn.suserfullname = suserfullname
        bedpn.suseremail = suseremail
        bedpn.susersession = susersession
        bedpn.susermachinename = susermachinename
        bedpn.suserip = suserip

        bedpn.ShowDialog(Me)

        If bedpn.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim fecha As Integer = 0
            Dim hora As String = ""

            fecha = getMySQLDate()
            hora = getAppTime()

            If bedpn.iselectedoption = 1 Then

                Dim ap As New AgregarPersona
                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isEdit = False

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            ElseIf bedpn.iselectedoption = 2 Then

                Dim ap As New AgregarProveedor
                ap.susername = susername
                ap.bactive = bactive
                ap.bonline = bonline
                ap.suserfullname = suserfullname
                ap.suseremail = suseremail
                ap.susersession = susersession
                ap.susermachinename = susermachinename
                ap.suserip = suserip

                ap.isEdit = False

                If Me.WindowState = FormWindowState.Maximized Then
                    ap.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                ap.ShowDialog(Me)
                Me.Visible = True


            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarPersonaProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarPersonaProveedor.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bed As New BuscaEnDirectorio

        bed.susername = susername
        bed.bactive = bactive
        bed.bonline = bonline
        bed.suserfullname = suserfullname
        bed.suseremail = suseremail
        bed.susersession = susersession
        bed.susermachinename = susermachinename
        bed.suserip = suserip

        If Me.WindowState = FormWindowState.Maximized Then
            bed.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bed.ShowDialog(Me)
        Me.Visible = True

        If bed.DialogResult = Windows.Forms.DialogResult.OK Then

            If bed.stype = "Persona No Cliente" Or bed.stype = "CLIENTE" Then

                Dim queryShipments As String
                Dim queryOrders As String
                Dim queryGasVouchers As String
                Dim querySupplierInvoices As String
                Dim queryClientes As String
                Dim queryIncomes As String

                Dim conteoShipments As Integer = 0
                Dim conteoOrders As Integer = 0
                Dim conteoGasVouchers As Integer = 0
                Dim conteoSupplierInvoices As Integer = 0
                Dim conteoClientes As Integer = 0
                Dim conteoIncomes As Integer = 0

                queryShipments = "" & _
                "SELECT COUNT(*) " & _
                "FROM shipments " & _
                "WHERE idriverid = " & bed.ipeopleid & " OR ireceiverid = " & bed.ipeopleid

                queryOrders = "" & _
                "SELECT COUNT(*) " & _
                "FROM orders " & _
                "WHERE ipeopleid = " & bed.ipeopleid

                queryGasVouchers = "" & _
                "SELECT COUNT(*) " & _
                "FROM gasvouchers " & _
                "WHERE ipeopleid = " & bed.ipeopleid

                querySupplierInvoices = "" & _
                "SELECT COUNT(*) " & _
                "FROM supplierinvoices " & _
                "WHERE ipeopleid = " & bed.ipeopleid

                queryClientes = "" & _
                "SELECT COUNT(*) " & _
                "FROM projects " & _
                "WHERE ipeopleid = " & bed.ipeopleid

                queryIncomes = "" & _
                "SELECT COUNT(*) " & _
                "FROM incomes " & _
                "WHERE ireceiverid = " & bed.ipeopleid

                conteoShipments = getSQLQueryAsInteger(0, queryShipments)
                conteoOrders = getSQLQueryAsInteger(0, queryOrders)
                conteoGasVouchers = getSQLQueryAsInteger(0, queryGasVouchers)
                conteoSupplierInvoices = getSQLQueryAsInteger(0, querySupplierInvoices)
                conteoClientes = getSQLQueryAsInteger(0, queryClientes)
                conteoIncomes = getSQLQueryAsInteger(0, queryIncomes)

                If conteoShipments + conteoOrders + conteoGasVouchers + conteoSupplierInvoices + conteoClientes + conteoIncomes > 0 Then

                    Dim lugares As String = "("

                    If conteoShipments > 0 Then
                        lugares = lugares & "Envíos, "
                    End If

                    If conteoOrders > 0 Then
                        lugares = lugares & "Órdenes, "
                    End If

                    If conteoGasVouchers > 0 Then
                        lugares = lugares & "Vales de Gasolina, "
                    End If

                    If conteoSupplierInvoices > 0 Then
                        lugares = lugares & "Facturas (de Proveedores), "
                    End If

                    If conteoClientes > 0 Then
                        lugares = lugares & "Proyectos, "
                    End If

                    If conteoIncomes > 0 Then
                        lugares = lugares & "Pagos (Ingresos), "
                    End If

                    lugares = lugares & ")"
                    lugares = lugares.Replace(", )", ")")

                    MsgBox("No puedo eliminar esta Persona, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                    Exit Sub

                Else

                    Dim queriesDelete(3) As String

                    queriesDelete(0) = "DELETE FROM people WHERE ipeopleid = " & bed.ipeopleid
                    queriesDelete(1) = "DELETE FROM peoplephonenumbers WHERE ipeopleid = " & bed.ipeopleid
                    queriesDelete(2) = "DELETE FROM suppliercontacts WHERE ipeopleid = " & bed.ipeopleid

                    If executeTransactedSQLCommand(0, queriesDelete) = True Then
                        MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                    Else
                        MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                    End If


                End If

            Else

                Dim queryShipments As String
                Dim queryOrders As String
                Dim querySupplierInvoices As String

                Dim conteoShipments As Integer = 0
                Dim conteoOrders As Integer = 0
                Dim conteoSupplierInvoices As Integer = 0


                queryShipments = "" & _
                "SELECT COUNT(*) " & _
                "FROM shipments " & _
                "WHERE isupplierid = " & bed.isupplierid


                queryOrders = "" & _
                "SELECT COUNT(*) " & _
                "FROM orders " & _
                "WHERE isupplierid = " & bed.isupplierid


                querySupplierInvoices = "" & _
                "SELECT COUNT(*) " & _
                "FROM supplierinvoices " & _
                "WHERE isupplierid = " & bed.isupplierid


                conteoShipments = getSQLQueryAsInteger(0, queryShipments)
                conteoOrders = getSQLQueryAsInteger(0, queryOrders)
                conteoSupplierInvoices = getSQLQueryAsInteger(0, querySupplierInvoices)

                If conteoShipments + conteoOrders + conteoSupplierInvoices > 0 Then

                    Dim lugares As String = "("

                    If conteoShipments > 0 Then
                        lugares = lugares & "Envíos, "
                    End If

                    If conteoOrders > 0 Then
                        lugares = lugares & "Órdenes, "
                    End If

                    If conteoSupplierInvoices > 0 Then
                        lugares = lugares & "Facturas (de Proveedores), "
                    End If

                    lugares = lugares & ")"
                    lugares = lugares.Replace(", )", ")")

                    MsgBox("No puedo eliminar este Proveedor, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                    Exit Sub

                Else

                    Dim queriesDelete(3) As String

                    queriesDelete(0) = "DELETE FROM suppliers WHERE isupplierid = " & bed.isupplierid
                    queriesDelete(1) = "DELETE FROM supplierphonenumbers WHERE isupplierid = " & bed.isupplierid
                    queriesDelete(2) = "DELETE FROM suppliercontacts WHERE isupplierid = " & bed.isupplierid

                    If executeTransactedSQLCommand(0, queriesDelete) = True Then
                        MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                    Else
                        MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                    End If

                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    'Private Sub tsmiVerProveedoresDeudas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim bp As New BuscaProveedores

    '    bp.susername = susername
    '    bp.bactive = bactive
    '    bp.bonline = bonline
    '    bp.suserfullname = suserfullname
    '    bp.suseremail = suseremail
    '    bp.susersession = susersession
    '    bp.susermachinename = susermachinename
    '    bp.suserip = suserip

    '    bp.isEdit = True

    '    Me.Visible = False
    '    bp.ShowDialog(Me)
    '    Me.Visible = True

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    'Private Sub tsmiNuevoProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim ap As New AgregarProveedor
    '    ap.susername = susername
    '    ap.bactive = bactive
    '    ap.bonline = bonline
    '    ap.suserfullname = suserfullname
    '    ap.suseremail = suseremail
    '    ap.susersession = susersession
    '    ap.susermachinename = susermachinename
    '    ap.suserip = suserip

    '    ap.isEdit = False

    '    Me.Visible = False
    '    ap.ShowDialog(Me)
    '    Me.Visible = True

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    'Private Sub tsmiEliminarProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim bp As New BuscaProveedores

    '    bp.susername = susername
    '    bp.bactive = bactive
    '    bp.bonline = bonline
    '    bp.suserfullname = suserfullname
    '    bp.suseremail = suseremail
    '    bp.susersession = susersession
    '    bp.susermachinename = susermachinename
    '    bp.suserip = suserip

    '    bp.isEdit = False

    '    Me.Visible = False
    '    bp.ShowDialog(Me)
    '    Me.Visible = True

    '    If bp.DialogResult = Windows.Forms.DialogResult.OK Then

    '        Dim queryShipments As String
    '        Dim queryOrders As String
    '        Dim querySupplierInvoices As String

    '        Dim conteoShipments As Integer = 0
    '        Dim conteoOrders As Integer = 0
    '        Dim conteoSupplierInvoices As Integer = 0


    '        queryShipments = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM shipments " & _
    '        "WHERE isupplierid = " & bp.isupplierid


    '        queryOrders = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM orders " & _
    '        "WHERE isupplierid = " & bp.isupplierid


    '        querySupplierInvoices = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM supplierinvoices " & _
    '        "WHERE isupplierid = " & bp.isupplierid


    '        conteoShipments = getSQLQueryAsInteger(0, queryShipments)
    '        conteoOrders = getSQLQueryAsInteger(0, queryOrders)
    '        conteoSupplierInvoices = getSQLQueryAsInteger(0, querySupplierInvoices)

    '        If conteoShipments + conteoOrders + conteoSupplierInvoices > 0 Then

    '            Dim lugares As String = "("

    '            If conteoShipments > 0 Then
    '                lugares = lugares & "Envíos, "
    '            End If

    '            If conteoOrders > 0 Then
    '                lugares = lugares & "Órdenes, "
    '            End If

    '            If conteoSupplierInvoices > 0 Then
    '                lugares = lugares & "Facturas (de Proveedores), "
    '            End If

    '            lugares = lugares & ")"
    '            lugares = lugares.Replace(", )", ")")

    '            MsgBox("No puedo eliminar este Proveedor, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
    '            Exit Sub

    '        Else

    '            Dim queriesDelete(3) As String

    '            queriesDelete(0) = "DELETE FROM suppliers WHERE isupplierid = " & bp.isupplierid
    '            queriesDelete(1) = "DELETE FROM supplierphonenumbers WHERE isupplierid = " & bp.isupplierid
    '            queriesDelete(2) = "DELETE FROM suppliercontacts WHERE isupplierid = " & bp.isupplierid

    '            If executeTransactedSQLCommand(0, queriesDelete) = True Then
    '                MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
    '            Else
    '                MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
    '            End If

    '        End If

    '    End If

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    Private Sub tsmiVerFacturasDeProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerFacturasDeProveedores.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bfp As New BuscaFacturasProveedor

        bfp.susername = susername
        bfp.bactive = bactive
        bfp.bonline = bonline
        bfp.suserfullname = suserfullname
        bfp.suseremail = suseremail
        bfp.susersession = susersession
        bfp.susermachinename = susermachinename
        bfp.suserip = suserip

        bfp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bfp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bfp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaFacturaDeProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaFacturaDeProveedor.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim afp As New AgregarFacturaProveedor

        afp.susername = susername
        afp.bactive = bactive
        afp.bonline = bonline
        afp.suserfullname = suserfullname
        afp.suseremail = suseremail
        afp.susersession = susersession
        afp.susermachinename = susermachinename
        afp.suserip = suserip

        afp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            afp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        afp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarFacturaDeProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarFacturaDeProveedor.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bfp As New BuscaFacturasProveedor

        bfp.susername = susername
        bfp.bactive = bactive
        bfp.bonline = bonline
        bfp.suserfullname = suserfullname
        bfp.suseremail = suseremail
        bfp.susersession = susersession
        bfp.susermachinename = susermachinename
        bfp.suserip = suserip

        bfp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bfp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bfp.ShowDialog(Me)
        Me.Visible = True

        If bfp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim querySupplierInvoiceAssets As String
            Dim querySupplierInvoiceProjects As String
            Dim querySupplierInvoicePayments As String

            Dim conteoSupplierInvoiceAssets As Integer = 0
            Dim conteoSupplierInvoiceProjects As Integer = 0
            Dim conteoSupplierInvoicePayments As Integer = 0

            querySupplierInvoiceAssets = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & bfp.isupplierinvoiceid

            querySupplierInvoiceProjects = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & bfp.isupplierinvoiceid

            querySupplierInvoicePayments = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & bfp.isupplierinvoiceid

            conteoSupplierInvoiceAssets = getSQLQueryAsInteger(0, querySupplierInvoiceAssets)
            conteoSupplierInvoiceProjects = getSQLQueryAsInteger(0, querySupplierInvoiceProjects)
            conteoSupplierInvoicePayments = getSQLQueryAsInteger(0, querySupplierInvoicePayments)

            If conteoSupplierInvoiceAssets + conteoSupplierInvoiceProjects + conteoSupplierInvoicePayments > 0 Then

                Dim lugares As String = "("

                If conteoSupplierInvoiceAssets > 0 Then
                    lugares = lugares & "Activos, "
                End If

                If conteoSupplierInvoiceProjects > 0 Then
                    lugares = lugares & "Proyectos, "
                End If

                If conteoSupplierInvoicePayments > 0 Then
                    lugares = lugares & "Pagos, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar esta Factura de Proveedor, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM supplierinvoices WHERE isupplierinvoiceid = " & bfp.isupplierinvoiceid
                queriesDelete(1) = "DELETE FROM supplierinvoicediscounts WHERE isupplierinvoiceid = " & bfp.isupplierinvoiceid
                queriesDelete(2) = "DELETE FROM supplierinvoiceinputs WHERE isupplierinvoiceid = " & bfp.isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    'Private Sub tsmiVerPersonas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim bp As New BuscaPersonas

    '    bp.susername = susername
    '    bp.bactive = bactive
    '    bp.bonline = bonline
    '    bp.suserfullname = suserfullname
    '    bp.suseremail = suseremail
    '    bp.susersession = susersession
    '    bp.susermachinename = susermachinename
    '    bp.suserip = suserip

    '    bp.isEdit = True

    '    Me.Visible = False
    '    bp.ShowDialog(Me)
    '    Me.Visible = True

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    'Private Sub tsmiNuevaPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim ap As New AgregarPersona

    '    ap.susername = susername
    '    ap.bactive = bactive
    '    ap.bonline = bonline
    '    ap.suserfullname = suserfullname
    '    ap.suseremail = suseremail
    '    ap.susersession = susersession
    '    ap.susermachinename = susermachinename
    '    ap.suserip = suserip

    '    ap.isEdit = False

    '    Me.Visible = False
    '    ap.ShowDialog(Me)
    '    Me.Visible = True

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    'Private Sub tsmiEliminarPersona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '    Dim bp As New BuscaPersonas

    '    bp.susername = susername
    '    bp.bactive = bactive
    '    bp.bonline = bonline
    '    bp.suserfullname = suserfullname
    '    bp.suseremail = suseremail
    '    bp.susersession = susersession
    '    bp.susermachinename = susermachinename
    '    bp.suserip = suserip

    '    bp.isEdit = False

    '    Me.Visible = False
    '    bp.ShowDialog(Me)
    '    Me.Visible = True

    '    If bp.DialogResult = Windows.Forms.DialogResult.OK Then

    '        Dim queryShipments As String
    '        Dim queryOrders As String
    '        Dim queryGasVouchers As String
    '        Dim querySupplierInvoices As String
    '        Dim queryClientes As String
    '        Dim queryIncomes As String

    '        Dim conteoShipments As Integer = 0
    '        Dim conteoOrders As Integer = 0
    '        Dim conteoGasVouchers As Integer = 0
    '        Dim conteoSupplierInvoices As Integer = 0
    '        Dim conteoClientes As Integer = 0
    '        Dim conteoIncomes As Integer = 0

    '        queryShipments = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM shipments " & _
    '        "WHERE idriverid = " & bp.ipeopleid & " OR ireceiverid = " & bp.ipeopleid

    '        queryOrders = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM orders " & _
    '        "WHERE ipeopleid = " & bp.ipeopleid

    '        queryGasVouchers = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM gasvouchers " & _
    '        "WHERE ipeopleid = " & bp.ipeopleid

    '        querySupplierInvoices = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM supplierinvoices " & _
    '        "WHERE ipeopleid = " & bp.ipeopleid

    '        queryClientes = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM projects " & _
    '        "WHERE ipeopleid = " & bp.ipeopleid

    '        queryIncomes = "" & _
    '        "SELECT COUNT(*) " & _
    '        "FROM incomes " & _
    '        "WHERE ireceiverid = " & bp.ipeopleid

    '        conteoShipments = getSQLQueryAsInteger(0, queryShipments)
    '        conteoOrders = getSQLQueryAsInteger(0, queryOrders)
    '        conteoGasVouchers = getSQLQueryAsInteger(0, queryGasVouchers)
    '        conteoSupplierInvoices = getSQLQueryAsInteger(0, querySupplierInvoices)
    '        conteoClientes = getSQLQueryAsInteger(0, queryClientes)
    '        conteoIncomes = getSQLQueryAsInteger(0, queryIncomes)

    '        If conteoShipments + conteoOrders + conteoGasVouchers + conteoSupplierInvoices + conteoClientes + conteoIncomes > 0 Then

    '            Dim lugares As String = "("

    '            If conteoShipments > 0 Then
    '                lugares = lugares & "Envíos, "
    '            End If

    '            If conteoOrders > 0 Then
    '                lugares = lugares & "Órdenes, "
    '            End If

    '            If conteoGasVouchers > 0 Then
    '                lugares = lugares & "Vales de Gasolina, "
    '            End If

    '            If conteoSupplierInvoices > 0 Then
    '                lugares = lugares & "Facturas (de Proveedores), "
    '            End If

    '            If conteoClientes > 0 Then
    '                lugares = lugares & "Proyectos, "
    '            End If

    '            If conteoIncomes > 0 Then
    '                lugares = lugares & "Pagos (Ingresos), "
    '            End If

    '            lugares = lugares & ")"
    '            lugares = lugares.Replace(", )", ")")

    '            MsgBox("No puedo eliminar esta Persona, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
    '            Exit Sub

    '        Else

    '            Dim queriesDelete(3) As String

    '            queriesDelete(0) = "DELETE FROM people WHERE ipeopleid = " & bp.ipeopleid
    '            queriesDelete(1) = "DELETE FROM peoplephonenumbers WHERE ipeopleid = " & bp.ipeopleid
    '            queriesDelete(2) = "DELETE FROM suppliercontacts WHERE ipeopleid = " & bp.ipeopleid

    '            If executeTransactedSQLCommand(0, queriesDelete) = True Then
    '                MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
    '            Else
    '                MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
    '            End If


    '        End If

    '    End If

    '    Cursor.Current = System.Windows.Forms.Cursors.Default

    'End Sub


    Private Sub tsmiVerFacturasEmitidas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerFacturasEmitidas.Click

    End Sub

    Private Sub tsmiNuevaFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaFactura.Click

    End Sub

    Private Sub tsmiEliminarFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarFactura.Click

    End Sub


    Private Sub tsmiVerUnidades_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerUnidades.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bu As New BuscaUnidades

        bu.susername = susername
        bu.bactive = bactive
        bu.bonline = bonline
        bu.suserfullname = suserfullname
        bu.suseremail = suseremail
        bu.susersession = susersession
        bu.susermachinename = susermachinename
        bu.suserip = suserip

        bu.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bu.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bu.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaUnidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaUnidad.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim au As New AgregarUnidad

        au.susername = susername
        au.bactive = bactive
        au.bonline = bonline
        au.suserfullname = suserfullname
        au.suseremail = suseremail
        au.susersession = susersession
        au.susermachinename = susermachinename
        au.suserip = suserip

        au.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            au.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        au.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarUnidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarUnidad.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bu As New BuscaUnidades

        bu.susername = susername
        bu.bactive = bactive
        bu.bonline = bonline
        bu.suserfullname = suserfullname
        bu.suseremail = suseremail
        bu.susersession = susersession
        bu.susermachinename = susermachinename
        bu.suserip = suserip

        bu.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bu.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bu.ShowDialog(Me)
        Me.Visible = True

        If bu.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queriesDelete(3) As String

            queriesDelete(0) = "DELETE FROM transformationunits WHERE soriginunit = '" & bu.sunit1 & "' AND sdestinationunit = '" & bu.sunit2 & "'"

            If executeTransactedSQLCommand(0, queriesDelete) = True Then
                MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
            Else
                MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    'Private Sub tsmiVerEquivalencias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub

    'Private Sub tsmiNuevaEquivalencia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub

    'Private Sub tsmiEliminarEquivalencia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub


    Private Sub tsmiVerUsuarios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerUsuarios.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bu As New BuscaUsuarios

        bu.susername = susername
        bu.bactive = bactive
        bu.bonline = bonline
        bu.suserfullname = suserfullname
        bu.suseremail = suseremail
        bu.susersession = susersession
        bu.susermachinename = susermachinename
        bu.suserip = suserip

        bu.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bu.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bu.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoUsuario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoUsuario.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim au As New AgregarUsuario

        au.susername = susername
        au.bactive = bactive
        au.bonline = bonline
        au.suserfullname = suserfullname
        au.suseremail = suseremail
        au.susersession = susersession
        au.susermachinename = susermachinename
        au.suserip = suserip

        au.IsEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            au.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        au.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarUsuario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarUsuario.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bu As New BuscaUsuarios

        bu.susername = susername
        bu.bactive = bactive
        bu.bonline = bonline
        bu.suserfullname = suserfullname
        bu.suseremail = suseremail
        bu.susersession = susersession
        bu.susermachinename = susermachinename
        bu.suserip = suserip

        bu.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bu.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bu.ShowDialog(Me)
        Me.Visible = True

        If bu.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim conteoUsuario As Integer = 0
            Dim dsTablas As DataSet
            dsTablas = getSQLQueryAsDataset(0, "SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = 'oversight'")

            For i = 0 To dsTablas.Tables(0).Rows.Count - 1

                conteoUsuario = conteoUsuario + getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM " & dsTablas.Tables(0).Rows(i).Item(0) & " WHERE supdateusername = '" & bu.sselectedusername & "'")
                conteoUsuario = conteoUsuario + getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM " & dsTablas.Tables(0).Rows(i).Item(0) & " WHERE susername = '" & bu.sselectedusername & "'")

            Next i

            If conteoUsuario > 0 Then

                MsgBox("No puedo eliminar este Usuario, porque está asignado a una operación histórica de la compañía. Porque no intentas mejor deactivarlo?", MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM users WHERE susername = '" & bu.sselectedusername & "'"
                queriesDelete(1) = "DELETE FROM userpermissions WHERE susername = '" & bu.sselectedusername & "'"

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If


            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerCotizacionesDeMateriales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerCotizacionesDeMateriales.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaCotizacionesInsumos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaCotizacionDeMateriales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaCotizacionDeMateriales.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaProyectos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim bi As New BuscaInsumos

            bi.susername = susername
            bi.bactive = bactive
            bi.bonline = bonline
            bi.suserfullname = suserfullname
            bi.suseremail = suseremail
            bi.susersession = susersession
            bi.susermachinename = susermachinename
            bi.suserip = suserip

            bi.iprojectid = bp.iprojectid

            bi.IsEdit = False

            bi.IsBase = False
            bi.IsModel = False

            bi.IsHistoric = False

            Me.Visible = False
            bi.ShowDialog(Me)
            Me.Visible = True

            If bi.DialogResult = Windows.Forms.DialogResult.OK Then

                Dim apre As New AgregarCotizacionInsumo

                apre.susername = susername
                apre.bactive = bactive
                apre.bonline = bonline
                apre.suserfullname = suserfullname
                apre.suseremail = suseremail
                apre.susersession = susersession
                apre.susermachinename = susermachinename
                apre.suserip = suserip

                apre.iprojectid = bp.iprojectid
                apre.iinputid = bi.iinputid

                apre.isEdit = False

                If Me.WindowState = FormWindowState.Maximized Then
                    apre.WindowState = FormWindowState.Maximized
                End If

                Me.Visible = False
                apre.ShowDialog(Me)
                Me.Visible = True

            End If

        End If


    End Sub


    Private Sub tsmiVerInventarioDeActivos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerInventarioDeActivos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim inv As New InventarioDeActivos
        inv.susername = susername
        inv.bactive = bactive
        inv.bonline = bonline
        inv.suserfullname = suserfullname
        inv.suseremail = suseremail
        inv.susersession = susersession
        inv.susermachinename = susermachinename
        inv.suserip = suserip

        inv.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            inv.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        inv.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiRevisarFacturasDeProveedores_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRevisarFacturasDeProveedores.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bfpsr As New BuscaRevisionesFacturasProveedor

        bfpsr.susername = susername
        bfpsr.bactive = bactive
        bfpsr.bonline = bonline
        bfpsr.suserfullname = suserfullname
        bfpsr.suseremail = suseremail
        bfpsr.susersession = susersession
        bfpsr.susermachinename = susermachinename
        bfpsr.suserip = suserip

        bfpsr.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bfpsr.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bfpsr.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerNominas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerNominas.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bn As New BuscaNominas

        bn.susername = susername
        bn.bactive = bactive
        bn.bonline = bonline
        bn.suserfullname = suserfullname
        bn.suseremail = suseremail
        bn.susersession = susersession
        bn.susermachinename = susermachinename
        bn.suserip = suserip

        bn.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bn.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bn.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaNomina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaNomina.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim an As New AgregarNomina

        an.susername = susername
        an.bactive = bactive
        an.bonline = bonline
        an.suserfullname = suserfullname
        an.suseremail = suseremail
        an.susersession = susersession
        an.susermachinename = susermachinename
        an.suserip = suserip

        an.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            an.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        an.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarNomina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarNomina.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bn As New BuscaNominas

        bn.susername = susername
        bn.bactive = bactive
        bn.bonline = bonline
        bn.suserfullname = suserfullname
        bn.suseremail = suseremail
        bn.susersession = susersession
        bn.susermachinename = susermachinename
        bn.suserip = suserip

        bn.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bn.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bn.ShowDialog(Me)
        Me.Visible = True

        If bn.DialogResult = Windows.Forms.DialogResult.OK Then

            If MsgBox("¿Está seguro de que desea eliminar esta Nómina?", MsgBoxStyle.YesNo, "Confirmación Eliminación") = MsgBoxResult.Yes Then

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM payrollpayments WHERE ipayrollid = " & bn.ipayrollid) > 0 Then
                    MsgBox("No puedo eliminar esta Nómina, porque está asignado a una operación histórica de la compañía (Pagos)", MsgBoxStyle.OkOnly, "Error de Eliminación")
                    Exit Sub
                End If

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM payrolls WHERE ipayrollid = " & bn.ipayrollid
                queriesDelete(1) = "DELETE FROM payrollpeople WHERE ipayrollid = " & bn.ipayrollid
                queriesDelete(2) = "DELETE FROM payrollpayments WHERE ipayrollid = " & bn.ipayrollid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerFacturaCombustibleVales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerFacturaCombustibleVales.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bfc As New BuscaFacturasCombustible

        bfc.susername = susername
        bfc.bactive = bactive
        bfc.bonline = bonline
        bfc.suserfullname = suserfullname
        bfc.suseremail = suseremail
        bfc.susersession = susersession
        bfc.susermachinename = susermachinename
        bfc.suserip = suserip

        bfc.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bfc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bfc.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaFacturaCombustibleVales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaFacturaCombustibleVales.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim afc As New AgregarFacturaCombustible

        afc.susername = susername
        afc.bactive = bactive
        afc.bonline = bonline
        afc.suserfullname = suserfullname
        afc.suseremail = suseremail
        afc.susersession = susersession
        afc.susermachinename = susermachinename
        afc.suserip = suserip

        afc.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            afc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        afc.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarFacturaCombustibleVales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarFacturaCombustibleVales.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bfc As New BuscaFacturasCombustible

        bfc.susername = susername
        bfc.bactive = bactive
        bfc.bonline = bonline
        bfc.suserfullname = suserfullname
        bfc.suseremail = suseremail
        bfc.susersession = susersession
        bfc.susermachinename = susermachinename
        bfc.suserip = suserip

        bfc.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bfc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bfc.ShowDialog(Me)
        Me.Visible = True

        If bfc.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim querySupplierInvoiceAssets As String
            Dim querySupplierInvoiceProjects As String
            Dim querySupplierInvoicePayments As String

            Dim conteoSupplierInvoiceAssets As Integer = 0
            Dim conteoSupplierInvoiceProjects As Integer = 0
            Dim conteoSupplierInvoicePayments As Integer = 0

            querySupplierInvoiceAssets = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoiceassets " & _
            "WHERE isupplierinvoiceid = " & bfc.isupplierinvoiceid

            querySupplierInvoiceProjects = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoiceprojects " & _
            "WHERE isupplierinvoiceid = " & bfc.isupplierinvoiceid

            querySupplierInvoicePayments = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoicepayments " & _
            "WHERE isupplierinvoiceid = " & bfc.isupplierinvoiceid

            conteoSupplierInvoiceAssets = getSQLQueryAsInteger(0, querySupplierInvoiceAssets)
            conteoSupplierInvoiceProjects = getSQLQueryAsInteger(0, querySupplierInvoiceProjects)
            conteoSupplierInvoicePayments = getSQLQueryAsInteger(0, querySupplierInvoicePayments)

            If conteoSupplierInvoiceAssets + conteoSupplierInvoiceProjects + conteoSupplierInvoicePayments > 0 Then

                Dim lugares As String = "("

                If conteoSupplierInvoiceAssets > 0 Then
                    lugares = lugares & "Activos, "
                End If

                If conteoSupplierInvoiceProjects > 0 Then
                    lugares = lugares & "Proyectos, "
                End If

                If conteoSupplierInvoicePayments > 0 Then
                    lugares = lugares & "Pagos, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar esta Factura de Combustible, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM supplierinvoices WHERE isupplierinvoiceid = " & bfc.isupplierinvoiceid
                queriesDelete(1) = "DELETE FROM supplierinvoicediscounts WHERE isupplierinvoiceid = " & bfc.isupplierinvoiceid
                queriesDelete(2) = "DELETE FROM supplierinvoicegasvouchers WHERE isupplierinvoiceid = " & bfc.isupplierinvoiceid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerPagos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerPagos.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPagos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoPago.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ap As New AgregarPago

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarPago.Click

        If MsgBox("Si deseas reasignar un Pago a otra Factura, intenta editandola. Eliminar un Pago es permanentemente eliminarlo de la base de datos. ¿Deseas continuar eliminando un Pago?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPagos

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim querySupplierInvoices As String
            Dim queryPayrolls As String

            Dim conteoSupplierInvoices As Integer = 0
            Dim conteoPayrolls As Integer = 0

            querySupplierInvoices = "" & _
            "SELECT COUNT(*) " & _
            "FROM supplierinvoicepayments " & _
            "WHERE iincomeid = " & bp.ipaymentid

            queryPayrolls = "" & _
            "SELECT COUNT(*) " & _
            "FROM payrollpayments " & _
            "WHERE iincomeid = " & bp.ipaymentid

            conteoSupplierInvoices = getSQLQueryAsInteger(0, querySupplierInvoices)
            conteoPayrolls = getSQLQueryAsInteger(0, queryPayrolls)

            If conteoSupplierInvoices + conteoPayrolls > 0 Then

                Dim lugares As String = "("

                If conteoSupplierInvoices > 0 Then
                    lugares = lugares & "Facturas, "
                End If

                If conteoPayrolls > 0 Then
                    lugares = lugares & "Nominas, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar este Pago, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM payments WHERE ipaymentid = " & bp.ipaymentid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiRecentlyOpenedFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRecentlyOpenedFiles.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bfc As New BuscaArchivosAbiertosRecientemente

        bfc.susername = susername
        bfc.bactive = bactive
        bfc.bonline = bonline
        bfc.suserfullname = suserfullname
        bfc.suseremail = suseremail
        bfc.susersession = susersession
        bfc.susermachinename = susermachinename
        bfc.suserip = suserip

        If Me.WindowState = FormWindowState.Maximized Then
            bfc.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bfc.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerPolizas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerPolizas.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPolizas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevaPoliza_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevaPoliza.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ap As New AgregarPoliza

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarPoliza_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarPoliza.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaPolizas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            If MsgBox("¿Estás seguro de que deseas eliminar esta Poliza?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                Dim queries(2) As String

                queries(0) = "DELETE FROM policies WHERE ipolicyid = " & bp.ipolicyid
                queries(1) = "DELETE FROM policymovements WHERE ipolicyid = " & bp.ipolicyid

                If executeTransactedSQLCommand(0, queries) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerMensajes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerMensajes.Click

        'If messagesWindowIsAlreadyOpen = False Then

        Dim msg As New Mensajes
        Dim pt As Point

        msg.susername = susername
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

            pt = New Point(Me.Location.X, Me.Location.Y)

        End If

        msg.Location = pt
        msg.bAlreadyOpen = True
        msg.bShowAllMessages = True

        messagesWindowIsAlreadyOpen = True

        msg.Show()

        'End If

    End Sub


    Private Sub tsmiNuevoMensaje_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoMensaje.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim am As New AgregarMensaje
        am.susername = susername
        am.bactive = bactive
        am.bonline = bonline
        am.suserfullname = suserfullname
        am.suseremail = suseremail
        am.susersession = susersession
        am.susermachinename = susermachinename
        am.suserip = suserip

        am.isEdit = False

        Me.Visible = False
        am.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerReportes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerReportes.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ap As New Reportes

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiVerGastosPorCasetas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiVerGastosPorCasetas.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaGastosPorCasetas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiNuevoGastoPorCaseta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiNuevoGastoPorCaseta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim ap As New AgregarGastoPorCaseta

        ap.susername = susername
        ap.bactive = bactive
        ap.bonline = bonline
        ap.suserfullname = suserfullname
        ap.suseremail = suseremail
        ap.susersession = susersession
        ap.susermachinename = susermachinename
        ap.suserip = suserip

        ap.isEdit = False

        If Me.WindowState = FormWindowState.Maximized Then
            ap.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ap.ShowDialog(Me)
        Me.Visible = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tsmiEliminarGastoPorCaseta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiEliminarGastoPorCaseta.Click

        If MsgBox("Si deseas reasignar un Gasto por Caseta a otro Proyecto o Activo, intenta editandolo. Eliminar un Gasto por Caseta es permanentemente eliminarlo de la base de datos. ¿Deseas continuar eliminando un Gasto por Caseta?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim bp As New BuscaGastosPorCasetas

        bp.susername = susername
        bp.bactive = bactive
        bp.bonline = bonline
        bp.suserfullname = suserfullname
        bp.suseremail = suseremail
        bp.susersession = susersession
        bp.susermachinename = susermachinename
        bp.suserip = suserip

        bp.isEdit = True

        If Me.WindowState = FormWindowState.Maximized Then
            bp.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bp.ShowDialog(Me)
        Me.Visible = True

        If bp.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim queryPaytollProjects As String
            Dim queryPaytollAssets As String

            Dim conteoPaytollProjects As Integer = 0
            Dim conteoPaytollAssets As Integer = 0

            queryPaytollProjects = "" & _
            "SELECT COUNT(*) " & _
            "FROM paytollprojects " & _
            "WHERE iincomeid = " & bp.ipaytollid

            queryPaytollAssets = "" & _
            "SELECT COUNT(*) " & _
            "FROM paytollassets " & _
            "WHERE iincomeid = " & bp.ipaytollid

            conteoPaytollProjects = getSQLQueryAsInteger(0, queryPaytollProjects)
            conteoPaytollAssets = getSQLQueryAsInteger(0, queryPaytollAssets)

            If conteoPaytollProjects + conteoPaytollAssets > 0 Then

                Dim lugares As String = "("

                If conteoPaytollProjects > 0 Then
                    lugares = lugares & "Proyectos, "
                End If

                If conteoPaytollAssets > 0 Then
                    lugares = lugares & "Activos, "
                End If

                lugares = lugares & ")"
                lugares = lugares.Replace(", )", ")")

                MsgBox("No puedo eliminar este Gasto por Caseta, porque está asignado a una operación histórica de la compañía " & lugares, MsgBoxStyle.OkOnly, "Error de Eliminación")
                Exit Sub

            Else

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM paytolls WHERE ipaytollid = " & bp.ipaytollid

                If executeTransactedSQLCommand(0, queriesDelete) = True Then
                    MsgBox("Eliminación Exitosa!", MsgBoxStyle.OkOnly, "Status")
                Else
                    MsgBox("No se pudo llevar a cabo la eliminación!", MsgBoxStyle.OkOnly, "Error Eliminacion")
                End If

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


End Class
