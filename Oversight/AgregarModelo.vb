Public Class AgregarModelo

    Private fDone As Boolean = False

    Public susername As String = ""
    Public bactive As Boolean = False
    Public bonline As Boolean = False
    Public suserfullname As String = ""
    Public suseremail As String = ""
    Public susersession As Integer = 0
    Public susermachinename As String = ""
    Public suserip As String = "0.0.0.0"

    Public imodelid As Integer = 0

    Public isEdit As Boolean = False
    Public isHistoric As Boolean = False
    Public isRecover As Boolean = False

    Public wasCreated As Boolean = False

    Private imodelmodifieddate As Integer = 0
    Private smodelmodifiedtime As String = "00:00:00"

    Private iselectedcostid As Integer = 0
    Private sselectedcostdescription As String = ""
    Private dselectedcostamount As Double = 0.0

    Private iselectedcardid As Integer = 0
    Private sselectedcardlegacyid As String = ""
    Private dselectedcardqty As Double = 1.0
    Private sselectedcarddescription As String = ""

    Private WithEvents txtCantidadDgvCostosIndirectos As TextBox
    Private WithEvents txtNombreDgvCostosIndirectos As TextBox

    Private WithEvents txtCantidadDgvResumenDeTarjetas As TextBox
    Private WithEvents txtNombreDgvResumenDeTarjetas As TextBox
    Private WithEvents txtLegacyIdDgvResumenDeTarjetas As TextBox

    Private txtCantidadDgvCostosIndirectos_OldText As String = ""
    Private txtNombreDgvCostosIndirectos_OldText As String = ""

    Private txtCantidadDgvResumenDeTarjetas_OldText As String = ""
    Private txtNombreDgvResumenDeTarjetas_OldText As String = ""
    Private txtLegacyIdDgvResumenDeTarjetas_OldText As String = ""

    Private paso2 As Boolean = False
    Private paso3 As Boolean = False
    Private paso4 As Boolean = False

    Public alertaMostrada As Boolean = False

    Private isFormReadyForAction As Boolean = False
    Private isResumenDGVReady As Boolean = False

    Private viewPermission As Boolean = False

    Private addIndirectCosts As Boolean = False
    Private modifyIndirectCosts As Boolean = False
    Private deleteIndirectCosts As Boolean = False

    Private addCards As Boolean = False
    Private openCards As Boolean = False
    Private deleteCards As Boolean = False

    Private modifyCardQty As Boolean = False
    Private viewDgvIndirectCosts As Boolean = False
    Private viewDgvProfits As Boolean = False
    Private viewDgvUnitPrices As Boolean = False
    Private viewDgvAmount As Boolean = False

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


    Private Sub setControlsByPermissions(ByVal windowname As String, ByVal username As String)

        'Check for specific permissions on every window, but only for that unique window permissions, not the entire list!!

        Dim dsPermissions As DataSet

        Dim permission As String

        dsPermissions = getSQLQueryAsDataset(0, "SELECT * FROM userpermissions WHERE susername = '" & username & "' AND swindowname = '" & windowname & "'")

        For j = 0 To dsPermissions.Tables(0).Rows.Count - 1

            Try

                permission = dsPermissions.Tables(0).Rows(j).Item("spermission")

                If permission = "Ver" Then
                    viewPermission = True
                End If

                If permission = "Modificar" Then
                    btnPaso2.Enabled = True
                    btnGuardar.Enabled = True
                    btnGuardarYCerrar.Enabled = True
                End If

                If permission = "Editar Datos Iniciales" Then
                    dtFechaModificacion.Enabled = True
                    txtNombreModelo.Enabled = True
                    rbCasa.Enabled = True
                    rbOficina.Enabled = True
                    rbOtro.Enabled = True
                    txtAnchoVivienda.Enabled = True
                    txtLongitudVivienda.Enabled = True
                    btnAbrirCarpeta.Enabled = True
                End If

                If permission = "Ver Costos Indirectos" Then
                    dgvCostosIndirectos.Visible = True
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Agregar Costo Indirecto" Then
                    addIndirectCosts = True
                    btnNuevoCostoIndirecto.Enabled = True
                    dgvCostosIndirectos.Visible = True
                    dgvCostosIndirectos.ReadOnly = False
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Modificar Costos Indirectos" Then
                    modifyIndirectCosts = True
                    dgvCostosIndirectos.Visible = True
                    dgvCostosIndirectos.ReadOnly = False
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Eliminar Costos Indirectos" Then
                    deleteIndirectCosts = True
                    btnEliminarCostoIndirecto.Enabled = True
                    dgvCostosIndirectos.Visible = True
                    lblTotalIndirectosLbl.Visible = True
                    lblTotalIndirectos.Visible = True
                    lblIngresosIndirectos.Visible = True
                    txtIngresosIndirectos.Visible = True
                    lblPorcentajeIndirectosLbl.Visible = True
                    lblPorcentajeIndirectos.Visible = True
                End If

                If permission = "Ver Resumen de Tarjetas" Then
                    dgvResumenDeTarjetas.Visible = True
                End If

                If permission = "Agregar Tarjeta" Then
                    addCards = True
                    dgvResumenDeTarjetas.Visible = True
                    btnNuevaTarjeta.Enabled = True
                End If

                If permission = "Modificar Resumen de Tarjetas" Then
                    openCards = True
                    modifyCardQty = True
                    dgvResumenDeTarjetas.Visible = True
                    btnInsertarTarjeta.Enabled = True
                End If

                If permission = "Eliminar Tarjeta" Then
                    deleteCards = True
                    dgvResumenDeTarjetas.Visible = True
                    btnEliminarTarjeta.Enabled = True
                End If

                If permission = "Cambiar Defaults e IVA" Then
                    txtPorcentajeIVA.Enabled = True
                    txtPorcentajeUtilidadDefault.Enabled = True
                    txtPorcentajeIndirectosDefault.Enabled = True
                End If

                If permission = "Actualizar Precios" Then
                    btnActualizarPrecios.Enabled = True
                End If

                If permission = "Cambiar Utilidades e Ind" Then
                    btnActualizarUtilidad.Enabled = True
                End If

                If permission = "Generar Archivo Excel" Then
                    btnGenerarArchivoExcel.Enabled = True
                End If

                If permission = "Si se hiciera hoy" Then
                    btnCostoHoy.Enabled = True
                End If

                If permission = "Ver Indirectos" Then
                    viewDgvIndirectCosts = True
                End If

                If permission = "Ver Utilidades" Then
                    viewDgvProfits = True
                End If

                If permission = "Ver Precios Unitarios" Then
                    viewDgvUnitPrices = True
                End If

                If permission = "Ver Importes" Then
                    viewDgvAmount = True
                End If

                If permission = "Ver Explosion de Insumos" Then
                    dgvExplosionDeInsumos.Visible = True
                End If

                If permission = "Exportar Explosion de Insumos" Then
                    btnGenerarExplosion.Enabled = True
                End If

                If permission = "Ver Revisiones" Then
                    btnRevisiones.Enabled = True
                End If

            Catch ex As Exception

            End Try

            permission = ""

        Next j


        If viewPermission = False Then

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Acceso denegado a la ventana de Agregar Modelo', 'OK')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedatetime, smessagecreatorusername, iupdatedatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Un usuario ha intentado propasar sus permisos. ¿Podrías revisar?', 0, '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM', '" & convertYYYYMMDDtoYYYYhyphenMMhyphenDD(fecha) & " " & hora & "', 'SYSTEM')")
                Next i

            End If

            MsgBox("No tienes los permisos necesarios para abrir esta Ventana. Este intento ha sido notificado al administrador.", MsgBoxStyle.Exclamation, "Access Denied")

            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()

        End If

    End Sub


    Private Sub AgregarModelo_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim conteo1 As Integer = 0
        Dim conteo2 As Integer = 0
        Dim conteo3 As Integer = 0
        Dim conteo4 As Integer = 0
        Dim conteo5 As Integer = 0
        Dim conteo6 As Integer = 0
        Dim conteo7 As Integer = 0
        Dim conteo8 As Integer = 0
        Dim conteo9 As Integer = 0
        Dim conteo10 As Integer = 0
        Dim conteo11 As Integer = 0
        Dim conteo12 As Integer = 0
        Dim conteo13 As Integer = 0
        Dim conteo14 As Integer = 0
        Dim conteo15 As Integer = 0
        Dim conteo16 As Integer = 0
        Dim conteo17 As Integer = 0
        Dim conteo18 As Integer = 0
        Dim conteo19 As Integer = 0
        Dim conteo20 As Integer = 0
        Dim conteo21 As Integer = 0
        Dim conteo22 As Integer = 0
        Dim conteo23 As Integer = 0
        Dim conteo24 As Integer = 0
        Dim conteo25 As Integer = 0
        Dim conteo26 As Integer = 0
        Dim conteo27 As Integer = 0
        Dim conteo28 As Integer = 0
        Dim conteo29 As Integer = 0
        Dim conteo30 As Integer = 0
        Dim conteo31 As Integer = 0
        Dim conteo32 As Integer = 0
        Dim conteo33 As Integer = 0
        Dim conteo34 As Integer = 0
        Dim conteo35 As Integer = 0
        Dim conteo36 As Integer = 0
        Dim conteo37 As Integer = 0
        Dim conteo38 As Integer = 0
        Dim conteo39 As Integer = 0
        Dim conteo40 As Integer = 0
        Dim conteo41 As Integer = 0
        Dim conteo42 As Integer = 0
        Dim conteo43 As Integer = 0
        Dim conteo44 As Integer = 0
        Dim conteo45 As Integer = 0
        Dim conteo46 As Integer = 0
        Dim conteo47 As Integer = 0
        Dim conteo48 As Integer = 0

        Dim unsaved As Boolean = False

        Dim baseid As Integer = 0

        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If


        conteo1 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM models " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp WHERE models.imodelid = tp.imodelid) ")

        conteo2 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM modelindirectcosts " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic WHERE modelindirectcosts.imodelid = tpic.imodelid AND modelindirectcosts.icostid = tpic.icostid) ")

        conteo3 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM modelcards " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc WHERE modelcards.imodelid = tpc.imodelid AND modelcards.icardid = tpc.icardid) ")

        conteo4 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM modelcardinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci WHERE modelcardinputs.imodelid = tpci.imodelid AND modelcardinputs.icardid = tpci.icardid AND modelcardinputs.iinputid = tpci.iinputid) ")

        conteo5 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM modelcardcompoundinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci WHERE modelcardcompoundinputs.imodelid = tpcci.imodelid AND modelcardcompoundinputs.icardid = tpcci.icardid AND modelcardcompoundinputs.iinputid = tpcci.iinputid AND modelcardcompoundinputs.icompoundinputid = tpcci.icompoundinputid) ")

        conteo6 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM modelprices " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp WHERE modelprices.imodelid = tpp.imodelid AND modelprices.iinputid = tpp.iinputid AND modelprices.iupdatedate = tpp.iupdatedate AND modelprices.supdatetime = tpp.supdatetime) ")

        'conteo7 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM modelexplosion " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex WHERE modelexplosion.imodelid = tpex.imodelid AND modelexplosion.iinputid = tpex.iinputid) ")

        conteo40 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM modeltimber " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt WHERE modeltimber.imodelid = tpt.imodelid AND modeltimber.iinputid = tpt.iinputid) ")

        'conteo46 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM modeladmincosts " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac WHERE modeladmincosts.imodelid = tpac.imodelid AND modeladmincosts.iadmincostid = tpac.iadmincostid) ")

        conteo8 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp JOIN models p ON tp.imodelid = p.imodelid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo9 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpic.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic JOIN modelindirectcosts pic ON tpic.imodelid = pic.imodelid AND tpic.icostid = pic.icostid WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') ")

        conteo10 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc JOIN modelcards pc ON tpc.imodelid = pc.imodelid AND tpc.icardid = pc.icardid WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') ")

        conteo11 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci JOIN modelcardinputs pci ON tpci.imodelid = pci.imodelid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') ")

        conteo12 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpcci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci JOIN modelcardcompoundinputs pcci ON tpcci.imodelid = pcci.imodelid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') ")

        conteo13 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp JOIN modelprices pp ON tpp.imodelid = pp.imodelid AND tpp.iinputid = pp.iinputid WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') ")

        'conteo14 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(tpex.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex JOIN modelexplosion pex ON tpex.imodelid = pex.imodelid AND tpex.iinputid = pex.iinputid WHERE STR_TO_DATE(CONCAT(tpex.iupdatedate, ' ', tpex.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pex.iupdatedate, ' ', pex.supdatetime), '%Y%c%d %T') ")

        conteo41 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpt.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt JOIN modeltimber pt ON tpt.imodelid = pt.imodelid AND tpt.iinputid = pt.iinputid WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') ")

        'conteo47 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(tpac.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac JOIN modeladmincosts pac ON tpac.imodelid = pac.imodelid AND tpac.iadmincostid = pac.iadmincostid WHERE STR_TO_DATE(CONCAT(tpac.iupdatedate, ' ', tpac.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pac.iupdatedate, ' ', pac.supdatetime), '%Y%c%d %T') ")

        conteo15 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM models p WHERE p.imodelid = tp.imodelid AND p.imodelid = " & imodelid & ") ")

        conteo16 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic " & _
        "WHERE NOT EXISTS (SELECT * FROM modelindirectcosts pic WHERE pic.imodelid = tpic.imodelid AND pic.icostid = tpic.icostid AND pic.imodelid = " & imodelid & ") ")

        conteo17 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcards pc WHERE pc.imodelid = tpc.imodelid AND pc.icardid = tpc.icardid AND pc.imodelid = " & imodelid & ") ")

        conteo18 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardinputs pci WHERE pci.imodelid = tpci.imodelid AND pci.icardid = tpci.icardid AND pci.iinputid = tpci.iinputid AND pci.imodelid = " & imodelid & ") ")

        conteo19 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardcompoundinputs pcci WHERE pcci.imodelid = tpcci.imodelid AND pcci.icardid = tpcci.icardid AND pcci.iinputid = tpcci.iinputid AND pcci.icompoundinputid = tpcci.icompoundinputid AND pcci.imodelid = " & imodelid & ") ")

        conteo20 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tp " & _
        "WHERE NOT EXISTS (SELECT * FROM modelprices pp WHERE pp.imodelid = tpp.imodelid AND pp.iinputid = tpp.iinputid AND pp.iupdatedate = tpp.iupdatedate AND pp.supdatetime = tp.supdatetime) ")

        'conteo21 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex " & _
        '"WHERE NOT EXISTS (SELECT * FROM modelexplosion pex WHERE pex.imodelid = tpex.imodelid AND pex.iinputid = tpex.iinputid) ")

        conteo42 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt " & _
        "WHERE NOT EXISTS (SELECT * FROM modeltimber pt WHERE pt.imodelid = tpt.imodelid AND pt.iinputid = tpt.iinputid) ")

        'conteo48 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac " & _
        '"WHERE NOT EXISTS (SELECT * FROM modeladmincosts pac WHERE pac.imodelid = tpac.imodelid AND pac.iadmincostid = tpac.iadmincostid AND pac.imodelid = " & imodelid & ") ")

        'conteo22 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM base " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb WHERE base.ibaseid = tb.ibaseid) ")

        'conteo23 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM baseindirectcosts " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) ")

        'conteo24 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) ")

        'conteo25 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM basecardinputs " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) ")

        'conteo26 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM basecardcompoundinputs " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) ")

        'conteo27 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM baseprices " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) ")

        'conteo43 = getSQLQueryAsInteger(0, "" & _
        '"SELECT COUNT(*) " & _
        '"FROM basetimber " & _
        '"WHERE ibaseid = " & baseid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) ")

        conteo28 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tp JOIN base p ON tp.ibaseid = p.ibaseid WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') ")

        conteo29 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpic.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tpic JOIN baseindirectcosts pic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') ")

        conteo30 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpc.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tpc JOIN basecards pc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') ")

        conteo31 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tpci JOIN basecardinputs pci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') ")

        conteo32 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpcci.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tpcci JOIN basecardcompoundinputs pcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') ")

        conteo33 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpp.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tpp JOIN baseprices pp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') ")

        conteo44 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(tpt.*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tpt JOIN basetimber pt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') ")

        conteo34 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") ")

        conteo35 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") ")

        conteo36 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") ")

        conteo37 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") ")

        conteo38 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") ")

        conteo39 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") ")

        conteo45 = getSQLQueryAsInteger(0, "" & _
        "SELECT COUNT(*) " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") ")

        If conteo1 + conteo2 + conteo3 + conteo4 + conteo5 + conteo6 + conteo7 + conteo8 + conteo10 + conteo11 + conteo12 + conteo13 + conteo14 + conteo15 + conteo16 + conteo17 + conteo18 + conteo19 + conteo20 + conteo21 + conteo22 + conteo23 + conteo24 + conteo25 + conteo26 + conteo27 + conteo28 + conteo29 + conteo30 + conteo31 + conteo32 + conteo33 + conteo34 + conteo35 + conteo36 + conteo37 + conteo38 + conteo39 + conteo40 + conteo41 + conteo42 + conteo43 + conteo44 + conteo45 + conteo46 + conteo47 + conteo48 > 0 Then

            unsaved = True

        End If


        Dim incomplete As Boolean = False
        Dim msg As String = ""
        Dim result As Integer = 0

        If (validaDatosIniciales(True, True) = False Or validaCostosIndirectos(True) = False Or validaResumenDeTarjetas(True) = False) And Me.DialogResult <> Windows.Forms.DialogResult.OK And isHistoric = False Then
            incomplete = True
        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default

        If incomplete = True Then
            result = MsgBox("Este Modelo está incompleto. Si sales ahora, se perderán los cambios que hayas hecho." & Chr(13) & "¿Realmente deseas Salir de esta ventana ahora?", MsgBoxStyle.YesNo, "Confirmación Salida")
        ElseIf unsaved = True Then
            result = MsgBox("Tienes datos sin guardar! Tienes 3 opciones: " & Chr(13) & "Guardar los cambios (Sí), Regresar a revisar los cambios y guardarlos manualmente (Cancelar) o No guardarlos (No)", MsgBoxStyle.YesNoCancel, "Confirmación Salida")
        End If

        If result = MsgBoxResult.No And incomplete = True Then

            If viewPermission = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub

            End If

        ElseIf result = MsgBoxResult.Yes And incomplete = False And isHistoric = False Then


            Dim timesProjectIsOpen As Integer = 1

            timesProjectIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid & "'")

            If timesProjectIsOpen > 1 And isEdit = True Then

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If MsgBox("Otro usuario tiene abierto el mismo Modelo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                    e.Cancel = True
                    Exit Sub

                Else

                    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                End If

            ElseIf timesProjectIsOpen > 1 And isEdit = False Then

                Dim newIdAddition As Integer = 1

                Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid + newIdAddition & "'") > 1 And isEdit = False
                    newIdAddition = newIdAddition + 1
                Loop

                'I got the new id (previousId + newIdAddition)

                Dim queriesNewId(33) As String

                queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition
                queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts"
                queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards"
                queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs"
                queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs"
                queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices"
                'queriesNewId(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion"
                queriesNewId(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber"
                'queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts"
                queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition
                queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts"
                queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards"
                queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs"
                queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs"
                queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices"
                queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber"
                queriesNewId(16) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardsAux"
                queriesNewId(17) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & " SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(18) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(19) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(20) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(21) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(22) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                'queriesNewId(23) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                'queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
                queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & " SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
                queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
                queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
                queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
                queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
                queriesNewId(31) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
                queriesNewId(32) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid

                If executeTransactedSQLCommand(0, queriesNewId) = True Then
                    imodelid = imodelid + newIdAddition
                End If

            End If

            Dim queries(50) As String

            queries(0) = "" & _
            "DELETE " & _
            "FROM models " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp WHERE models.imodelid = tp.imodelid) "

            queries(1) = "" & _
            "DELETE " & _
            "FROM modelindirectcosts " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic WHERE modelindirectcosts.imodelid = tpic.imodelid AND modelindirectcosts.icostid = tpic.icostid) "

            queries(2) = "" & _
            "DELETE " & _
            "FROM modelcards " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc WHERE modelcards.imodelid = tpc.imodelid AND modelcards.icardid = tpc.icardid) "

            queries(3) = "" & _
            "DELETE " & _
            "FROM modelcardinputs " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci WHERE modelcardinputs.imodelid = tpci.imodelid AND modelcardinputs.icardid = tpci.icardid AND modelcardinputs.iinputid = tpci.iinputid) "

            queries(4) = "" & _
            "DELETE " & _
            "FROM modelcardcompoundinputs " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci WHERE modelcardcompoundinputs.imodelid = tpcci.imodelid AND modelcardcompoundinputs.icardid = tpcci.icardid AND modelcardcompoundinputs.iinputid = tpcci.iinputid AND modelcardcompoundinputs.icompoundinputid = tpcci.icompoundinputid) "

            queries(5) = "" & _
            "DELETE " & _
            "FROM modelprices " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp WHERE modelprices.imodelid = tpp.imodelid AND modelprices.iinputid = tpp.iinputid AND modelprices.iupdatedate = tpp.iupdatedate AND modelprices.supdatetime = tpp.supdatetime) "

            'queries(6) = "" & _
            '"DELETE " & _
            '"FROM modelexplosion " & _
            '"WHERE imodelid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex WHERE modelexplosion.imodelid = tpex.imodelid AND modelexplosion.iinputid = tpex.iinputid) "

            queries(7) = "" & _
            "DELETE " & _
            "FROM modeltimber " & _
            "WHERE imodelid = " & imodelid & " AND " & _
            "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt WHERE modeltimber.imodelid = tpt.imodelid AND modeltimber.iinputid = tpt.iinputid) "

            'queries(47) = "" & _
            '"DELETE " & _
            '"FROM modeladmincosts " & _
            '"WHERE imodelid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac WHERE modeladmincosts.imodelid = tpac.imodelid AND modeladmincosts.iadmincostid = tpac.iadmincostid) "

            queries(8) = "" & _
            "UPDATE models p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp ON tp.imodelid = p.imodelid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.smodelname = tp.smodelname, p.smodeltype = tp.smodeltype, p.dmodellength = tp.dmodellength, p.dmodelwidth = tp.dmodelwidth, p.smodelfileslocation = tp.smodelfileslocation, p.dmodelindirectpercentagedefault = tp.dmodelindirectpercentagedefault, p.dmodelgainpercentagedefault = tp.dmodelgainpercentagedefault, p.dmodelIVApercentage = tp.dmodelIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queries(9) = "" & _
            "UPDATE modelindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic ON tpic.imodelid = pic.imodelid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.smodelcostname = tpic.smodelcostname, pic.dmodelcost = tpic.dmodelcost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

            queries(10) = "" & _
            "UPDATE modelcards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc ON tpc.imodelid = pc.imodelid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

            queries(11) = "" & _
            "UPDATE modelcardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci ON tpci.imodelid = pci.imodelid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

            queries(12) = "" & _
            "UPDATE modelcardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci ON tpcci.imodelid = pcci.imodelid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.iinputid = tpcci.iinputid, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

            queries(13) = "" & _
            "UPDATE modelprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp ON tpp.imodelid = pp.imodelid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

            'queries(14) = "" & _
            '"UPDATE modelexplosion pex JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex ON tpex.imodelid = pex.imodelid AND tpex.iinputid = pex.iinputid SET pex.iupdatedate = tpex.iupdatedate, pex.supdatetime = tpex.supdatetime, pex.supdateusername = tpex.supdateusername, pex.dinputrealqty = tpex.dinputrealqty WHERE STR_TO_DATE(CONCAT(tpex.iupdatedate, ' ', tpex.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pex.iupdatedate, ' ', pex.supdatetime), '%Y%c%d %T') "

            queries(15) = "" & _
            "UPDATE modeltimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt ON tpt.imodelid = pt.imodelid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

            'queries(48) = "" & _
            '"UPDATE modeladmincosts pac JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac ON tpac.imodelid = pac.imodelid AND tpac.iadmincostid = pac.iadmincostid SET pac.iupdatedate = tpac.iupdatedate, pac.supdatetime = tpac.supdatetime, pac.supdateusername = tpac.supdateusername, pac.smodeladmincostname = tpac.smodeladmincostname, pac.dmodeladmincost = tpac.dmodeladmincost WHERE STR_TO_DATE(CONCAT(tpac.iupdatedate, ' ', tpac.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pac.iupdatedate, ' ', pac.supdatetime), '%Y%c%d %T') "

            queries(16) = "" & _
            "INSERT INTO models " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp " & _
            "WHERE NOT EXISTS (SELECT * FROM models p WHERE p.imodelid = tp.imodelid AND p.imodelid = " & imodelid & ") "

            queries(17) = "" & _
            "INSERT INTO modelindirectcosts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic " & _
            "WHERE NOT EXISTS (SELECT * FROM modelindirectcosts pic WHERE pic.imodelid = tpic.imodelid AND pic.icostid = tpic.icostid AND pic.imodelid = " & imodelid & ") "

            queries(18) = "" & _
            "INSERT INTO modelcards " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc " & _
            "WHERE NOT EXISTS (SELECT * FROM modelcards pc WHERE pc.imodelid = tpc.imodelid AND pc.icardid = tpc.icardid AND pc.imodelid = " & imodelid & ") "

            queries(19) = "" & _
            "INSERT INTO modelcardinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci " & _
            "WHERE NOT EXISTS (SELECT * FROM modelcardinputs pci WHERE pci.imodelid = tpci.imodelid AND pci.icardid = tpci.icardid AND pci.iinputid = tpci.iinputid AND pci.imodelid = " & imodelid & ") "

            queries(20) = "" & _
            "INSERT INTO modelcardcompoundinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci " & _
            "WHERE NOT EXISTS (SELECT * FROM modelcardcompoundinputs pcci WHERE pcci.imodelid = tpcci.imodelid AND pcci.icardid = tpcci.icardid AND pcci.iinputid = tpcci.iinputid AND pcci.icompoundinputid = tpcci.icompoundinputid AND pcci.imodelid = " & imodelid & ") "

            queries(21) = "" & _
            "INSERT INTO modelprices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp " & _
            "WHERE NOT EXISTS (SELECT * FROM modelprices pp WHERE pp.imodelid = tpp.imodelid AND pp.iinputid = tpp.iinputid AND pp.iupdatedate = tpp.iupdatedate AND pp.supdatetime = tpp.supdatetime) "

            'queries(22) = "" & _
            '"INSERT INTO modelexplosion " & _
            '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex " & _
            '"WHERE NOT EXISTS (SELECT * FROM modelexplosion pex WHERE pex.imodelid = tpex.imodelid AND pex.iinputid = tpex.iinputid) "

            queries(23) = "" & _
            "INSERT INTO modeltimber " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt " & _
            "WHERE NOT EXISTS (SELECT * FROM modeltimber pt WHERE pt.imodelid = tpt.imodelid AND pt.iinputid = tpt.iinputid) "

            'queries(49) = "" & _
            '"INSERT INTO modeladmincosts " & _
            '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac " & _
            '"WHERE NOT EXISTS (SELECT * FROM modeladmincosts pac WHERE pac.imodelid = tpac.imodelid AND pac.iadmincostid = tpac.iadmincostid AND pac.imodelid = " & imodelid & ") "

            'queries(24) = "" & _
            '"DELETE " & _
            '"FROM base " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb WHERE base.ibaseid = tb.ibaseid) "

            'queries(25) = "" & _
            '"DELETE " & _
            '"FROM baseindirectcosts " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

            'queries(26) = "" & _
            '"DELETE " & _
            '"FROM basecards " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

            'queries(27) = "" & _
            '"DELETE " & _
            '"FROM basecards " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

            'queries(28) = "" & _
            '"DELETE " & _
            '"FROM basecardinputs " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

            'queries(29) = "" & _
            '"DELETE " & _
            '"FROM basecardcompoundinputs " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

            'queries(30) = "" & _
            '"DELETE " & _
            '"FROM baseprices " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

            'queries(31) = "" & _
            '"DELETE " & _
            '"FROM basetimber " & _
            '"WHERE ibaseid = " & imodelid & " AND " & _
            '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

            queries(32) = "" & _
            "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

            queries(33) = "" & _
            "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

            queries(34) = "" & _
            "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

            queries(35) = "" & _
            "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

            queries(36) = "" & _
            "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

            queries(37) = "" & _
            "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

            queries(38) = "" & _
            "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

            queries(39) = "" & _
            "INSERT INTO base " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb " & _
            "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

            queries(40) = "" & _
            "INSERT INTO baseindirectcosts " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic " & _
            "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

            queries(41) = "" & _
            "INSERT INTO basecards " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc " & _
            "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

            queries(42) = "" & _
            "INSERT INTO basecardinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci " & _
            "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

            queries(43) = "" & _
            "INSERT INTO basecardcompoundinputs " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci " & _
            "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

            queries(44) = "" & _
            "INSERT INTO baseprices " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp " & _
            "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

            queries(45) = "" & _
            "INSERT INTO basetimber " & _
            "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt " & _
            "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") "

            queries(46) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Modelo " & imodelid & ": " & txtNombreModelo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

            If executeTransactedSQLCommand(0, queries) = True Then
                MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            Else
                MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
                Cursor.Current = System.Windows.Forms.Cursors.Default
                e.Cancel = True
                Exit Sub
            End If

            wasCreated = True

        ElseIf result = MsgBoxResult.Cancel Then

            Cursor.Current = System.Windows.Forms.Cursors.Default
            e.Cancel = True
            Exit Sub

        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim queriesDelete(20) As String

        queriesDelete(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid
        queriesDelete(1) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts"
        queriesDelete(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards"
        queriesDelete(3) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
        queriesDelete(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs"
        queriesDelete(5) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs"
        queriesDelete(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices"
        'queriesDelete(7) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion"
        queriesDelete(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber"
        'queriesDelete(9) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts"
        queriesDelete(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid
        queriesDelete(11) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts"
        queriesDelete(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards"
        queriesDelete(13) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardsAux"
        queriesDelete(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs"
        queriesDelete(15) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs"
        queriesDelete(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices"
        queriesDelete(17) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber"
        queriesDelete(18) = "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Cerró el Modelo " & imodelid & ": " & txtNombreModelo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"
        queriesDelete(19) = "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Model', 'Modelo', '" & imodelid & "', '" & txtNombreModelo.Text.Replace("'", "").Replace("--", "") & "', 0, " & fecha & ", '" & hora & "', '" & susername & "')"

        executeTransactedSQLCommand(0, queriesDelete)

        verifySuspiciousData()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub AgregarModelo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

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


    Private Sub AgregarModelo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Me.KeyPreview = True

        closeTimedOutConnections()
        checkForKickoutsAndTimedOuts()
        checkMessages(susername, Me.Location.X, Me.Location.Y)
        setControlsByPermissions(Me.Name, susername)

        loadProgramSettings()

        Dim timesProjectIsOpen As Integer = 0

        timesProjectIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid & "'")

        If timesProjectIsOpen > 0 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Modelo. Esto podría causar que alguno de ustedes perdiera los cambios que hiciera. ¿Deseas seguir abriendo el Modelo?", MsgBoxStyle.YesNo, "Confirmación Apertura") = MsgBoxResult.No Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        End If

        Dim baseid As Integer = 0

        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If isRecover = False Then

            Dim queriesCreation(32) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " ( `imodelid` int(11) NOT NULL AUTO_INCREMENT, `smodelname` varchar(200) CHARACTER SET latin1 NOT NULL, `smodeltype` varchar(200) CHARACTER SET latin1 NOT NULL, `dmodellength` varchar(100) CHARACTER SET latin1 NOT NULL, `dmodelwidth` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `smodelfileslocation` varchar(1000) CHARACTER SET latin1 NOT NULL, `dmodelindirectpercentagedefault` decimal(20,5) NOT NULL, `dmodelgainpercentagedefault` decimal(20,5) NOT NULL, `dmodelIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts"
            queriesCreation(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts ( `imodelid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `smodelcostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodelcost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icostid`), KEY `modelid` (`imodelid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards"
            queriesCreation(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `legacycategoryid` (`scardlegacycategoryid`), KEY `legacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs"
            queriesCreation(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs"
            queriesCreation(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices"
            queriesCreation(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `inputid` (`iinputid`), KEY `modelid` (`imodelid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesCreation(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion"
            'queriesCreation(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputrealqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber"
            queriesCreation(15) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesCreation(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts"
            'queriesCreation(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts ( `imodelid` int(11) NOT NULL, `iadmincostid` int(11) NOT NULL, `smodeladmincostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodeladmincost` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iadmincostid`), KEY `modelid` (`imodelid`), KEY `admincostid` (`iadmincostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs"
            queriesCreation(19) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(20) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs"
            queriesCreation(21) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(22) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards"
            queriesCreation(23) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(24) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts"
            queriesCreation(25) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(26) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices"
            queriesCreation(27) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(28) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid
            queriesCreation(29) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesCreation(30) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber"
            queriesCreation(31) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

        End If

        If isEdit = False Then

            'Modelo nuevo

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            tcModelo.SelectedTab = tpDatosIniciales

            imodelmodifieddate = getMySQLDate()
            smodelmodifiedtime = getAppTime()

            dtFechaModificacion.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & smodelmodifiedtime

            txtNombreModelo.Text = ""
            rbCasa.Checked = True
            rbOficina.Checked = False
            rbOtro.Checked = False

            txtRuta.Text = ""
            txtPrecioProyectadoSubTotal.Text = ""
            txtIVA.Text = ""
            txtPrecioProyectadoTotal.Text = ""

            btnRevisiones.Enabled = False

            Me.Text = "Modelo Nuevo"

            txtNombreModelo.Select()
            txtNombreModelo.Focus()

            Cursor.Current = System.Windows.Forms.Cursors.Default

        Else

            'Revisar/Modificar


            'Tab de Datos Iniciales


            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim alertaModeloIncompleto As Boolean = False
            Dim mensajeAlerta As String = ""

            If isRecover = False Then

                Dim queriesInsert(16) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SELECT * FROM models WHERE imodelid = " & imodelid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts SELECT * FROM modelindirectcosts WHERE imodelid = " & imodelid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT * FROM modelcards WHERE imodelid = " & imodelid
                queriesInsert(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT * FROM modelcardinputs WHERE imodelid = " & imodelid
                queriesInsert(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT * FROM modelcardcompoundinputs WHERE imodelid = " & imodelid
                queriesInsert(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices SELECT * FROM modelprices WHERE imodelid = " & imodelid
                'queriesInsert(6) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion SELECT * FROM modelexplosion WHERE imodelid = " & imodelid
                queriesInsert(7) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber SELECT * FROM modeltimber WHERE imodelid = " & imodelid
                'queriesInsert(8) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts SELECT * FROM modeladmincosts WHERE imodelid = " & imodelid
                queriesInsert(9) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs SELECT * FROM basecardcompoundinputs WHERE ibaseid = " & baseid
                queriesInsert(10) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs SELECT * FROM basecardinputs WHERE ibaseid = " & baseid
                queriesInsert(11) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards SELECT * FROM basecards WHERE ibaseid = " & baseid
                queriesInsert(12) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts SELECT * FROM baseindirectcosts WHERE ibaseid = " & baseid
                queriesInsert(13) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices SELECT * FROM baseprices WHERE ibaseid = " & baseid
                queriesInsert(14) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " SELECT * FROM base WHERE ibaseid = " & baseid
                queriesInsert(15) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber SELECT * FROM basetimber WHERE ibaseid = " & baseid

                executeTransactedSQLCommand(0, queriesInsert)

            End If


            Dim dsModelo As DataSet
            dsModelo = getSQLQueryAsDataset(0, "SELECT p.imodelid, p.iupdatedate, " & _
            "p.supdatetime, p.smodelname, p.smodeltype, p.dmodellength, p.dmodelwidth, " & _
            "p.smodelfileslocation, p.dmodelindirectpercentagedefault, p.dmodelgainpercentagedefault, " & _
            "p.dmodelIVApercentage " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p WHERE p.imodelid = " & imodelid)


            Try

                If dsModelo.Tables(0).Rows.Count > 0 Then

                    imodelmodifieddate = dsModelo.Tables(0).Rows(0).Item("iupdatedate")
                    smodelmodifiedtime = dsModelo.Tables(0).Rows(0).Item("supdatetime")

                    dtFechaModificacion.Value = convertYYYYMMDDtoDDhyphenMMhyphenYYYY(dsModelo.Tables(0).Rows(0).Item("iupdatedate")) & " " & dsModelo.Tables(0).Rows(0).Item("supdatetime")

                    txtNombreModelo.Text = dsModelo.Tables(0).Rows(0).Item("smodelname")

                    If txtNombreModelo.Text.Trim = "" And alertaModeloIncompleto = False Then
                        alertaModeloIncompleto = True
                        mensajeAlerta = "¿Podrías ponerle nombre al Modelo?"
                    Else
                        Me.Text = "Modelo " & txtNombreModelo.Text
                    End If

                    Dim tipoConstruccion As String = dsModelo.Tables(0).Rows(0).Item("smodeltype")

                    If tipoConstruccion = "Casa Habitación" Then
                        rbCasa.Checked = True
                    ElseIf tipoConstruccion = "Oficina" Then
                        rbOficina.Checked = True
                    ElseIf tipoConstruccion = "Otro" Then
                        rbOtro.Checked = True
                    End If

                    If rbCasa.Checked = False And rbOficina.Checked = False And rbOtro.Checked = False And alertaModeloIncompleto = False Then
                        alertaModeloIncompleto = True
                        mensajeAlerta = "¿Podrías escoger de qué tipo de Construcción se trata el Modelo?"
                    End If

                    txtLongitudVivienda.Text = dsModelo.Tables(0).Rows(0).Item("dmodellength")
                    txtAnchoVivienda.Text = dsModelo.Tables(0).Rows(0).Item("dmodelwidth")

                    If (txtLongitudVivienda.Text.Trim = "" Or txtAnchoVivienda.Text.Trim = "" Or txtLongitudVivienda.Text.Trim = "0" Or txtAnchoVivienda.Text.Trim = "0") And alertaModeloIncompleto = False Then
                        alertaModeloIncompleto = True
                        mensajeAlerta = "¿Qué medidas tiene la construcción del Modelo?"
                    End If

                    Try
                        txtRuta.Text = dsModelo.Tables(0).Rows(0).Item("smodelfileslocation")
                        txtRuta.Text = txtRuta.Text.Replace("/", "\")
                    Catch ex As Exception

                    End Try

                    Dim porcentajeIVA As Double
                    Try
                        porcentajeIVA = CDbl(dsModelo.Tables(0).Rows(0).Item("dmodelIVApercentage"))
                    Catch ex As Exception
                        porcentajeIVA = 0.0
                    End Try

                    txtPorcentajeIVA.Text = porcentajeIVA * 100




                    'Revisión de Completitud de Datos


                    If alertaModeloIncompleto = True Then

                        Select Case mensajeAlerta

                            Case "¿Podrías ponerle nombre al Modelo?"

                                tcModelo.SelectedTab = tpDatosIniciales
                                txtNombreModelo.Select()
                                txtNombreModelo.Focus()
                                Cursor.Current = System.Windows.Forms.Cursors.Default
                                MsgBox(mensajeAlerta, MsgBoxStyle.OkOnly, "Dato Faltante")

                            Case "¿Podrías escoger de qué tipo de Construcción se trata el Modelo?"

                                tcModelo.SelectedTab = tpDatosIniciales
                                rbCasa.Select()
                                rbCasa.Focus()
                                Cursor.Current = System.Windows.Forms.Cursors.Default
                                MsgBox(mensajeAlerta, MsgBoxStyle.OkOnly, "Dato Faltante")

                            Case "¿Qué medidas tiene la construcción del Modelo?"

                                tcModelo.SelectedTab = tpDatosIniciales
                                txtLongitudVivienda.Select()
                                txtLongitudVivienda.Focus()
                                Cursor.Current = System.Windows.Forms.Cursors.Default
                                MsgBox(mensajeAlerta, MsgBoxStyle.OkOnly, "Dato Faltante")

                            Case "¿Podrías hacer el cálculo de costos indirectos del Modelo?"

                                tcModelo.SelectedTab = tpCostosIndirectos
                                dgvCostosIndirectos.Select()
                                dgvCostosIndirectos.Focus()
                                Cursor.Current = System.Windows.Forms.Cursors.Default
                                MsgBox(mensajeAlerta, MsgBoxStyle.OkOnly, "Dato Faltante")

                            Case "¿Podrías poner el porcentaje de IVA aplicable a las Tarjetas del Modelo?"

                                tcModelo.SelectedTab = tpResumenTarjetas
                                txtPorcentajeIVA.Select()
                                txtPorcentajeIVA.Focus()
                                Cursor.Current = System.Windows.Forms.Cursors.Default
                                MsgBox(mensajeAlerta, MsgBoxStyle.OkOnly, "Dato Faltante")


                        End Select

                        Cursor.Current = System.Windows.Forms.Cursors.Default

                    Else

                        tcModelo.SelectedTab = tpDatosIniciales
                        txtNombreModelo.Select()
                        txtNombreModelo.Focus()
                        Cursor.Current = System.Windows.Forms.Cursors.Default

                    End If

                End If

            Catch ex As Exception

            End Try

        End If

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Abrió el Modelo " & imodelid & ": " & txtNombreModelo.Text.Replace("--", "").Replace("'", "") & "', 'OK')")
        executeSQLCommand(0, "INSERT INTO recentlyopenedfiles VALUES ('" & susername & "', '" & susersession & "', 'Model', 'Modelo', '" & imodelid & "', '" & txtNombreModelo.Text.Replace("'", "").Replace("--", "") & "', 1, " & fecha & ", '" & hora & "', '" & susername & "')")

        isFormReadyForAction = True
        isResumenDGVReady = True

        'If verifyLicense(False, True) = False And isEdit = False And contarModelos() >= 3 Then

        '    btnGuardar.Enabled = False
        '    btnGuardarYCerrar.Enabled = False
        '    btnPaso2.Enabled = False

        'End If

        txtNombreModelo.Select()
        txtNombreModelo.Focus()
        txtNombreModelo.SelectionStart() = txtNombreModelo.Text.Length

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


    Public Function contarModelos() As Integer

        Return getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "")

    End Function


    Private Sub txtNombreModelo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNombreModelo.KeyUp

        Dim strcaracteresprohibidos As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtNombreModelo.Text.Contains(arrayCaractProhib(carp)) Then
                txtNombreModelo.Text = txtNombreModelo.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If resultado = True Then
            txtNombreModelo.Select(txtNombreModelo.Text.Length, 0)
        End If

        txtNombreModelo.Text = txtNombreModelo.Text.Replace("--", "").Replace("'", "").Replace("@", "")

        If dataLocation.Replace("/", "\").EndsWith("\") = True Then
            txtRuta.Text = dataLocation.Replace("/", "\") & "Modelos\"
        Else
            txtRuta.Text = dataLocation.Replace("/", "\") & "\Modelos\"
        End If

        If txtNombreModelo.Text = "" Then
            If dataLocation.Replace("/", "\").EndsWith("\") = True Then
                txtRuta.Text = dataLocation.Replace("/", "\")
            Else
                txtRuta.Text = dataLocation.Replace("/", "\") & "\"
            End If
        ElseIf txtNombreModelo.Text <> "" Then
            If dataLocation.Replace("/", "\").EndsWith("\") = True Then
                txtRuta.Text = dataLocation.Replace("/", "\") & "Modelos\" & txtNombreModelo.Text & "\"
            Else
                txtRuta.Text = dataLocation.Replace("/", "\") & "\Modelos\" & txtNombreModelo.Text & "\"
            End If
        End If

    End Sub


    Private Sub txtLongitudVivienda_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLongitudVivienda.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtLongitudVivienda.Text.Contains(arrayCaractProhib(carp)) Then
                txtLongitudVivienda.Text = txtLongitudVivienda.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtLongitudVivienda.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtLongitudVivienda.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtLongitudVivienda.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtLongitudVivienda.Text.Length

                    If longitud > (lugar + 1) Then
                        txtLongitudVivienda.Text = txtLongitudVivienda.Text.Substring(0, lugar) & txtLongitudVivienda.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtLongitudVivienda.Text = txtLongitudVivienda.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtLongitudVivienda.Select(txtLongitudVivienda.Text.Length, 0)
        End If

        txtLongitudVivienda.Text = txtLongitudVivienda.Text.Replace("--", "").Replace("'", "").Replace("@", "")

    End Sub


    Private Sub txtAnchoVivienda_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAnchoVivienda.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtAnchoVivienda.Text.Contains(arrayCaractProhib(carp)) Then
                txtAnchoVivienda.Text = txtAnchoVivienda.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtAnchoVivienda.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtAnchoVivienda.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtAnchoVivienda.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtAnchoVivienda.Text.Length

                    If longitud > (lugar + 1) Then
                        txtAnchoVivienda.Text = txtAnchoVivienda.Text.Substring(0, lugar) & txtAnchoVivienda.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtAnchoVivienda.Text = txtAnchoVivienda.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtAnchoVivienda.Select(txtAnchoVivienda.Text.Length, 0)
        End If

        txtAnchoVivienda.Text = txtAnchoVivienda.Text.Replace("--", "").Replace("'", "").Replace("@", "")

    End Sub


    Private Sub btnPaso2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaso2.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        paso2 = True

        If validaDatosIniciales(False, True) = False Then

            paso2 = False
            Exit Sub

        End If

        If isEdit = False Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            imodelid = getSQLQueryAsInteger(0, "SELECT IF(MAX(imodelid) + 1 IS NULL, 1, MAX(imodelid) + 1) AS imodelid FROM models")

            Dim queriesDropCreate(35) As String

            queriesDropCreate(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0"
            queriesDropCreate(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " ( `imodelid` int(11) NOT NULL AUTO_INCREMENT, `smodelname` varchar(200) CHARACTER SET latin1 NOT NULL, `smodeltype` varchar(200) CHARACTER SET latin1 NOT NULL, `dmodellength` varchar(100) CHARACTER SET latin1 NOT NULL, `dmodelwidth` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `smodelfileslocation` varchar(1000) CHARACTER SET latin1 NOT NULL, `dmodelindirectpercentagedefault` decimal(20,5) NOT NULL, `dmodelgainpercentagedefault` decimal(20,5) NOT NULL, `dmodelIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0IndirectCosts"
            queriesDropCreate(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts" & " ( `imodelid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `smodelcostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodelcost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icostid`), KEY `modelid` (`imodelid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Cards"
            queriesDropCreate(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `legacycategoryid` (`scardlegacycategoryid`), KEY `legacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardInputs"
            queriesDropCreate(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardCompoundInputs"
            queriesDropCreate(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Prices"
            queriesDropCreate(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices" & " ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `inputid` (`iinputid`), KEY `modelid` (`imodelid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Explosion"
            'queriesDropCreate(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputrealqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Timber"
            queriesDropCreate(15) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0AdminCosts"
            'queriesDropCreate(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts ( `imodelid` int(11) NOT NULL, `iadmincostid` int(11) NOT NULL, `smodeladmincostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodeladmincost` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iadmincostid`), KEY `modelid` (`imodelid`), KEY `admincostid` (`iadmincostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0CardCompoundInputs"
            queriesDropCreate(19) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(20) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0CardInputs"
            queriesDropCreate(21) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(22) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0Cards"
            queriesDropCreate(23) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(24) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0IndirectCosts"
            queriesDropCreate(25) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(26) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0Prices"
            queriesDropCreate(27) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(28) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0"
            queriesDropCreate(29) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(30) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0Timber"
            queriesDropCreate(31) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(32) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardsAux"

            queriesDropCreate(33) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " VALUES (" & imodelid & ", '" & txtNombreModelo.Text & "', '" & validaTipoDeConstruccion() & "', " & txtLongitudVivienda.Text & ", " & txtAnchoVivienda.Text & ", '" & txtRuta.Text.Replace("\", "/") & "', " & txtPorcentajeIndirectosDefault.Text & ", " & txtPorcentajeUtilidadDefault.Text & ", " & txtPorcentajeIVA.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            queriesDropCreate(34) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts VALUES (" & imodelid & ", 1, 'DATO FALSO - SOLO PARA ACELERAR LA CAPTURA DE LOS MODELOS', 40000, 400000, " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesDropCreate)

            Dim queryCostosModelo As String = ""

            queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
            "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc " & _
            "WHERE pc.imodelid = " & imodelid

            setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

            dgvCostosIndirectos.Columns(0).Visible = False
            dgvCostosIndirectos.Columns(1).Visible = False
            dgvCostosIndirectos.Columns(4).Visible = False

            dgvCostosIndirectos.Columns(0).ReadOnly = True
            dgvCostosIndirectos.Columns(1).ReadOnly = True
            dgvCostosIndirectos.Columns(4).ReadOnly = True

            dgvCostosIndirectos.Columns(0).Width = 30
            dgvCostosIndirectos.Columns(1).Width = 30
            dgvCostosIndirectos.Columns(2).Width = 200
            dgvCostosIndirectos.Columns(3).Width = 100
            dgvCostosIndirectos.Columns(4).Width = 30


            'Dim dsCategorias As DataSet
            'Dim contadorCategorias As Integer = 0
            'Dim resumenDeTarjetas As String = ""
            'dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            'contadorCategorias = dsCategorias.Tables(0).Rows.Count

            'Dim queriesFill(contadorCategorias) As String

            'Dim queriesCreation(2) As String

            'queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
            'queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'executeTransactedSQLCommand(0, queriesCreation)

            'Try

            '    For i As Integer = 0 To contadorCategorias - 1

            '        Dim queryContadorDeTarjetas As String = "" & _
            '        "SELECT COUNT(*) " & _
            '        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

            '        Dim contadorDeTarjetas As Integer = 0

            '        contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

            '        If contadorDeTarjetas > 0 Then

            '            queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
            '            "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
            '            "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
            '            "UNION " & _
            '            "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
            '            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
            '            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
            '            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
            '            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
            '            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
            '            "UNION " & _
            '            "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
            '            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
            '            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
            '            "'' AS dcardunitaryprice, " & _
            '            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
            '            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
            '            "UNION " & _
            '            "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
            '            "ORDER BY scardlegacyid "

            '        End If

            '    Next i

            '    executeTransactedSQLCommand(0, queriesFill)

            'Catch ex As Exception

            'End Try


            'setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

            'dgvResumenDeTarjetas.Columns(0).Visible = False

            'If viewDgvIndirectCosts = False Then
            '    dgvResumenDeTarjetas.Columns(5).Visible = False
            'End If

            'If viewDgvProfits = False Then
            '    dgvResumenDeTarjetas.Columns(6).Visible = False
            'End If

            'If viewDgvUnitPrices = False Then
            '    dgvResumenDeTarjetas.Columns(7).Visible = False
            'End If

            'If viewDgvAmount = False Then
            '    dgvResumenDeTarjetas.Columns(8).Visible = False
            'End If

            'dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            'dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            'dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            'dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            'dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            'dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            'dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            'dgvResumenDeTarjetas.Columns(0).Width = 30
            'dgvResumenDeTarjetas.Columns(1).Width = 60
            'dgvResumenDeTarjetas.Columns(2).Width = 200
            'dgvResumenDeTarjetas.Columns(3).Width = 60
            'dgvResumenDeTarjetas.Columns(4).Width = 70
            'dgvResumenDeTarjetas.Columns(5).Width = 70
            'dgvResumenDeTarjetas.Columns(6).Width = 70
            'dgvResumenDeTarjetas.Columns(7).Width = 70
            'dgvResumenDeTarjetas.Columns(8).Width = 70


        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET smodelname = '" & txtNombreModelo.Text & "', smodeltype = '" & validaTipoDeConstruccion() & "', dmodellength = " & txtLongitudVivienda.Text & ", dmodelwidth = " & txtLongitudVivienda.Text & ", smodelfileslocation = '" & txtRuta.Text.Replace("\", "/") & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)

        End If


        tcModelo.SelectedTab = tpCostosIndirectos
        dgvCostosIndirectos.Select()
        dgvCostosIndirectos.Focus()

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnAbrirCarpeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrirCarpeta.Click

        If txtRuta.Text.Trim = "" Then
            Exit Sub
        Else

            Try
                MkDir(txtRuta.Text)
            Catch ex As Exception

            End Try

            Try
                System.Diagnostics.Process.Start(txtRuta.Text)
            Catch ex As Exception

            End Try

        End If

    End Sub


    Private Sub tcModelo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcModelo.SelectedIndexChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If validaDatosIniciales(True, True) = True And (tcModelo.SelectedTab Is tpDatosIniciales) = False Then
            'Continue

            'If verifyLicense(False, True) = False And isEdit = False And contarModelos() >= 3 Then

            '    If verifyLicense(False, False) = False Then

            '        Exit Sub

            '    End If

            'End If

            If validaCostosIndirectos(True) = True And (tcModelo.SelectedTab Is tpDatosIniciales) = False And (tcModelo.SelectedTab Is tpCostosIndirectos) = False Then
                'Continue
                If validaResumenDeTarjetas(True) = True Then
                    'Continue
                    'If validaAnalisisUtilidades(True) = True Then
                    '    'Continue
                    'Else
                    '    tcModelo.SelectedTab = tpAnalisisUtilidades
                    'End If
                Else
                    tcModelo.SelectedTab = tpResumenTarjetas
                End If

            Else
                tcModelo.SelectedTab = tpCostosIndirectos
            End If


            If tcModelo.SelectedTab Is tpCostosIndirectos Then


                Dim queryCostosModelo As String = ""

                queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
                "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc " & _
                "WHERE pc.imodelid = " & imodelid

                setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

                dgvCostosIndirectos.Columns(0).Visible = False
                dgvCostosIndirectos.Columns(1).Visible = False
                dgvCostosIndirectos.Columns(4).Visible = False

                dgvCostosIndirectos.Columns(0).ReadOnly = True
                dgvCostosIndirectos.Columns(1).ReadOnly = True
                dgvCostosIndirectos.Columns(4).ReadOnly = True

                dgvCostosIndirectos.Columns(0).Width = 30
                dgvCostosIndirectos.Columns(1).Width = 30
                dgvCostosIndirectos.Columns(2).Width = 200
                dgvCostosIndirectos.Columns(3).Width = 100
                dgvCostosIndirectos.Columns(4).Width = 30


                Dim sumaCostosModelos As Double = 0.0
                Dim querySumaCostosModelos As String = ""
                querySumaCostosModelos = "SELECT SUM(pc.dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid & " GROUP BY imodelid"

                sumaCostosModelos = getSQLQueryAsDouble(0, querySumaCostosModelos)
                lblTotalIndirectos.Text = FormatCurrency(sumaCostosModelos, 2, TriState.True, TriState.False, TriState.True)

                Dim ingresosIndirectos As Double = 0.0
                Try
                    If dgvCostosIndirectos.RowCount > 0 Then
                        ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                    End If
                Catch ex As Exception

                End Try

                txtIngresosIndirectos.Text = ingresosIndirectos

                Dim porcentajeIndirectoSugerido As Double = 0.0
                Try
                    porcentajeIndirectoSugerido = sumaCostosModelos / ingresosIndirectos
                Catch ex As Exception

                End Try

                lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "


            ElseIf tcModelo.SelectedTab Is tpResumenTarjetas Then


                'Check/Save/Update First Project page

                If imodelid = 0 Then

                    Dim fecha As Integer = getMySQLDate()
                    Dim hora As String = getAppTime()

                    imodelid = getSQLQueryAsInteger(0, "SELECT IF(MAX(imodelid) + 1 IS NULL, 1, MAX(imodelid) + 1) AS imodelid FROM models")

                    Dim queriesDropCreate(34) As String

                    queriesDropCreate(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0"
                    queriesDropCreate(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " ( `imodelid` int(11) NOT NULL AUTO_INCREMENT, `smodelname` varchar(200) CHARACTER SET latin1 NOT NULL, `smodeltype` varchar(200) CHARACTER SET latin1 NOT NULL, `dmodellength` varchar(100) CHARACTER SET latin1 NOT NULL, `dmodelwidth` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `smodelfileslocation` varchar(1000) CHARACTER SET latin1 NOT NULL, `dmodelindirectpercentagedefault` decimal(20,5) NOT NULL, `dmodelgainpercentagedefault` decimal(20,5) NOT NULL, `dmodelIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0IndirectCosts"
                    queriesDropCreate(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts" & " ( `imodelid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `smodelcostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodelcost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icostid`), KEY `modelid` (`imodelid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Cards"
                    queriesDropCreate(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `legacycategoryid` (`scardlegacycategoryid`), KEY `legacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardInputs"
                    queriesDropCreate(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardCompoundInputs"
                    queriesDropCreate(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Prices"
                    queriesDropCreate(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices" & " ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `inputid` (`iinputid`), KEY `modelid` (`imodelid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    'queriesDropCreate(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Explosion"
                    'queriesDropCreate(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputrealqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Timber"
                    queriesDropCreate(15) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    'queriesDropCreate(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0AdminCosts"
                    'queriesDropCreate(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts ( `imodelid` int(11) NOT NULL, `iadmincostid` int(11) NOT NULL, `smodeladmincostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodeladmincost` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iadmincostid`), KEY `modelid` (`imodelid`), KEY `admincostid` (`iadmincostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs"
                    queriesDropCreate(19) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(20) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs"
                    queriesDropCreate(21) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(22) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards"
                    queriesDropCreate(23) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(24) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts"
                    queriesDropCreate(25) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(26) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices"
                    queriesDropCreate(27) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(28) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid
                    queriesDropCreate(29) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(30) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber"
                    queriesDropCreate(31) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                    queriesDropCreate(32) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardsAux"

                    queriesDropCreate(33) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " VALUES (" & imodelid & ", '" & txtNombreModelo.Text & "', '" & validaTipoDeConstruccion() & "', " & txtLongitudVivienda.Text & ", " & txtAnchoVivienda.Text & ", '" & txtRuta.Text.Replace("\", "/") & "', " & txtPorcentajeIndirectosDefault.Text & ", " & txtPorcentajeUtilidadDefault.Text & ", " & txtPorcentajeIVA.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"

                    executeTransactedSQLCommand(0, queriesDropCreate)

                Else

                    executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET smodelname = '" & txtNombreModelo.Text & "', smodeltype = '" & validaTipoDeConstruccion() & "', dmodellength = " & txtLongitudVivienda.Text & ", dmodelwidth = " & txtLongitudVivienda.Text & ", smodelfileslocation = '" & txtRuta.Text.Replace("\", "/") & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)

                End If



                'Empieza la carga del grid

                Dim dsCategorias As DataSet
                Dim contadorCategorias As Integer = 0
                Dim resumenDeTarjetas As String = ""
                dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
                contadorCategorias = dsCategorias.Tables(0).Rows.Count

                Dim queriesFill(contadorCategorias) As String

                Dim queriesCreation(2) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                Try

                    For i As Integer = 0 To contadorCategorias - 1

                        Dim queryContadorDeTarjetas As String = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                        Dim contadorDeTarjetas As Integer = 0

                        contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                        If contadorDeTarjetas > 0 Then

                            queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                            "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                            "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                            "'' AS dcardunitaryprice, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                            "ORDER BY scardlegacyid "

                        End If

                    Next i

                    executeTransactedSQLCommand(0, queriesFill)

                Catch ex As Exception

                End Try


                setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

                dgvResumenDeTarjetas.Columns(0).Visible = False

                If viewDgvIndirectCosts = False Then
                    dgvResumenDeTarjetas.Columns(5).Visible = False
                End If

                If viewDgvProfits = False Then
                    dgvResumenDeTarjetas.Columns(6).Visible = False
                End If

                If viewDgvUnitPrices = False Then
                    dgvResumenDeTarjetas.Columns(7).Visible = False
                End If

                If viewDgvAmount = False Then
                    dgvResumenDeTarjetas.Columns(8).Visible = False
                End If

                dgvResumenDeTarjetas.Columns(0).ReadOnly = True
                dgvResumenDeTarjetas.Columns(2).ReadOnly = True
                dgvResumenDeTarjetas.Columns(3).ReadOnly = True
                dgvResumenDeTarjetas.Columns(5).ReadOnly = True
                dgvResumenDeTarjetas.Columns(6).ReadOnly = True
                dgvResumenDeTarjetas.Columns(7).ReadOnly = True
                dgvResumenDeTarjetas.Columns(8).ReadOnly = True

                dgvResumenDeTarjetas.Columns(0).Width = 30
                dgvResumenDeTarjetas.Columns(1).Width = 60
                dgvResumenDeTarjetas.Columns(2).Width = 200
                dgvResumenDeTarjetas.Columns(3).Width = 60
                dgvResumenDeTarjetas.Columns(4).Width = 70
                dgvResumenDeTarjetas.Columns(5).Width = 70
                dgvResumenDeTarjetas.Columns(6).Width = 70
                dgvResumenDeTarjetas.Columns(7).Width = 70
                dgvResumenDeTarjetas.Columns(8).Width = 70


                Dim dsModelo As DataSet
                dsModelo = getSQLQueryAsDataset(0, "SELECT p.imodelid, p.iupdatedate, " & _
                "p.supdatetime, p.smodelname, p.smodeltype, p.dmodellength, p.dmodelwidth, " & _
                "p.smodelfileslocation, p.dmodelindirectpercentagedefault, p.dmodelgainpercentagedefault, " & _
                "p.dmodelIVApercentage " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p WHERE p.imodelid = " & imodelid)


                Dim porcentajeIVA As Double
                Try
                    porcentajeIVA = CDbl(dsModelo.Tables(0).Rows(0).Item("dmodelIVApercentage"))
                Catch ex As Exception
                    porcentajeIVA = 0.0
                End Try

                txtPorcentajeIVA.Text = porcentajeIVA * 100

                Dim porcentajeIndirectosDefault As Double
                Try
                    porcentajeIndirectosDefault = CDbl(dsModelo.Tables(0).Rows(0).Item("dmodelindirectpercentagedefault"))
                Catch ex As Exception
                    porcentajeIndirectosDefault = 0.0
                End Try

                txtPorcentajeIndirectosDefault.Text = porcentajeIndirectosDefault * 100

                Dim porcentajeUtilidadDefault As Double
                Try
                    porcentajeUtilidadDefault = CDbl(dsModelo.Tables(0).Rows(0).Item("dmodelgainpercentagedefault"))
                Catch ex As Exception
                    porcentajeUtilidadDefault = 0.0
                End Try

                txtPorcentajeUtilidadDefault.Text = porcentajeUtilidadDefault * 100


                Dim queryIndirectosSubTotal As String
                Dim indirectosSubTotal As Double = 0.0
                queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim queryPrecioSubTotal As String
                Dim precioSubTotal As Double = 0.0
                queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim precioTotal As Double = 0.0


                txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)
                txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                If isEdit = False Then

                    btnCostoHoy.Visible = False

                Else

                    btnCostoHoy.Visible = True

                End If



            ElseIf tcModelo.SelectedTab Is tpExplosionDeInsumos Then



                Dim queryExplosionInsumos As String = ""

                queryExplosionInsumos = "" & _
                "SELECT t2.imodelid, t2.iinputid, t2.sinputdescription AS 'Insumo', t2.sinputunit AS 'Unidad de Medida', " & _
                "FORMAT(SUM(t2.dcardinputqty), 3) AS 'Cantidad Requerida' " & _
                "FROM ( " & _
                "SELECT " & _
                "t1.imodelid, t1.iinputid, t1.quequery, t1.sinputdescription, t1.sinputunit, " & _
                "t1.dcardinputqty " & _
                "FROM ( " & _
                "SELECT " & _
                "ptfi.imodelid, ptfi.iinputid, 'normal' as quequery, i.sinputdescription, i.sinputunit, " & _
                "IF(SUM(ptfi.dcardinputqty*ptf.dcardqty) IS NULL, 0, SUM(ptfi.dcardinputqty*ptf.dcardqty)) AS dcardinputqty, " & _
                "IF(ptf.dcardqty IS NULL, 0, ptf.dcardqty) AS dcardqty " & _
                "FROM " & _
                "tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid " & _
                "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
                "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                "JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "WHERE it.sinputtypedescription = 'MATERIALES' AND p.imodelid = " & imodelid & " " & _
                "GROUP BY ptfi.imodelid, ptfi.iinputid " & _
                "UNION " & _
                "SELECT ptfi.imodelid, ptfci.icompoundinputid AS iinputid, 'compound' as quequery, i.sinputdescription, i.sinputunit, " & _
                "IF(SUM(ptfci.dcompoundinputqty*ptfi.dcardinputqty*ptf.dcardqty) IS NULL, 0, SUM(ptfci.dcompoundinputqty*ptfi.dcardinputqty*ptf.dcardqty)) AS dcardinputqty, " & _
                "IF(ptf.dcardqty IS NULL, 0, ptf.dcardqty) AS dcardqty " & _
                "FROM " & _
                "tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfi.imodelid = ptfci.imodelid AND ptfi.icardid = ptfci.icardid AND ptfi.iinputid = ptfci.iinputid " & _
                "JOIN inputs i ON ptfci.icompoundinputid = i.iinputid " & _
                "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
                "JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND ptfci.icompoundinputid = pp.iinputid " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "WHERE it.sinputtypedescription = 'MATERIALES' AND p.imodelid = " & imodelid & " " & _
                "GROUP BY ptfci.imodelid, ptfci.icompoundinputid " & _
                ") t1 " & _
                ")t2 " & _
                "GROUP BY t2.iinputid " & _
                "ORDER BY 3 "


                executeSQLCommand(0, queryExplosionInsumos)

                setDataGridView(dgvExplosionDeInsumos, queryExplosionInsumos, True)

                dgvExplosionDeInsumos.Columns(0).Visible = False
                dgvExplosionDeInsumos.Columns(1).Visible = False

                dgvExplosionDeInsumos.Columns(1).Width = 30
                dgvExplosionDeInsumos.Columns(2).Width = 300
                dgvExplosionDeInsumos.Columns(3).Width = 70
                dgvExplosionDeInsumos.Columns(4).Width = 70


            Else

                tcModelo.SelectedTab = tpDatosIniciales

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub tcInsumosObra_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcInsumosObra.SelectedIndexChanged


        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


        If tcInsumosObra.SelectedTab Is tpExplosion Then



            Dim queryExplosionInsumos As String = ""

            queryExplosionInsumos = "" & _
            "SELECT t2.imodelid, t2.iinputid, t2.sinputdescription AS 'Insumo', t2.sinputunit AS 'Unidad de Medida', " & _
            "FORMAT(SUM(t2.dcardinputqty), 3) AS 'Cantidad Requerida' " & _
            "FROM ( " & _
            "SELECT " & _
            "t1.imodelid, t1.iinputid, t1.quequery, t1.sinputdescription, t1.sinputunit, " & _
            "t1.dcardinputqty " & _
            "FROM ( " & _
            "SELECT " & _
            "ptfi.imodelid, ptfi.iinputid, 'normal' as quequery, i.sinputdescription, i.sinputunit, " & _
            "IF(SUM(ptfi.dcardinputqty*ptf.dcardqty) IS NULL, 0, SUM(ptfi.dcardinputqty*ptf.dcardqty)) AS dcardinputqty, " & _
            "IF(ptf.dcardqty IS NULL, 0, ptf.dcardqty) AS dcardqty " & _
            "FROM " & _
            "tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid " & _
            "JOIN inputs i ON ptfi.iinputid = i.iinputid " & _
            "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
            "JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "WHERE it.sinputtypedescription = 'MATERIALES' AND p.imodelid = " & imodelid & " " & _
            "GROUP BY ptfi.imodelid, ptfi.iinputid " & _
            "UNION " & _
            "SELECT ptfi.imodelid, ptfci.icompoundinputid AS iinputid, 'compound' as quequery, i.sinputdescription, i.sinputunit, " & _
            "IF(SUM(ptfci.dcompoundinputqty*ptfi.dcardinputqty*ptf.dcardqty) IS NULL, 0, SUM(ptfci.dcompoundinputqty*ptfi.dcardinputqty*ptf.dcardqty)) AS dcardinputqty, " & _
            "IF(ptf.dcardqty IS NULL, 0, ptf.dcardqty) AS dcardqty " & _
            "FROM " & _
            "tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfi.imodelid = ptfci.imodelid AND ptfi.icardid = ptfci.icardid AND ptfi.iinputid = ptfci.iinputid " & _
            "JOIN inputs i ON ptfci.icompoundinputid = i.iinputid " & _
            "JOIN inputtypes it ON i.iinputid = it.iinputid " & _
            "JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND ptfci.icompoundinputid = pp.iinputid " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "WHERE it.sinputtypedescription = 'MATERIALES' AND p.imodelid = " & imodelid & " " & _
            "GROUP BY ptfci.imodelid, ptfci.icompoundinputid " & _
            ") t1 " & _
            ")t2 " & _
            "GROUP BY t2.iinputid " & _
            "ORDER BY 3 "

            executeSQLCommand(0, queryExplosionInsumos)

            setDataGridView(dgvExplosionDeInsumos, queryExplosionInsumos, True)

            dgvExplosionDeInsumos.Columns(0).Visible = False
            dgvExplosionDeInsumos.Columns(1).Visible = False

            dgvExplosionDeInsumos.Columns(1).Width = 30
            dgvExplosionDeInsumos.Columns(2).Width = 300
            dgvExplosionDeInsumos.Columns(3).Width = 70
            dgvExplosionDeInsumos.Columns(4).Width = 70


        End If


        Cursor.Current = System.Windows.Forms.Cursors.Default


    End Sub


    Private Function validaDatosIniciales(ByVal silent As Boolean, ByVal quick As Boolean) As Boolean

        If isFormReadyForAction = False Then
            Exit Function
        End If

        alertaMostrada = False

        txtNombreModelo.Text = txtNombreModelo.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtLongitudVivienda.Text = txtLongitudVivienda.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")
        txtAnchoVivienda.Text = txtAnchoVivienda.Text.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

        If quick = False Then

            If txtNombreModelo.Text = "" Then

                If silent = False Then
                    MsgBox("¿Podrías ponerle nombre al Modelo?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                tcModelo.SelectedTab = tpDatosIniciales
                txtNombreModelo.Select()
                txtNombreModelo.Focus()
                alertaMostrada = True
                Return False

            End If

        End If


        Dim longitudVivienda As Double = 0.0
        Dim anchoVivienda As Double = 0.0

        Try
            longitudVivienda = CDbl(txtLongitudVivienda.Text)
            anchoVivienda = CDbl(txtAnchoVivienda.Text)
        Catch ex As Exception

        End Try

        If longitudVivienda = 0.0 Or anchoVivienda = 0.0 Then

            If silent = False Then
                MsgBox("¿Qué medidas tiene la construcción del Modelo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            tcModelo.SelectedTab = tpDatosIniciales
            txtLongitudVivienda.Select()
            txtLongitudVivienda.Focus()
            alertaMostrada = True
            Return False

        End If


        If quick = False Then

            If rbCasa.Checked = False And rbOficina.Checked = False And rbOtro.Checked = False Then

                If silent = False Then
                    MsgBox("¿Podrías escoger de qué tipo de Construcción se trata el Modelo?", MsgBoxStyle.OkOnly, "Dato Faltante")
                End If
                tcModelo.SelectedTab = tpDatosIniciales
                rbCasa.Select()
                rbCasa.Focus()
                alertaMostrada = True
                Return False

            End If

        End If

        If isEdit = False Then

            If paso2 = False Then

                Return False

            End If

        End If

        Return True

    End Function


    Private Function validaTipoDeConstruccion() As String

        If rbCasa.Checked = True Then
            Return "Casa Habitación"
        ElseIf rbOficina.Checked = True Then
            Return "Oficina"
        ElseIf rbOtro.Checked = True Then
            Return "Otro"
        End If

        Return ""

    End Function


    Private Sub dgvCostosIndirectos_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellClick

        Try

            If dgvCostosIndirectos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcostid = CInt(dgvCostosIndirectos.Rows(e.RowIndex).Cells(1).Value())
            sselectedcostdescription = dgvCostosIndirectos.Rows(e.RowIndex).Cells(2).Value()
            dselectedcostamount = CDbl(dgvCostosIndirectos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

            iselectedcostid = 0
            sselectedcostdescription = ""
            dselectedcostamount = 1.0

        End Try

    End Sub


    Private Sub dgvCostosIndirectos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellContentClick

        Try

            If dgvCostosIndirectos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcostid = CInt(dgvCostosIndirectos.Rows(e.RowIndex).Cells(1).Value())
            sselectedcostdescription = dgvCostosIndirectos.Rows(e.RowIndex).Cells(2).Value()
            dselectedcostamount = CDbl(dgvCostosIndirectos.Rows(e.RowIndex).Cells(3).Value())

        Catch ex As Exception

            iselectedcostid = 0
            sselectedcostdescription = ""
            dselectedcostamount = 1.0

        End Try

    End Sub


    Private Sub dgvCostosIndirectos_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvCostosIndirectos.SelectionChanged

        txtCantidadDgvCostosIndirectos = Nothing
        txtCantidadDgvCostosIndirectos_OldText = Nothing
        txtNombreDgvCostosIndirectos = Nothing
        txtNombreDgvCostosIndirectos_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If dgvCostosIndirectos.Rows.Count > 0 Then

            If dgvCostosIndirectos.Rows.Count = 1 Then
                Exit Sub
            End If

        End If

        Try

            If dgvCostosIndirectos.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcostid = CInt(dgvCostosIndirectos.CurrentRow.Cells(1).Value)
            sselectedcostdescription = dgvCostosIndirectos.CurrentRow.Cells(2).Value()
            dselectedcostamount = CDbl(dgvCostosIndirectos.CurrentRow.Cells(3).Value)

        Catch ex As Exception

            iselectedcostid = 0
            sselectedcostdescription = ""
            dselectedcostamount = 1.0

        End Try

    End Sub


    Private Sub dgvCostosIndirectos_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim queryCostosModelo As String = ""
        queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
        "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid
        setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

        dgvCostosIndirectos.Columns(0).Visible = False
        dgvCostosIndirectos.Columns(1).Visible = False
        dgvCostosIndirectos.Columns(4).Visible = False

        dgvCostosIndirectos.Columns(0).ReadOnly = True
        dgvCostosIndirectos.Columns(1).ReadOnly = True
        dgvCostosIndirectos.Columns(4).ReadOnly = True

        dgvCostosIndirectos.Columns(0).Width = 30
        dgvCostosIndirectos.Columns(1).Width = 30
        dgvCostosIndirectos.Columns(2).Width = 200
        dgvCostosIndirectos.Columns(3).Width = 100
        dgvCostosIndirectos.Columns(4).Width = 30

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvCostosIndirectos_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCostosIndirectos.CellValueChanged

        If modifyIndirectCosts = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 2 Y 3: smodelcostname y dmodelcost

        If e.ColumnIndex = 2 Then

            If dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Modelo") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                    Dim total As Double = 0.0
                    total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
                    lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                    Dim ingresosIndirectos As Double = 0.0

                    Try

                        If dgvCostosIndirectos.RowCount > 0 Then
                            ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                        End If

                    Catch ex As Exception

                    End Try

                    txtIngresosIndirectos.Text = ingresosIndirectos

                    Dim porcentajeIndirectoSugerido As Double = 0.0
                    Try
                        porcentajeIndirectoSugerido = total / ingresosIndirectos
                    Catch ex As Exception

                    End Try

                    lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Else

                    dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcostdescription

                End If

            Else

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts SET smodelcostname = '" & dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

            End If

        ElseIf e.ColumnIndex = 3 Then

            If dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Modelo") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                    Dim total As Double = 0.0
                    total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
                    lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                    Dim ingresosIndirectos As Double = 0.0
                    Try
                        If dgvCostosIndirectos.RowCount > 0 Then
                            ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                        End If
                    Catch ex As Exception

                    End Try

                    txtIngresosIndirectos.Text = ingresosIndirectos

                    Dim porcentajeIndirectoSugerido As Double = 0.0
                    Try
                        porcentajeIndirectoSugerido = total / ingresosIndirectos
                    Catch ex As Exception

                    End Try

                    lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Else

                    dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcostamount

                End If

            ElseIf dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Modelo") = MsgBoxResult.Yes Then

                    executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                    Dim total As Double = 0.0
                    total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
                    lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                    Dim ingresosIndirectos As Double = 0.0
                    Try
                        If dgvCostosIndirectos.RowCount > 0 Then
                            ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                        End If
                    Catch ex As Exception

                    End Try

                    txtIngresosIndirectos.Text = ingresosIndirectos

                    Dim porcentajeIndirectoSugerido As Double = 0.0
                    Try
                        porcentajeIndirectoSugerido = total / ingresosIndirectos
                    Catch ex As Exception

                    End Try

                    lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Else

                    dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcostamount

                End If

            Else

                Dim costoindirecto As Double = 0.0

                Try
                    costoindirecto = CDbl(dgvCostosIndirectos.Rows(e.RowIndex).Cells(3).Value)
                Catch ex As Exception
                    costoindirecto = dselectedcostamount
                End Try

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts SET dmodelcost = " & dgvCostosIndirectos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid & " AND icostid = " & dgvCostosIndirectos.CurrentRow.Cells(1).Value)

                Dim total As Double = 0.0
                total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
                lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                Dim ingresosIndirectos As Double = 0.0
                Try
                    If dgvCostosIndirectos.RowCount > 0 Then
                        ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                    End If
                Catch ex As Exception

                End Try

                txtIngresosIndirectos.Text = ingresosIndirectos

                Dim porcentajeIndirectoSugerido As Double = 0.0
                Try
                    porcentajeIndirectoSugerido = total / ingresosIndirectos
                Catch ex As Exception

                End Try

                lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvCostosIndirectos_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvCostosIndirectos.EditingControlShowing

        If dgvCostosIndirectos.CurrentCell.ColumnIndex = 2 Then

            txtNombreDgvCostosIndirectos = CType(e.Control, TextBox)
            txtNombreDgvCostosIndirectos_OldText = txtNombreDgvCostosIndirectos.Text

        ElseIf dgvCostosIndirectos.CurrentCell.ColumnIndex = 3 Then

            txtCantidadDgvCostosIndirectos = CType(e.Control, TextBox)
            txtCantidadDgvCostosIndirectos_OldText = txtCantidadDgvCostosIndirectos.Text

        Else
            txtCantidadDgvCostosIndirectos = Nothing
            txtCantidadDgvCostosIndirectos_OldText = Nothing
            txtNombreDgvCostosIndirectos = Nothing
            txtNombreDgvCostosIndirectos_OldText = Nothing
        End If

    End Sub


    Private Sub txtNombreDgvCostosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDgvCostosIndirectos.KeyUp

        txtNombreDgvCostosIndirectos.Text = txtNombreDgvCostosIndirectos.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNombreDgvCostosIndirectos.Text.Contains(arrayForbidden1(fc)) Then
                txtNombreDgvCostosIndirectos.Text = txtNombreDgvCostosIndirectos.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtCantidadDgvCostosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvCostosIndirectos.KeyUp

        If Not IsNumeric(txtCantidadDgvCostosIndirectos.Text) Then

            Dim strForbidden2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

            For cp = 0 To arrayForbidden2.Length - 1

                If txtCantidadDgvCostosIndirectos.Text.Contains(arrayForbidden2(cp)) Then
                    txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Replace(arrayForbidden2(cp), "")
                End If

            Next cp

            If txtCantidadDgvCostosIndirectos.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvCostosIndirectos.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvCostosIndirectos.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvCostosIndirectos.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Substring(0, lugar) & txtCantidadDgvCostosIndirectos.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvCostosIndirectos.Text = txtCantidadDgvCostosIndirectos.Text.Trim

        Else
            txtCantidadDgvCostosIndirectos_OldText = txtCantidadDgvCostosIndirectos.Text
        End If

    End Sub


    Private Sub dgvCostosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvCostosIndirectos.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteIndirectCosts = False Then
                Exit Sub
            End If

            If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Modelo") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedcostid As Integer = 0
                Try
                    tmpselectedcostid = CInt(dgvCostosIndirectos.CurrentRow.Cells(1).Value)
                Catch ex As Exception

                End Try

                executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid & " AND icostid = " & tmpselectedcostid)

                Dim queryCostosModelo As String = ""
                queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
                "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid
                setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

                dgvCostosIndirectos.Columns(0).ReadOnly = True
                dgvCostosIndirectos.Columns(1).ReadOnly = True
                dgvCostosIndirectos.Columns(4).ReadOnly = True

                dgvCostosIndirectos.Columns(0).Width = 30
                dgvCostosIndirectos.Columns(1).Width = 30
                dgvCostosIndirectos.Columns(2).Width = 200
                dgvCostosIndirectos.Columns(3).Width = 100
                dgvCostosIndirectos.Columns(4).Width = 30

                dgvCostosIndirectos.Columns(0).Visible = False
                dgvCostosIndirectos.Columns(1).Visible = False
                dgvCostosIndirectos.Columns(4).Visible = False

                Dim total As Double = 0.0
                total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
                lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

                Dim ingresosIndirectos As Double = 0.0
                Try
                    If dgvCostosIndirectos.RowCount > 0 Then
                        ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                    End If
                Catch ex As Exception

                End Try

                txtIngresosIndirectos.Text = ingresosIndirectos

                Dim porcentajeIndirectoSugerido As Double = 0.0
                Try
                    porcentajeIndirectoSugerido = total / ingresosIndirectos
                Catch ex As Exception

                End Try

                lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub dgvCostosIndirectos_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvCostosIndirectos.UserAddedRow

        If addIndirectCosts = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedcostid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icostid) + 1 IS NULL, 1, MAX(icostid) + 1) AS icostid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)

        dgvCostosIndirectos.CurrentRow.Cells(0).Value = imodelid
        dgvCostosIndirectos.CurrentRow.Cells(1).Value = iselectedcostid
        dgvCostosIndirectos.CurrentRow.Cells(3).Value = 1

        Dim valor As Double = 0.0

        Try
            valor = CDbl(txtIngresosIndirectos.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", ""))
        Catch ex As Exception

        End Try

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts VALUES (" & imodelid & ", " & iselectedcostid & ", '" & dgvCostosIndirectos.CurrentRow.Cells(2).Value & "', " & dgvCostosIndirectos.CurrentRow.Cells(3).Value & ", " & valor & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        Dim total As Double = 0.0
        total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
        lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

        Dim ingresosIndirectos As Double = 0.0

        Try
            If dgvCostosIndirectos.RowCount > 0 Then
                ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
            End If
        Catch ex As Exception

        End Try

        txtIngresosIndirectos.Text = ingresosIndirectos

        Dim porcentajeIndirectoSugerido As Double = 0.0
        Try
            porcentajeIndirectoSugerido = total / ingresosIndirectos
        Catch ex As Exception

        End Try

        lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtIngresosIndirectos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtIngresosIndirectos.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtIngresosIndirectos.Text.Contains(arrayCaractProhib(carp)) Then
                txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtIngresosIndirectos.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtIngresosIndirectos.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtIngresosIndirectos.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtIngresosIndirectos.Text.Length

                    If longitud > (lugar + 1) Then
                        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Substring(0, lugar) & txtIngresosIndirectos.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtIngresosIndirectos.Select(txtIngresosIndirectos.Text.Length, 0)
        End If

        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Replace("--", "").Replace("'", "")
        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Trim

    End Sub


    Private Sub txtIngresosIndirectos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIngresosIndirectos.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        If modifyIndirectCosts = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtIngresosIndirectos.Text = txtIngresosIndirectos.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0

        Try

            valor = CDbl(txtIngresosIndirectos.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts SET dcompanyprojectedearnings = " & valor & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)

            Dim queryCostosModelo As String = ""
            queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
            "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid
            setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

            dgvCostosIndirectos.Columns(0).Visible = False
            dgvCostosIndirectos.Columns(1).Visible = False
            dgvCostosIndirectos.Columns(4).Visible = False

            dgvCostosIndirectos.Columns(0).ReadOnly = True
            dgvCostosIndirectos.Columns(1).ReadOnly = True
            dgvCostosIndirectos.Columns(4).ReadOnly = True

            dgvCostosIndirectos.Columns(0).Width = 30
            dgvCostosIndirectos.Columns(1).Width = 30
            dgvCostosIndirectos.Columns(2).Width = 200
            dgvCostosIndirectos.Columns(3).Width = 100
            dgvCostosIndirectos.Columns(4).Width = 30

            Dim total As Double = 0.0
            total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
            lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

            Dim ingresosIndirectos As Double = 0.0
            Try
                If dgvCostosIndirectos.RowCount > 0 Then
                    ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                End If
            Catch ex As Exception

            End Try

            Dim porcentajeIndirectoSugerido As Double = 0.0
            Try
                porcentajeIndirectoSugerido = total / ingresosIndirectos
            Catch ex As Exception

            End Try

            lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevoCostoIndirecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevoCostoIndirecto.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        iselectedcostid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icostid) + 1 IS NULL, 1, MAX(icostid) + 1) AS icostid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)

        dgvCostosIndirectos.CurrentRow.Cells(0).Value = imodelid
        dgvCostosIndirectos.CurrentRow.Cells(1).Value = iselectedcostid
        dgvCostosIndirectos.CurrentRow.Cells(3).Value = 1

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtIngresosIndirectos.Text.Trim.Replace("--", "").Replace("'", "").Replace("@", ""))
        Catch ex As Exception

        End Try

        executeSQLCommand(0, "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts VALUES (" & imodelid & ", " & iselectedcostid & ", '" & dgvCostosIndirectos.CurrentRow.Cells(2).Value & "', " & dgvCostosIndirectos.CurrentRow.Cells(3).Value & ", " & valor & ", " & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "')")

        Dim queryCostosModelo As String = ""
        queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
        "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid
        setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

        dgvCostosIndirectos.Columns(0).Visible = False
        dgvCostosIndirectos.Columns(1).Visible = False
        dgvCostosIndirectos.Columns(4).Visible = False

        dgvCostosIndirectos.Columns(0).ReadOnly = True
        dgvCostosIndirectos.Columns(1).ReadOnly = True
        dgvCostosIndirectos.Columns(4).ReadOnly = True

        dgvCostosIndirectos.Columns(0).Width = 30
        dgvCostosIndirectos.Columns(1).Width = 30
        dgvCostosIndirectos.Columns(2).Width = 200
        dgvCostosIndirectos.Columns(3).Width = 100
        dgvCostosIndirectos.Columns(4).Width = 30


        Dim total As Double = 0.0
        total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
        lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

        Dim ingresosIndirectos As Double = 0.0
        Try
            If dgvCostosIndirectos.RowCount > 0 Then
                ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
            End If
        Catch ex As Exception

        End Try

        txtIngresosIndirectos.Text = ingresosIndirectos

        Dim porcentajeIndirectoSugerido As Double = 0.0
        Try
            porcentajeIndirectoSugerido = total / ingresosIndirectos
        Catch ex As Exception

        End Try

        lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarCostoIndirecto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarCostoIndirecto.Click

        If MsgBox("¿Estás seguro de que quieres eliminar este Costo Indirecto del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Costo Indirecto del Modelo") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedcostid As Integer = 0
            Try
                tmpselectedcostid = CInt(dgvCostosIndirectos.CurrentRow.Cells(1).Value)
            Catch ex As Exception

            End Try

            executeSQLCommand(0, "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid & " AND icostid = " & tmpselectedcostid)

            Dim queryCostosModelo As String = ""
            queryCostosModelo = "SELECT pc.imodelid, pc.icostid, pc.smodelcostname AS 'Nombre del Costo', " & _
            "FORMAT(pc.dmodelcost, 2) AS 'Importe del Costo', pc.dcompanyprojectedearnings FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid
            setDataGridView(dgvCostosIndirectos, queryCostosModelo, False)

            dgvCostosIndirectos.Columns(0).Visible = False
            dgvCostosIndirectos.Columns(1).Visible = False
            dgvCostosIndirectos.Columns(4).Visible = False

            dgvCostosIndirectos.Columns(0).ReadOnly = True
            dgvCostosIndirectos.Columns(1).ReadOnly = True
            dgvCostosIndirectos.Columns(4).ReadOnly = True

            dgvCostosIndirectos.Columns(0).Width = 30
            dgvCostosIndirectos.Columns(1).Width = 30
            dgvCostosIndirectos.Columns(2).Width = 200
            dgvCostosIndirectos.Columns(3).Width = 100
            dgvCostosIndirectos.Columns(4).Width = 30

            Dim total As Double = 0.0
            total = getSQLQueryAsDouble(0, "SELECT SUM(dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts WHERE imodelid = " & imodelid)
            lblTotalIndirectos.Text = FormatCurrency(total, 2, TriState.True, TriState.False, TriState.True)

            Dim ingresosIndirectos As Double = 0.0
            Try
                If dgvCostosIndirectos.RowCount > 0 Then
                    ingresosIndirectos = CDbl(dgvCostosIndirectos.Rows(0).Cells(4).Value)
                End If
            Catch ex As Exception

            End Try

            txtIngresosIndirectos.Text = ingresosIndirectos

            Dim porcentajeIndirectoSugerido As Double = 0.0
            Try
                porcentajeIndirectoSugerido = total / ingresosIndirectos
            Catch ex As Exception

            End Try

            lblPorcentajeIndirectos.Text = FormatCurrency(porcentajeIndirectoSugerido * 100, 2, TriState.True, TriState.False, TriState.True).Replace("$", "") & " % "

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Function validaCostosIndirectos(ByVal silent As Boolean) As Boolean

        If isFormReadyForAction = False Then
            Exit Function
        End If

        Dim querySumaCostosModelos As String = ""
        Dim sumaCostosModelos As Double = 0.0

        querySumaCostosModelos = "SELECT SUM(pc.dmodelcost) AS dmodelcost FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts pc WHERE pc.imodelid = " & imodelid & " GROUP BY imodelid"

        sumaCostosModelos = getSQLQueryAsDouble(0, querySumaCostosModelos)

        If sumaCostosModelos = 0.0 Then

            If silent = False Then
                MsgBox("¿Podrías hacer el cálculo de costos indirectos del Modelo?", MsgBoxStyle.OkOnly, "Dato Faltante")
            End If

            alertaMostrada = True
            Return False

        End If

        If isEdit = False Then

            If paso3 = False Then

                Return False

            End If

        End If

        Return True


    End Function


    Private Sub btnPaso3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaso3.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        paso3 = True

        If validaCostosIndirectos(False) = False Then

            MsgBox("¿Podrías ponerle Costos Indirectos al Modelo?", MsgBoxStyle.OkOnly, "Dato Faltante")

            paso3 = False

            Exit Sub

        Else

            tcModelo.SelectedTab = tpResumenTarjetas

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellClick

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

    End Sub


    Private Sub dgvResumenDeTarjetas_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellContentClick

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

    End Sub


    Private Sub dgvResumenDeTarjetas_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvResumenDeTarjetas.SelectionChanged

        txtLegacyIdDgvResumenDeTarjetas = Nothing
        txtLegacyIdDgvResumenDeTarjetas_OldText = Nothing
        txtNombreDgvResumenDeTarjetas = Nothing
        txtNombreDgvResumenDeTarjetas_OldText = Nothing
        txtCantidadDgvResumenDeTarjetas = Nothing
        txtCantidadDgvResumenDeTarjetas_OldText = Nothing

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)
            sselectedcardlegacyid = dgvResumenDeTarjetas.CurrentRow.Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.CurrentRow.Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.CurrentRow.Cells(4).Value)

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

    End Sub


    Private Sub dgvResumenDeTarjetas_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellValueChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'LAS UNICAS COLUMNAS EDITABLES SON LAS COLUMNAS 1, 2 Y 4: LEGACYID, SCARDDESCRIPTION Y DCARDQTY

        If isResumenDGVReady = False Then
            Exit Sub
        End If

        If modifyCardQty = False Then
            Exit Sub
        End If

        Dim baseid As Integer = 0
        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        Dim dsBusquedaTarjetasRepetidas As DataSet
        Dim dsBusquedaTarjetasBase As DataSet

        If e.ColumnIndex = 1 Then

            'SCardLegacyID

            If dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar la Tarjeta " & sselectedcardlegacyid & " del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Tarjeta del Modelo") = MsgBoxResult.Yes Then

                    Dim queriesDelete(3) As String

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                    queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

                    executeTransactedSQLCommand(0, queriesDelete)

                    Dim porcentajeIVA As Double = 0.0
                    porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                    txtPorcentajeIVA.Text = porcentajeIVA * 100


                    Dim queryIndirectosSubTotal As String
                    Dim indirectosSubTotal As Double = 0.0
                    queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE p.imodelid = " & imodelid

                    indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                    txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    Dim queryPrecioSubTotal As String
                    Dim precioSubTotal As Double = 0.0
                    queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE p.imodelid = " & imodelid

                    precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                    txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    Dim precioTotal As Double = 0.0
                    precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                    txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Else
                    'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid
                End If

            Else

                'Si pone un texto, e.g. un LegacyID...

                dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND scardlegacyid = '" & dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Modelo. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    Exit Sub

                End If

                dsBusquedaTarjetasBase = getSQLQueryAsDataset(0, "SELECT * FROM basecards WHERE ibaseid = " & baseid & " AND scardlegacyid = '" & dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "'")

                If dsBusquedaTarjetasBase.Tables(0).Rows.Count > 0 Then

                    'Si sí encuentro una Tarjeta con ese LegacyId

                    If MsgBox("¿Estás seguro de que deseas reemplazar la Tarjeta " & sselectedcardlegacyid & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Tarjeta") = MsgBoxResult.Yes Then

                        Dim cantidaddeTarjeta As Double = 1

                        Try
                            cantidaddeTarjeta = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value)
                        Catch ex As Exception

                        End Try

                        Dim fecha As Integer = 0
                        Dim hora As String = "00:00:00"

                        fecha = getMySQLDate()
                        hora = getAppTime()

                        Dim queriesReplace(6) As String

                        queriesReplace(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                        queriesReplace(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                        queriesReplace(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

                        queriesReplace(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT " & imodelid & ", " & iselectedcardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & dsBusquedaTarjetasBase.Tables(0).Rows(0).Item("icardid")
                        queriesReplace(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT " & imodelid & ", " & iselectedcardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & dsBusquedaTarjetasBase.Tables(0).Rows(0).Item("icardid")
                        queriesReplace(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT " & imodelid & ", " & iselectedcardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & dsBusquedaTarjetasBase.Tables(0).Rows(0).Item("icardid")

                        executeTransactedSQLCommand(0, queriesReplace)

                        Dim porcentajeIVA As Double = 0.0
                        porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                        txtPorcentajeIVA.Text = porcentajeIVA * 100

                        Dim queryIndirectosSubTotal As String
                        Dim indirectosSubTotal As Double = 0.0
                        queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE p.imodelid = " & imodelid

                        indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                        txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                        txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                        Dim queryPrecioSubTotal As String
                        Dim precioSubTotal As Double = 0.0
                        queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE p.imodelid = " & imodelid

                        precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                        txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                        txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                        Dim precioTotal As Double = 0.0
                        precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                        txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                    Else

                        'Si cancela el reemplazo de Tarjeta
                        'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid

                    End If

                Else

                    'Si no encuentro una Tarjeta con ese LegacyId

                    If MsgBox("No encontré Tarjetas con esa clave." & Chr(13) & "¿Estás seguro de que deseas reemplazar esta Tarjeta?" & Chr(13) & "Presiona 'Sí' para que te muestre la ventana de búsqueda de Tarjetas o 'No' para regresar a la ventana anterior.", MsgBoxStyle.YesNo, "Clave no encontrada") = MsgBoxResult.Yes Then

                        Dim bf As New BuscaTarjetas
                        bf.susername = susername
                        bf.bactive = bactive
                        bf.bonline = bonline
                        bf.suserfullname = suserfullname
                        bf.suseremail = suseremail
                        bf.susersession = susersession
                        bf.susermachinename = susermachinename
                        bf.suserip = suserip

                        bf.iprojectid = imodelid

                        bf.querystring = dgvResumenDeTarjetas.CurrentCell.EditedFormattedValue

                        bf.isEdit = False

                        bf.isBase = False
                        bf.isModel = False

                        bf.isHistoric = isHistoric

                        If Me.WindowState = FormWindowState.Maximized Then
                            bf.WindowState = FormWindowState.Maximized
                        End If

                        Me.Visible = False
                        bf.ShowDialog(Me)
                        Me.Visible = True

                        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

                            dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & bf.icardid)

                            If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                                MsgBox("Ya tienes esa Tarjeta insertada en este Modelo. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                                Exit Sub

                            End If


                            If MsgBox("¿Estás seguro de que deseas reemplazar la Tarjeta " & sselectedcardlegacyid & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Tarjeta") = MsgBoxResult.Yes Then

                                Dim cantidaddeTarjeta As Double = 1

                                Try
                                    cantidaddeTarjeta = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value)
                                Catch ex As Exception

                                End Try

                                Dim fecha As Integer = 0
                                Dim hora As String = "00:00:00"

                                fecha = getMySQLDate()
                                hora = getAppTime()

                                Dim queriesReplace(6) As String

                                queriesReplace(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                                queriesReplace(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                                queriesReplace(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

                                queriesReplace(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                                queriesReplace(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                                queriesReplace(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT " & imodelid & ", " & bf.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

                                executeTransactedSQLCommand(0, queriesReplace)

                                Dim porcentajeIVA As Double = 0.0
                                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                                txtPorcentajeIVA.Text = porcentajeIVA * 100

                                Dim queryIndirectosSubTotal As String
                                Dim indirectosSubTotal As Double = 0.0
                                queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                                "WHERE p.imodelid = " & imodelid

                                indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                                txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                                txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                                Dim queryPrecioSubTotal As String
                                Dim precioSubTotal As Double = 0.0
                                queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                                "WHERE p.imodelid = " & imodelid

                                precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                                txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                                txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                                Dim precioTotal As Double = 0.0
                                precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                                txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                            Else

                                dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid

                            End If

                        Else

                            'Si cancela el reemplazo de Tarjeta
                            'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid

                        End If

                    Else

                        'Si cancela el reemplazo de Tarjeta
                        'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcardlegacyid

                    End If


                End If

            End If

            'ElseIf e.ColumnIndex = 2 Then

            '    'SCardDescription

            '    If dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

            '        If MsgBox("¿Estás seguro de que quieres eliminar la Tarjeta " & sselectedcardlegacyid & " del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Tarjeta del Modelo") = MsgBoxResult.Yes Then

            '            Dim queriesDelete(3) As String

            '            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
            '            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
            '            queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

            '            executeTransactedSQLCommand(0, queriesDelete)

            '            Dim porcentajeIVA As Double = 0.0
            '            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            '            txtPorcentajeIVA.Text = porcentajeIVA * 100

            '            Dim queryIndirectosSubTotal As String
            '            Dim indirectosSubTotal As Double = 0.0
            '            queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            '            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '            "WHERE p.imodelid = " & imodelid

            '            indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            '            txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            '            txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            '            Dim queryPrecioSubTotal As String
            '            Dim precioSubTotal As Double = 0.0
            '            queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            '            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '            "WHERE p.imodelid = " & imodelid

            '            precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            '            txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            '            txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            '            Dim precioTotal As Double = 0.0
            '            precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            '            txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            '        Else
            '            'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcarddescription
            '        End If

            '    Else

            '        'Si pone un texto, e.g. una descripcion de otro producto

            '        dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Trim.Replace("'", "").Replace("--", "").Replace("@", "")

            '        Dim bf As New BuscaTarjetas
            '        bf.susername = susername
            '        bf.bactive = bactive
            '        bf.bonline = bonline
            '        bf.suserfullname = suserfullname
            '        bf.suseremail = suseremail
            '        bf.susersession = susersession
            '        bf.susermachinename = susermachinename
            '        bf.suserip = suserip

            '        bf.imodelid = imodelid

            '        bf.querystring = dgvResumenDeTarjetas.CurrentCell.EditedFormattedValue

            '        bf.isEdit = False

            '        bf.isBase = False
            '        bf.isModel = False

            '        bf.isHistoric = isHistoric

            'If Me.WindowState = FormWindowState.Maximized Then
            '    bf.WindowState = FormWindowState.Maximized
            'End If

            '        Me.Visible = False
            '        bf.ShowDialog(Me)
            '        Me.Visible = True

            '        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            '            dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & bf.icardid)

            '            If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

            '                MsgBox("Ya tienes esa Tarjeta insertada en este Modelo. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
            '                Exit Sub

            '            End If

            '            If MsgBox("¿Estás seguro de que deseas reemplazar la Tarjeta " & sselectedcardlegacyid & "?", MsgBoxStyle.YesNo, "Confirmación de Reemplazo de Tarjeta") = MsgBoxResult.Yes Then

            '                Dim cantidaddeTarjeta As Double = 1

            '                Try
            '                    cantidaddeTarjeta = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value)
            '                Catch ex As Exception

            '                End Try

            '                Dim fecha As Integer = 0
            '                Dim hora As String = "00:00:00"

            '                fecha = getMySQLDate()
            '                hora = getAppTime()

            '                Dim queriesReplace(6) As String

            '                queriesReplace(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
            '                queriesReplace(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
            '                queriesReplace(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

            '                queriesReplace(3) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
            '                queriesReplace(4) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
            '                queriesReplace(5) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT " & imodelid & ", " & bf.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

            '                executeTransactedSQLCommand(0, queriesReplace)

            '                Dim porcentajeIVA As Double = 0.0
            '                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            '                txtPorcentajeIVA.Text = porcentajeIVA * 100

            '                Dim queryIndirectosSubTotal As String
            '                Dim indirectosSubTotal As Double = 0.0
            '                queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            '                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            '                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '                "WHERE p.imodelid = " & imodelid

            '                indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            '                txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            '                txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            '                Dim queryPrecioSubTotal As String
            '                Dim precioSubTotal As Double = 0.0
            '                queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            '                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            '                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            '                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            '                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            '                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            '                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            '                "WHERE p.imodelid = " & imodelid

            '                precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            '                txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            '                txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            '                Dim precioTotal As Double = 0.0
            '                precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            '                txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")



            '            Else

            '                'Si cancela el reemplazo de Tarjeta
            '                'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcarddescription

            '            End If


            '        Else

            '            'Si cancela el reemplazo de Tarjeta
            '            'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = sselectedcarddescription

            '        End If

            '    End If

        ElseIf e.ColumnIndex = 4 Then

            'DCardQty

            If dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is DBNull.Value Then

                If MsgBox("¿Estás seguro de que quieres eliminar la Tarjeta " & sselectedcardlegacyid & " del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Tarjeta del Modelo") = MsgBoxResult.Yes Then

                    Dim queriesDelete(3) As String

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                    queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

                    executeTransactedSQLCommand(0, queriesDelete)

                    Dim porcentajeIVA As Double = 0.0
                    porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                    txtPorcentajeIVA.Text = porcentajeIVA * 100

                    Dim queryIndirectosSubTotal As String
                    Dim indirectosSubTotal As Double = 0.0
                    queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE p.imodelid = " & imodelid

                    indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                    txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    Dim queryPrecioSubTotal As String
                    Dim precioSubTotal As Double = 0.0
                    queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE p.imodelid = " & imodelid

                    precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                    txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    Dim precioTotal As Double = 0.0
                    precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                    txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Else

                    'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcardqty

                End If


            ElseIf dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0 Then

                If MsgBox("¿Estás seguro de que quieres eliminar la Tarjeta " & sselectedcardlegacyid & " del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Eliminación de Tarjeta del Modelo") = MsgBoxResult.Yes Then

                    Dim queriesDelete(3) As String

                    queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                    queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid
                    queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & iselectedcardid

                    executeTransactedSQLCommand(0, queriesDelete)

                    Dim porcentajeIVA As Double = 0.0
                    porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                    txtPorcentajeIVA.Text = porcentajeIVA * 100

                    Dim queryIndirectosSubTotal As String
                    Dim indirectosSubTotal As Double = 0.0
                    queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE p.imodelid = " & imodelid

                    indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                    txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    Dim queryPrecioSubTotal As String
                    Dim precioSubTotal As Double = 0.0
                    queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE p.imodelid = " & imodelid

                    precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                    txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                    txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                    Dim precioTotal As Double = 0.0
                    precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                    txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


                Else

                    'dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = dselectedcardqty

                End If

            Else

                'Si pone un número

                Dim cantidaddeTarjeta As Double = 1.0

                Try
                    cantidaddeTarjeta = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value)
                Catch ex As Exception
                    cantidaddeTarjeta = dselectedcardqty
                End Try

                executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SET dcardqty = " & dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid & " AND icardid = " & dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)

                Dim porcentajeIVA As Double = 0.0
                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                txtPorcentajeIVA.Text = porcentajeIVA * 100

                Dim queryIndirectosSubTotal As String
                Dim indirectosSubTotal As Double = 0.0
                queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim queryPrecioSubTotal As String
                Dim precioSubTotal As Double = 0.0
                queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim precioTotal As Double = 0.0
                precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            End If

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvResumenDeTarjetas.EditingControlShowing

        If dgvResumenDeTarjetas.CurrentCell.ColumnIndex = 1 Then

            txtLegacyIdDgvResumenDeTarjetas = CType(e.Control, TextBox)
            txtLegacyIdDgvResumenDeTarjetas_OldText = txtLegacyIdDgvResumenDeTarjetas.Text

            'ElseIf dgvResumenDeTarjetas.CurrentCell.ColumnIndex = 2 Then

            '    txtNombreDgvResumenDeTarjetas = CType(e.Control, TextBox)
            '    txtNombreDgvResumenDeTarjetas_OldText = txtNombreDgvResumenDeTarjetas.Text

        ElseIf dgvResumenDeTarjetas.CurrentCell.ColumnIndex = 4 Then

            txtCantidadDgvResumenDeTarjetas = CType(e.Control, TextBox)
            txtCantidadDgvResumenDeTarjetas_OldText = txtCantidadDgvResumenDeTarjetas.Text

        Else

            txtLegacyIdDgvResumenDeTarjetas = Nothing
            txtLegacyIdDgvResumenDeTarjetas_OldText = Nothing
            txtNombreDgvResumenDeTarjetas = Nothing
            txtNombreDgvResumenDeTarjetas_OldText = Nothing
            txtCantidadDgvResumenDeTarjetas = Nothing
            txtCantidadDgvResumenDeTarjetas_OldText = Nothing

        End If

    End Sub


    Private Sub dgvResumenDeTarjetas_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellEndEdit

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim dsCategorias As DataSet
        Dim contadorCategorias As Integer = 0
        Dim resumenDeTarjetas As String = ""
        dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
        contadorCategorias = dsCategorias.Tables(0).Rows.Count

        Dim queriesFill(contadorCategorias) As String

        Dim queriesCreation(2) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)

        Try

            For i As Integer = 0 To contadorCategorias - 1

                Dim queryContadorDeTarjetas As String = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                Dim contadorDeTarjetas As Integer = 0

                contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                If contadorDeTarjetas > 0 Then

                    queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                    "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                    "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                    "'' AS dcardunitaryprice, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                    "ORDER BY scardlegacyid "

                End If

            Next i

            executeTransactedSQLCommand(0, queriesFill)

        Catch ex As Exception

        End Try


        setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

        dgvResumenDeTarjetas.Columns(0).Visible = False

        If viewDgvIndirectCosts = False Then
            dgvResumenDeTarjetas.Columns(5).Visible = False
        End If

        If viewDgvProfits = False Then
            dgvResumenDeTarjetas.Columns(6).Visible = False
        End If

        If viewDgvUnitPrices = False Then
            dgvResumenDeTarjetas.Columns(7).Visible = False
        End If

        If viewDgvAmount = False Then
            dgvResumenDeTarjetas.Columns(8).Visible = False
        End If

        dgvResumenDeTarjetas.Columns(0).ReadOnly = True
        dgvResumenDeTarjetas.Columns(2).ReadOnly = True
        dgvResumenDeTarjetas.Columns(3).ReadOnly = True
        dgvResumenDeTarjetas.Columns(5).ReadOnly = True
        dgvResumenDeTarjetas.Columns(6).ReadOnly = True
        dgvResumenDeTarjetas.Columns(7).ReadOnly = True
        dgvResumenDeTarjetas.Columns(8).ReadOnly = True

        dgvResumenDeTarjetas.Columns(0).Width = 30
        dgvResumenDeTarjetas.Columns(1).Width = 60
        dgvResumenDeTarjetas.Columns(2).Width = 200
        dgvResumenDeTarjetas.Columns(3).Width = 60
        dgvResumenDeTarjetas.Columns(4).Width = 70
        dgvResumenDeTarjetas.Columns(5).Width = 70
        dgvResumenDeTarjetas.Columns(6).Width = 70
        dgvResumenDeTarjetas.Columns(7).Width = 70
        dgvResumenDeTarjetas.Columns(8).Width = 70

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellDoubleClick

        If openCards = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

        If iselectedcardid = 0 Then
            Exit Sub
        End If


        Dim ag As New AgregarTarjeta
        ag.susername = susername
        ag.bactive = bactive
        ag.bonline = bonline
        ag.suserfullname = suserfullname
        ag.suseremail = suseremail
        ag.susersession = susersession
        ag.susermachinename = susermachinename
        ag.suserip = suserip

        ag.IsBase = False
        ag.IsModel = True

        ag.IsEdit = True
        ag.IsHistoric = isHistoric

        ag.iprojectid = imodelid
        ag.icardid = iselectedcardid

        If Me.WindowState = FormWindowState.Maximized Then
            ag.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ag.ShowDialog(Me)
        Me.Visible = True

        If ag.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                        "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                        "'' AS dcardunitaryprice, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                        "ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)


            dgvResumenDeTarjetas.Columns(0).Visible = False

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70


        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResumenDeTarjetas.CellContentDoubleClick

        If openCards = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Try

            If dgvResumenDeTarjetas.CurrentRow.IsNewRow Then
                Exit Sub
            End If

            iselectedcardid = CInt(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(0).Value())
            sselectedcardlegacyid = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(1).Value()
            sselectedcarddescription = dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(2).Value()
            dselectedcardqty = CDbl(dgvResumenDeTarjetas.Rows(e.RowIndex).Cells(4).Value())

        Catch ex As Exception

            iselectedcardid = 0
            sselectedcardlegacyid = ""
            sselectedcostdescription = ""
            dselectedcardqty = 1.0

        End Try

        If iselectedcardid = 0 Then
            Exit Sub
        End If

        Dim ag As New AgregarTarjeta
        ag.susername = susername
        ag.bactive = bactive
        ag.bonline = bonline
        ag.suserfullname = suserfullname
        ag.suseremail = suseremail
        ag.susersession = susersession
        ag.susermachinename = susermachinename
        ag.suserip = suserip

        ag.IsBase = False
        ag.IsModel = True

        ag.IsEdit = True
        ag.IsHistoric = isHistoric

        ag.iprojectid = imodelid
        ag.icardid = iselectedcardid

        If Me.WindowState = FormWindowState.Maximized Then
            ag.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        ag.ShowDialog(Me)
        Me.Visible = True

        If ag.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                        "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                        "'' AS dcardunitaryprice, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                        "ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try

            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70



        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtLegacyIdDgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLegacyIdDgvResumenDeTarjetas.KeyUp

        txtLegacyIdDgvResumenDeTarjetas.Text = txtLegacyIdDgvResumenDeTarjetas.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtLegacyIdDgvResumenDeTarjetas.Text.Contains(arrayForbidden1(fc)) Then
                txtLegacyIdDgvResumenDeTarjetas.Text = txtLegacyIdDgvResumenDeTarjetas.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtNombreDgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreDgvResumenDeTarjetas.KeyUp

        txtNombreDgvResumenDeTarjetas.Text = txtNombreDgvResumenDeTarjetas.Text.Replace("'", "").Replace("--", "").Replace("@", "").Replace("|", "")

        Dim strForbidden1 As String = "|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayForbidden1 As Char() = strForbidden1.ToCharArray

        For fc = 0 To arrayForbidden1.Length - 1

            If txtNombreDgvResumenDeTarjetas.Text.Contains(arrayForbidden1(fc)) Then
                txtNombreDgvResumenDeTarjetas.Text = txtNombreDgvResumenDeTarjetas.Text.Replace(arrayForbidden1(fc), "")
            End If

        Next fc

    End Sub


    Private Sub txtCantidadDgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidadDgvResumenDeTarjetas.KeyUp

        If Not IsNumeric(txtCantidadDgvResumenDeTarjetas.Text) Then

            Dim strForbidden2 As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
            Dim arrayForbidden2 As Char() = strForbidden2.ToCharArray

            For cp = 0 To arrayForbidden2.Length - 1

                If txtCantidadDgvResumenDeTarjetas.Text.Contains(arrayForbidden2(cp)) Then
                    txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Replace(arrayForbidden2(cp), "")
                End If

            Next cp

            If txtCantidadDgvResumenDeTarjetas.Text.Contains(".") Then

                Dim comparaPuntos As Char() = txtCantidadDgvResumenDeTarjetas.Text.ToCharArray
                Dim cuantosPuntos As Integer = 0


                For letra = 0 To comparaPuntos.Length - 1

                    If comparaPuntos(letra) = "." Then
                        cuantosPuntos = cuantosPuntos + 1
                    End If

                Next

                If cuantosPuntos > 1 Then

                    For cantidad = 1 To cuantosPuntos
                        Dim lugar As Integer = txtCantidadDgvResumenDeTarjetas.Text.LastIndexOf(".")
                        Dim longitud As Integer = txtCantidadDgvResumenDeTarjetas.Text.Length

                        If longitud > (lugar + 1) Then
                            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Substring(0, lugar) & txtCantidadDgvResumenDeTarjetas.Text.Substring(lugar + 1)
                            Exit For
                        Else
                            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Substring(0, lugar)
                            Exit For
                        End If
                    Next

                End If

            End If

            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Replace("--", "").Replace("'", "")
            txtCantidadDgvResumenDeTarjetas.Text = txtCantidadDgvResumenDeTarjetas.Text.Trim

        Else
            txtCantidadDgvResumenDeTarjetas_OldText = txtCantidadDgvResumenDeTarjetas.Text
        End If

    End Sub


    Private Sub dgvResumenDeTarjetas_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles dgvResumenDeTarjetas.UserAddedRow

        If addCards = False Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isResumenDGVReady = False

        Dim bf As New BuscaTarjetas
        bf.susername = susername
        bf.bactive = bactive
        bf.bonline = bonline
        bf.suserfullname = suserfullname
        bf.suseremail = suseremail
        bf.susersession = susersession
        bf.susermachinename = susermachinename
        bf.suserip = suserip

        bf.iprojectid = imodelid

        bf.querystring = dgvResumenDeTarjetas.CurrentCell.EditedFormattedValue

        bf.isEdit = False

        bf.isBase = False
        bf.isModel = False

        bf.isHistoric = isHistoric

        If Me.WindowState = FormWindowState.Maximized Then
            bf.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bf.ShowDialog(Me)
        Me.Visible = True

        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim chequeoPrimeraVezQueSeInsertaTarjetaDelModelo As Integer = 0
            chequeoPrimeraVezQueSeInsertaTarjetaDelModelo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid)

            If chequeoPrimeraVezQueSeInsertaTarjetaDelModelo = 0 Then

                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices SELECT " & imodelid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber SELECT " & imodelid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)

            End If

            If bf.wasCreated = False Then


                Dim dsBusquedaTarjetasRepetidas As DataSet

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND scardlegacyid = '" & bf.scardlegacyid & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Modelo. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    dgvResumenDeTarjetas.EndEdit()
                    Exit Sub

                End If


                'Dim cardid As Integer
                'cardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
                'iselectedcardid = cardid

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT " & imodelid & ", " & bf.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

            Dim queryIndirectosSubTotal As String
            Dim indirectosSubTotal As Double = 0.0
            queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim queryPrecioSubTotal As String
            Dim precioSubTotal As Double = 0.0
            queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim precioTotal As Double = 0.0
            precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            dgvResumenDeTarjetas.EndEdit()

            isResumenDGVReady = True

        Else

            dgvResumenDeTarjetas.EndEdit()

            isResumenDGVReady = True

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub dgvResumenDeTarjetas_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvResumenDeTarjetas.KeyUp

        If e.KeyCode = Keys.Delete Then

            If deleteCards = False Then
                Exit Sub
            End If

            If MsgBox("¿Está seguro que deseas eliminar la Tarjeta " & sselectedcardlegacyid & " del Modelo?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Tarjeta") = MsgBoxResult.Yes Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim tmpselectedcardid As Integer = 0
                Try
                    tmpselectedcardid = CInt(dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)
                Catch ex As Exception

                End Try

                Dim queriesDelete(3) As String

                queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & tmpselectedcardid
                queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & tmpselectedcardid
                queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & tmpselectedcardid

                executeTransactedSQLCommand(0, queriesDelete)

                Dim porcentajeIVA As Double = 0.0
                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                txtPorcentajeIVA.Text = porcentajeIVA * 100

                Dim queryIndirectosSubTotal As String
                Dim indirectosSubTotal As Double = 0.0
                queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim queryPrecioSubTotal As String
                Dim precioSubTotal As Double = 0.0
                queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim precioTotal As Double = 0.0
                precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")



                Dim dsCategorias As DataSet
                Dim contadorCategorias As Integer = 0
                Dim resumenDeTarjetas As String = ""
                dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
                contadorCategorias = dsCategorias.Tables(0).Rows.Count

                Dim queriesFill(contadorCategorias) As String

                Dim queriesCreation(2) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                Try

                    For i As Integer = 0 To contadorCategorias - 1

                        Dim queryContadorDeTarjetas As String = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                        Dim contadorDeTarjetas As Integer = 0

                        contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                        If contadorDeTarjetas > 0 Then

                            queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                            "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                            "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                            "'' AS dcardunitaryprice, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                            "ORDER BY scardlegacyid "

                        End If

                    Next i

                    executeTransactedSQLCommand(0, queriesFill)

                Catch ex As Exception

                End Try


                setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

                dgvResumenDeTarjetas.Columns(0).Visible = False

                dgvResumenDeTarjetas.Columns(0).ReadOnly = True
                dgvResumenDeTarjetas.Columns(2).ReadOnly = True
                dgvResumenDeTarjetas.Columns(3).ReadOnly = True
                dgvResumenDeTarjetas.Columns(5).ReadOnly = True
                dgvResumenDeTarjetas.Columns(6).ReadOnly = True
                dgvResumenDeTarjetas.Columns(7).ReadOnly = True
                dgvResumenDeTarjetas.Columns(8).ReadOnly = True

                dgvResumenDeTarjetas.Columns(0).Width = 30
                dgvResumenDeTarjetas.Columns(1).Width = 60
                dgvResumenDeTarjetas.Columns(2).Width = 200
                dgvResumenDeTarjetas.Columns(3).Width = 60
                dgvResumenDeTarjetas.Columns(4).Width = 70
                dgvResumenDeTarjetas.Columns(5).Width = 70
                dgvResumenDeTarjetas.Columns(6).Width = 70
                dgvResumenDeTarjetas.Columns(7).Width = 70
                dgvResumenDeTarjetas.Columns(8).Width = 70


                Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

        End If

    End Sub


    Private Sub txtPorcentajeIVA_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIVA.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIVA.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIVA.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIVA.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIVA.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIVA.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Substring(0, lugar) & txtPorcentajeIVA.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIVA.Select(txtPorcentajeIVA.Text.Length, 0)
        End If

        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Trim

    End Sub


    Private Sub txtPorcentajeIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeIVA.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeIVA.Text = txtPorcentajeIVA.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeIVA.Text.Trim.Trim("--").Trim("'").Trim("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET dmodelIVApercentage = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)
        Catch ex As Exception

        End Try

        Dim queryIndirectosSubTotal As String
        Dim indirectosSubTotal As Double = 0.0
        queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
        "WHERE p.imodelid = " & imodelid

        indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

        txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * (valor / 100)), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Dim queryPrecioSubTotal As String
        Dim precioSubTotal As Double = 0.0
        queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
        "WHERE p.imodelid = " & imodelid

        precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

        txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
        txtIVA.Text = FormatCurrency(precioSubTotal * (valor / 100), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

        Dim precioTotal As Double = 0.0
        precioTotal = precioSubTotal + (precioSubTotal * (valor / 100))

        txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPorcentajeIndirectosDefault_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeIndirectosDefault.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeIndirectosDefault.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeIndirectosDefault.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeIndirectosDefault.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeIndirectosDefault.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeIndirectosDefault.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Substring(0, lugar) & txtPorcentajeIndirectosDefault.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeIndirectosDefault.Select(txtPorcentajeIndirectosDefault.Text.Length, 0)
        End If

        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Trim

    End Sub


    Private Sub txtPorcentajeIndirectosDefault_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeIndirectosDefault.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeIndirectosDefault.Text = txtPorcentajeIndirectosDefault.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeIndirectosDefault.Text.Trim.Trim("--").Trim("'").Trim("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET dmodelindirectpercentagedefault = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)
        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub txtPorcentajeUtilidadDefault_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPorcentajeUtilidadDefault.KeyUp

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<>"
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        Dim resultado As Boolean = False

        For carp = 0 To arrayCaractProhib.Length - 1

            If txtPorcentajeUtilidadDefault.Text.Contains(arrayCaractProhib(carp)) Then
                txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Replace(arrayCaractProhib(carp), "")
                resultado = True
            End If

        Next carp

        If txtPorcentajeUtilidadDefault.Text.Contains(".") Then

            Dim comparaPuntos As Char() = txtPorcentajeUtilidadDefault.Text.ToCharArray
            Dim cuantosPuntos As Integer = 0


            For letra = 0 To comparaPuntos.Length - 1

                If comparaPuntos(letra) = "." Then
                    cuantosPuntos = cuantosPuntos + 1
                End If

            Next

            If cuantosPuntos > 1 Then

                For cantidad = 1 To cuantosPuntos
                    Dim lugar As Integer = txtPorcentajeUtilidadDefault.Text.LastIndexOf(".")
                    Dim longitud As Integer = txtPorcentajeUtilidadDefault.Text.Length

                    If longitud > (lugar + 1) Then
                        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Substring(0, lugar) & txtPorcentajeUtilidadDefault.Text.Substring(lugar + 1)
                        resultado = True
                        Exit For
                    Else
                        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Substring(0, lugar)
                        resultado = True
                        Exit For
                    End If
                Next

            End If

        End If

        If resultado = True Then
            txtPorcentajeUtilidadDefault.Select(txtPorcentajeUtilidadDefault.Text.Length, 0)
        End If

        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Replace("--", "").Replace("'", "")
        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Trim

    End Sub


    Private Sub txtPorcentajeUtilidadDefault_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeUtilidadDefault.TextChanged

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        If isFormReadyForAction = False Then
            Exit Sub
        End If

        Dim strcaracteresprohibidos As String = "abcdefghijklmnopqrstuvwxyzñABCDEFGHIJKLMNOPQRSTUVWXYZÑ|°!#$%&/()=?¡*¨[]_:;,-{}+´¿'¬^`~@\<> "
        Dim arrayCaractProhib As Char() = strcaracteresprohibidos.ToCharArray
        txtPorcentajeUtilidadDefault.Text = txtPorcentajeUtilidadDefault.Text.Trim(arrayCaractProhib)

        Dim valor As Double = 0.0
        Try
            valor = CDbl(txtPorcentajeUtilidadDefault.Text.Trim.Trim("--").Trim("'").Trim("@", ""))
            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET dmodelgainpercentagedefault = " & valor / 100 & ", iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)
        Catch ex As Exception

        End Try

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnNuevaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevaTarjeta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isResumenDGVReady = False

        Dim af As New AgregarTarjeta
        af.susername = susername
        af.bactive = bactive
        af.bonline = bonline
        af.suserfullname = suserfullname
        af.suseremail = suseremail
        af.susersession = susersession
        af.susermachinename = susermachinename
        af.suserip = suserip

        af.iprojectid = imodelid

        af.IsEdit = False

        af.IsBase = False
        af.IsModel = True

        af.IsHistoric = isHistoric

        If Me.WindowState = FormWindowState.Maximized Then
            af.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        af.ShowDialog(Me)
        Me.Visible = True

        If af.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim chequeoPrimeraVezQueSeInsertaTarjetaDelModelo As Integer = 0
            chequeoPrimeraVezQueSeInsertaTarjetaDelModelo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid)

            If chequeoPrimeraVezQueSeInsertaTarjetaDelModelo = 0 Then
                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices SELECT " & imodelid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber SELECT " & imodelid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)
            End If

            If af.wasCreated = False Then


                Dim dsBusquedaTarjetasRepetidas As DataSet

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND scardlegacyid = '" & getSQLQueryAsString(0, "SELECT scardlegacyid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE icardid = " & af.icardid) & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Modelo. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    dgvResumenDeTarjetas.EndEdit()
                    Exit Sub

                End If


                'Dim cardid As Integer
                'cardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
                'iselectedcardid = cardid

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT " & imodelid & ", " & af.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & af.icardid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT " & imodelid & ", " & af.icardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & af.icardid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT " & imodelid & ", " & af.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & af.icardid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

            Dim queryIndirectosSubTotal As String
            Dim indirectosSubTotal As Double = 0.0
            queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim queryPrecioSubTotal As String
            Dim precioSubTotal As Double = 0.0
            queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim precioTotal As Double = 0.0
            precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


        End If

        Dim dsCategorias As DataSet
        Dim contadorCategorias As Integer = 0
        Dim resumenDeTarjetas As String = ""
        dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
        contadorCategorias = dsCategorias.Tables(0).Rows.Count

        Dim queriesFill(contadorCategorias) As String

        Dim queriesCreation(2) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)

        Try

            For i As Integer = 0 To contadorCategorias - 1

                Dim queryContadorDeTarjetas As String = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                Dim contadorDeTarjetas As Integer = 0

                contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                If contadorDeTarjetas > 0 Then

                    queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                    "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                    "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                    "'' AS dcardunitaryprice, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                    "ORDER BY scardlegacyid "

                End If

            Next i

            executeTransactedSQLCommand(0, queriesFill)

        Catch ex As Exception

        End Try


        setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

        dgvResumenDeTarjetas.Columns(0).Visible = False

        dgvResumenDeTarjetas.Columns(0).ReadOnly = True
        dgvResumenDeTarjetas.Columns(2).ReadOnly = True
        dgvResumenDeTarjetas.Columns(3).ReadOnly = True
        dgvResumenDeTarjetas.Columns(5).ReadOnly = True
        dgvResumenDeTarjetas.Columns(6).ReadOnly = True
        dgvResumenDeTarjetas.Columns(7).ReadOnly = True
        dgvResumenDeTarjetas.Columns(8).ReadOnly = True

        dgvResumenDeTarjetas.Columns(0).Width = 30
        dgvResumenDeTarjetas.Columns(1).Width = 60
        dgvResumenDeTarjetas.Columns(2).Width = 200
        dgvResumenDeTarjetas.Columns(3).Width = 60
        dgvResumenDeTarjetas.Columns(4).Width = 70
        dgvResumenDeTarjetas.Columns(5).Width = 70
        dgvResumenDeTarjetas.Columns(6).Width = 70
        dgvResumenDeTarjetas.Columns(7).Width = 70
        dgvResumenDeTarjetas.Columns(8).Width = 70

        isResumenDGVReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnInsertarTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertarTarjeta.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        isResumenDGVReady = False

        Dim bf As New BuscaTarjetas
        bf.susername = susername
        bf.bactive = bactive
        bf.bonline = bonline
        bf.suserfullname = suserfullname
        bf.suseremail = suseremail
        bf.susersession = susersession
        bf.susermachinename = susermachinename
        bf.suserip = suserip

        bf.iprojectid = imodelid

        bf.isEdit = False

        bf.isBase = False
        bf.isModel = False

        bf.isHistoric = isHistoric

        If Me.WindowState = FormWindowState.Maximized Then
            bf.WindowState = FormWindowState.Maximized
        End If

        Me.Visible = False
        bf.ShowDialog(Me)
        Me.Visible = True

        If bf.DialogResult = Windows.Forms.DialogResult.OK Then

            Dim baseid As Integer = 0
            baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

            If baseid = 0 Then
                baseid = 99999
            End If

            Dim fecha As Integer = 0
            Dim hora As String = "00:00:00"

            fecha = getMySQLDate()
            hora = getAppTime()

            Dim chequeoPrimeraVezQueSeInsertaTarjetaDelModelo As Integer = 0
            chequeoPrimeraVezQueSeInsertaTarjetaDelModelo = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices WHERE imodelid = " & imodelid)

            If chequeoPrimeraVezQueSeInsertaTarjetaDelModelo = 0 Then
                Dim queriesPrimeraVez(2) As String
                queriesPrimeraVez(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices SELECT " & imodelid & ", iinputid, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice, " & fecha & ", '" & hora & "', '" & susername & "' FROM (SELECT * FROM baseprices WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid"
                queriesPrimeraVez(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber SELECT " & imodelid & ", bt.iinputid, bt.dinputtimberespesor, bt.dinputtimberancho, bt.dinputtimberlargo, bt.dinputtimberpreciopiecubico, " & fecha & ", '" & hora & "', '" & susername & "' FROM basetimber bt where ibaseid = " & baseid
                executeTransactedSQLCommand(0, queriesPrimeraVez)
            End If

            If bf.wasCreated = False Then


                Dim dsBusquedaTarjetasRepetidas As DataSet

                dsBusquedaTarjetasRepetidas = getSQLQueryAsDataset(0, "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND scardlegacyid = '" & bf.scardlegacyid & "'")

                If dsBusquedaTarjetasRepetidas.Tables(0).Rows.Count > 0 Then

                    MsgBox("Ya tienes esa Tarjeta insertada en este Modelo. ¿Podrías buscarla en la lista de Resumen de Tarjetas y cambiar la cantidad si así lo deseas?", MsgBoxStyle.OkOnly, "Tarjeta Repetida")
                    dgvResumenDeTarjetas.EndEdit()
                    Exit Sub

                End If


                'Dim cardid As Integer
                'cardid = getSQLQueryAsInteger(0, "SELECT IF(MAX(icardid) + 1 IS NULL, 1, MAX(icardid) + 1) AS icardid FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")
                'iselectedcardid = cardid

                Dim queriesInsert(3) As String

                queriesInsert(0) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, scardinputunit, dcardinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(1) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs SELECT " & imodelid & ", " & bf.icardid & ", iinputid, icompoundinputid, scompoundinputunit, dcompoundinputqty, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecardcompoundinputs WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid
                queriesInsert(2) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards SELECT " & imodelid & ", " & bf.icardid & ", scardlegacycategoryid, scardlegacyid, scarddescription, scardunit, dcardqty, dcardindirectcostspercentage, dcardgainpercentage, " & fecha & ", '" & hora & "', '" & susername & "' FROM basecards WHERE ibaseid = " & baseid & " AND icardid = " & bf.icardid

                executeTransactedSQLCommand(0, queriesInsert)

            End If

            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

            Dim queryIndirectosSubTotal As String
            Dim indirectosSubTotal As Double = 0.0
            queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim queryPrecioSubTotal As String
            Dim precioSubTotal As Double = 0.0
            queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim precioTotal As Double = 0.0
            precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


        End If

        Dim dsCategorias As DataSet
        Dim contadorCategorias As Integer = 0
        Dim resumenDeTarjetas As String = ""
        dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
        contadorCategorias = dsCategorias.Tables(0).Rows.Count

        Dim queriesFill(contadorCategorias) As String

        Dim queriesCreation(2) As String

        queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
        queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

        executeTransactedSQLCommand(0, queriesCreation)

        Try

            For i As Integer = 0 To contadorCategorias - 1

                Dim queryContadorDeTarjetas As String = "" & _
                "SELECT COUNT(*) " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                Dim contadorDeTarjetas As Integer = 0

                contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                If contadorDeTarjetas > 0 Then

                    queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                    "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                    "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                    "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                    "'' AS dcardunitaryprice, " & _
                    "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                    "UNION " & _
                    "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                    "ORDER BY scardlegacyid "

                End If

            Next i

            executeTransactedSQLCommand(0, queriesFill)

        Catch ex As Exception

        End Try


        setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

        dgvResumenDeTarjetas.Columns(0).Visible = False

        dgvResumenDeTarjetas.Columns(0).ReadOnly = True
        dgvResumenDeTarjetas.Columns(2).ReadOnly = True
        dgvResumenDeTarjetas.Columns(3).ReadOnly = True
        dgvResumenDeTarjetas.Columns(5).ReadOnly = True
        dgvResumenDeTarjetas.Columns(6).ReadOnly = True
        dgvResumenDeTarjetas.Columns(7).ReadOnly = True
        dgvResumenDeTarjetas.Columns(8).ReadOnly = True

        dgvResumenDeTarjetas.Columns(0).Width = 30
        dgvResumenDeTarjetas.Columns(1).Width = 60
        dgvResumenDeTarjetas.Columns(2).Width = 200
        dgvResumenDeTarjetas.Columns(3).Width = 60
        dgvResumenDeTarjetas.Columns(4).Width = 70
        dgvResumenDeTarjetas.Columns(5).Width = 70
        dgvResumenDeTarjetas.Columns(6).Width = 70
        dgvResumenDeTarjetas.Columns(7).Width = 70
        dgvResumenDeTarjetas.Columns(8).Width = 70

        isResumenDGVReady = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnEliminarTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarTarjeta.Click

        If MsgBox("¿Está seguro que deseas eliminar la Tarjeta " & sselectedcardlegacyid & " del Modelo?", MsgBoxStyle.YesNo, "Confirmar Eliminación de Tarjeta") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            Dim tmpselectedcardid As Integer = 0
            Try
                tmpselectedcardid = CInt(dgvResumenDeTarjetas.CurrentRow.Cells(0).Value)
            Catch ex As Exception

            End Try

            Dim queriesDelete(3) As String

            queriesDelete(0) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards WHERE imodelid = " & imodelid & " AND icardid = " & tmpselectedcardid
            queriesDelete(1) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs WHERE imodelid = " & imodelid & " AND icardid = " & tmpselectedcardid
            queriesDelete(2) = "DELETE FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs WHERE imodelid = " & imodelid & " AND icardid = " & tmpselectedcardid

            executeTransactedSQLCommand(0, queriesDelete)

            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

            Dim queryIndirectosSubTotal As String
            Dim indirectosSubTotal As Double = 0.0
            queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim queryPrecioSubTotal As String
            Dim precioSubTotal As Double = 0.0
            queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim precioTotal As Double = 0.0
            precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")



            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                        "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                        "'' AS dcardunitaryprice, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                        "ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70

            isResumenDGVReady = True

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Function validaResumenDeTarjetas(ByVal silent As Boolean) As Boolean

        If isFormReadyForAction = False Then
            Exit Function
        End If


        'Dim porcentajeIVA As Double = 0.0

        'Try
        '    porcentajeIVA = CDbl(txtPorcentajeIVA.Text)

        'Catch ex As Exception

        'End Try

        'If porcentajeIVA = 0 Then

        '    tcModelo.SelectedTab = tpResumenTarjetas
        '    txtPorcentajeIVA.Select()
        '    txtPorcentajeIVA.Focus()

        '    If silent = False Then
        '        MsgBox("¿Podrías poner el porcentaje de IVA aplicable a las Tarjetas del Modelo?", MsgBoxStyle.OkOnly, "Dato Faltante")
        '    End If

        '    alertaMostrada = True
        '    Return False

        'End If

        If isEdit = False Then

            If paso4 = False Then

                Return False

            End If

        End If

        Return True


    End Function


    Private Sub btnActualizarPrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizarPrecios.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        paso4 = True

        If validaResumenDeTarjetas(False) = False Then
            paso4 = False
            Exit Sub
        End If

        Dim queryPreciosYaCambiaron As String
        Dim preciosCambiados As Double = 0.0
        Dim baseid As Integer = 0

        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        queryPreciosYaCambiaron = "SELECT SUM(bp.dinputfinalprice <> pp.dinputfinalprice) AS diferente FROM inputs i JOIN (SELECT * FROM (SELECT * FROM baseprices bp WHERE ibaseid = " & baseid & " ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid) bp ON bp.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices pp WHERE imodelid = " & imodelid & " ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid) pp ON pp.iinputid = i.iinputid"
        preciosCambiados = getSQLQueryAsDouble(0, queryPreciosYaCambiaron)

        If preciosCambiados > 0 Then

            Dim micpd As New BuscaInsumosConPrecioDiferente
            micpd.susername = susername
            micpd.bactive = bactive
            micpd.bonline = bonline
            micpd.suserfullname = suserfullname
            micpd.suseremail = suseremail
            micpd.susersession = susersession
            micpd.susermachinename = susermachinename
            micpd.suserip = suserip

            'micpd.isEdit = True
            micpd.IsBase = False
            micpd.IsModel = True
            micpd.IsHistoric = isHistoric

            micpd.iprojectid = imodelid

            If Me.WindowState = FormWindowState.Maximized Then
                micpd.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            micpd.ShowDialog(Me)
            Me.Visible = True

            'Acá van los querys del tab de Resumen de Tarjetas (del Load)
            Dim dsCategorias As DataSet
            Dim contadorCategorias As Integer = 0
            Dim resumenDeTarjetas As String = ""
            dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
            contadorCategorias = dsCategorias.Tables(0).Rows.Count

            Dim queriesFill(contadorCategorias) As String

            Dim queriesCreation(2) As String

            queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
            queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            executeTransactedSQLCommand(0, queriesCreation)

            Try

                For i As Integer = 0 To contadorCategorias - 1

                    Dim queryContadorDeTarjetas As String = "" & _
                    "SELECT COUNT(*) " & _
                    "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                    "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                    "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                    "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                    "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                    Dim contadorDeTarjetas As Integer = 0

                    contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                    If contadorDeTarjetas > 0 Then

                        queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                        "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                        "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                        "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                        "'' AS dcardunitaryprice, " & _
                        "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                        "UNION " & _
                        "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                        "ORDER BY scardlegacyid "

                    End If

                Next i

                executeTransactedSQLCommand(0, queriesFill)

            Catch ex As Exception

            End Try


            setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

            dgvResumenDeTarjetas.Columns(0).Visible = False

            dgvResumenDeTarjetas.Columns(0).ReadOnly = True
            dgvResumenDeTarjetas.Columns(2).ReadOnly = True
            dgvResumenDeTarjetas.Columns(3).ReadOnly = True
            dgvResumenDeTarjetas.Columns(5).ReadOnly = True
            dgvResumenDeTarjetas.Columns(6).ReadOnly = True
            dgvResumenDeTarjetas.Columns(7).ReadOnly = True
            dgvResumenDeTarjetas.Columns(8).ReadOnly = True

            dgvResumenDeTarjetas.Columns(0).Width = 30
            dgvResumenDeTarjetas.Columns(1).Width = 60
            dgvResumenDeTarjetas.Columns(2).Width = 200
            dgvResumenDeTarjetas.Columns(3).Width = 60
            dgvResumenDeTarjetas.Columns(4).Width = 70
            dgvResumenDeTarjetas.Columns(5).Width = 70
            dgvResumenDeTarjetas.Columns(6).Width = 70
            dgvResumenDeTarjetas.Columns(7).Width = 70
            dgvResumenDeTarjetas.Columns(8).Width = 70


            Dim porcentajeIVA As Double = 0.0
            porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
            txtPorcentajeIVA.Text = porcentajeIVA * 100

            Dim queryIndirectosSubTotal As String
            Dim indirectosSubTotal As Double = 0.0
            queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

            txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim queryPrecioSubTotal As String
            Dim precioSubTotal As Double = 0.0
            queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
            "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
            "WHERE p.imodelid = " & imodelid

            precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

            txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
            txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

            Dim precioTotal As Double = 0.0
            precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

            txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


        Else

            Cursor.Current = System.Windows.Forms.Cursors.Default
            MsgBox("No hay ningún precio desactualizado", MsgBoxStyle.OkOnly, "Precios Actualizados")

        End If

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub


    Private Sub btnActualizarUtilidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizarUtilidad.Click

        If MsgBox("¿Está seguro de que deseas actualizar los Porcentajes de Indirectos y Utilidades para TODAS las Tarjetas del Modelo?", MsgBoxStyle.YesNo, "Confirmación de Actualización de Utilidades e Indirectos") = MsgBoxResult.Yes Then

            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            paso4 = True

            If validaResumenDeTarjetas(False) = False Then
                paso4 = False
                Exit Sub
            End If

            Dim ciyu As New AgregarProyectoCambiaIndirectosYUtilidad
            ciyu.susername = susername
            ciyu.bactive = bactive
            ciyu.bonline = bonline
            ciyu.suserfullname = suserfullname
            ciyu.suseremail = suseremail
            ciyu.susersession = susersession
            ciyu.susermachinename = susermachinename
            ciyu.suserip = suserip

            ciyu.isBase = False
            ciyu.IsModel = True

            ciyu.iprojectid = imodelid

            Me.Visible = False
            ciyu.ShowDialog(Me)
            Me.Visible = True

            If ciyu.DialogResult = Windows.Forms.DialogResult.OK Then

                'Acá van los querys del tab de Resumen de Tarjetas (del Load)
                Dim dsCategorias As DataSet
                Dim contadorCategorias As Integer = 0
                Dim resumenDeTarjetas As String = ""
                dsCategorias = getSQLQueryAsDataset(0, "SELECT scardlegacycategoryid, scardlegacycategorydescription FROM cardlegacycategories")
                contadorCategorias = dsCategorias.Tables(0).Rows.Count

                Dim queriesFill(contadorCategorias) As String

                Dim queriesCreation(2) As String

                queriesCreation(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux"
                queriesCreation(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " (   scardid varchar(11) COLLATE latin1_spanish_ci NOT NULL,   scardlegacyid varchar(510) CHARACTER SET latin1 NOT NULL,   scarddescription VARCHAR(1000) CHARACTER SET latin1 NOT NULL,   scardunit varchar(49) CHARACTER SET latin1 NOT NULL,   smodelcardqty varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardindirectcosts varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardgain varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardunitaryprice varchar(20) COLLATE latin1_spanish_ci NOT NULL,   scardamount varchar(20) COLLATE latin1_spanish_ci NOT NULL ) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

                executeTransactedSQLCommand(0, queriesCreation)

                Try

                    For i As Integer = 0 To contadorCategorias - 1

                        Dim queryContadorDeTarjetas As String = "" & _
                        "SELECT COUNT(*) " & _
                        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                        "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' "

                        Dim contadorDeTarjetas As Integer = 0

                        contadorDeTarjetas = getSQLQueryAsInteger(0, queryContadorDeTarjetas)

                        If contadorDeTarjetas > 0 Then

                            queriesFill(i) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux" & " " & _
                            "SELECT '' AS 'icardid', CONCAT(ptflc.scardlegacycategoryid, ' ', ptflc.scardlegacycategorydescription) AS 'scardlegacyid', " & _
                            "'' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' FROM cardlegacycategories ptflc WHERE ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT ptf.icardid, ptf.scardlegacyid, ptf.scarddescription, ptf.scardunit, FORMAT(ptf.dcardqty, 3) AS dcardqty, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty)), 2) AS dcardindirectcosts, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardgain, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)), 2) AS dcardunitaryprice, " & _
                            "FORMAT(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty)), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT '' AS icardid, CONCAT('SUBTOTAL CATEGORIA ', ptf.scardlegacycategoryid) AS scardlegacyid, '' AS scarddescription, '' AS scardunit, '' AS dcardqty, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage)*(ptf.dcardqty))), 2) AS dcardindirectcosts, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardgain, " & _
                            "'' AS dcardunitaryprice, " & _
                            "FORMAT(SUM(((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage)*(ptf.dcardqty))), 2) AS dcardsprice " & _
                            "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                            "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                            "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                            "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                            "WHERE ptf.imodelid = " & imodelid & " AND ptflc.scardlegacycategoryid = '" & dsCategorias.Tables(0).Rows(i).Item("scardlegacycategoryid") & "' " & _
                            "UNION " & _
                            "SELECT '' AS 'icardid', '' AS 'scardlegacyid', '' AS 'scarddescription', '' AS 'scardunit', '' AS 'dcardqty', '' AS 'dcardindirectcosts', '' AS 'dcardgain', '' AS 'dcardunitaryprice', '' AS 'dcardsprice' " & _
                            "ORDER BY scardlegacyid "

                        End If

                    Next i

                    executeTransactedSQLCommand(0, queriesFill)

                Catch ex As Exception

                End Try


                setDataGridView(dgvResumenDeTarjetas, "SELECT scardid, scardlegacyid AS 'ID', scarddescription AS 'Descripcion Tarjeta', scardunit AS 'Unidad de Medida', smodelcardqty AS 'Cantidad', scardindirectcosts AS 'Indirectos', scardgain AS 'Utilidad', scardunitaryprice AS 'P.U.', scardamount AS 'Importe' FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux", False)

                dgvResumenDeTarjetas.Columns(0).Visible = False

                dgvResumenDeTarjetas.Columns(0).ReadOnly = True
                dgvResumenDeTarjetas.Columns(2).ReadOnly = True
                dgvResumenDeTarjetas.Columns(3).ReadOnly = True
                dgvResumenDeTarjetas.Columns(5).ReadOnly = True
                dgvResumenDeTarjetas.Columns(6).ReadOnly = True
                dgvResumenDeTarjetas.Columns(7).ReadOnly = True
                dgvResumenDeTarjetas.Columns(8).ReadOnly = True

                dgvResumenDeTarjetas.Columns(0).Width = 30
                dgvResumenDeTarjetas.Columns(1).Width = 60
                dgvResumenDeTarjetas.Columns(2).Width = 200
                dgvResumenDeTarjetas.Columns(3).Width = 60
                dgvResumenDeTarjetas.Columns(4).Width = 70
                dgvResumenDeTarjetas.Columns(5).Width = 70
                dgvResumenDeTarjetas.Columns(6).Width = 70
                dgvResumenDeTarjetas.Columns(7).Width = 70
                dgvResumenDeTarjetas.Columns(8).Width = 70


                Dim porcentajeIVA As Double = 0.0
                porcentajeIVA = getSQLQueryAsDouble(0, "SELECT dmodelIVApercentage FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " WHERE imodelid = " & imodelid)
                txtPorcentajeIVA.Text = porcentajeIVA * 100

                Dim queryIndirectosSubTotal As String
                Dim indirectosSubTotal As Double = 0.0
                queryIndirectosSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(ptf.dcardindirectcostspercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                indirectosSubTotal = getSQLQueryAsDouble(0, queryIndirectosSubTotal)

                txtIndirectosSubtotal.Text = FormatCurrency(indirectosSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                txtIndirectosTotal.Text = FormatCurrency(indirectosSubTotal + (indirectosSubTotal * porcentajeIVA), 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim queryPrecioSubTotal As String
                Dim precioSubTotal As Double = 0.0
                queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
                "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
                "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
                "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
                "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
                "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
                "WHERE p.imodelid = " & imodelid

                precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

                txtPrecioProyectadoSubTotal.Text = FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")
                txtIVA.Text = FormatCurrency(precioSubTotal * porcentajeIVA, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")

                Dim precioTotal As Double = 0.0
                precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

                txtPrecioProyectadoTotal.Text = FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True).Replace("$", "")


            End If

            Cursor.Current = System.Windows.Forms.Cursors.Default

        End If

    End Sub


    Private Sub btnGenerarArchivoExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarArchivoExcel.Click

        If (txtPrecioProyectadoSubTotal.Text = "" Or txtPrecioProyectadoSubTotal.Text = "0.0") Then
            MsgBox("Aún NO has terminado de definir este Modelo. ¿Podrías completarlo?", MsgBoxStyle.OkOnly, "Modelo incompleto")
            Exit Sub
        End If

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux



            msSaveFileDialog.FileName = "Presupuesto de Modelo " & txtNombreModelo.Text & " " & fecha

            Try

                If Not My.Computer.FileSystem.DirectoryExists(txtRuta.Text) Then
                    My.Computer.FileSystem.CreateDirectory(txtRuta.Text)
                End If

                msSaveFileDialog.InitialDirectory = txtRuta.Text

            Catch ex As Exception

            End Try

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportPresupuestoToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Presupuesto de Modelo Exportado Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar el presupuesto. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar el presupuesto")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportPresupuestoToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the model header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""63""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""494.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""65.25"" ss:Span=""5""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell></Cell>")
            fs.WriteLine("  <Cell ss:MergeAcross=""6"" ss:StyleID=""1""><Data ss:Type=""String"">PRESUPUESTO DE CONSTRUCCION</Data></Cell>")
            fs.WriteLine("   </Row>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", lblNombreDelModelo.Text.Trim))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", txtNombreModelo.Text.Trim))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next

            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""22.5"">")

            For Each col As DataGridViewColumn In dgvResumenDeTarjetas.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next

            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvResumenDeTarjetas.Rows

                If dgvResumenDeTarjetas.AllowUserToAddRows = True And row.Index = dgvResumenDeTarjetas.Rows.Count - 1 Then
                    Exit For
                End If

                fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))

                For Each col As DataGridViewColumn In dgvResumenDeTarjetas.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            If row.Cells(0).Value.ToString.StartsWith("SUBTOTAL") = True Then
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""12""></Cell>"))
                            Else
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))
                            End If

                        Else

                            If row.Cells(0).Value.ToString.StartsWith("SUBTOTAL") = True Then
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""12""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                            Else
                                fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))
                            End If

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

            'Write the separation between results and totals
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            'Write the totals 
            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""13""><Data ss:Type=""String"">{0}</Data></Cell>", lblCostoProyectadoTotal.Text))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""14""><Data ss:Type=""String"">{0}</Data></Cell>", txtPrecioProyectadoSubTotal.Text))
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""13""><Data ss:Type=""String"">{0}</Data></Cell>", "IVA"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""14""><Data ss:Type=""String"">{0}</Data></Cell>", txtIVA.Text))
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("      <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""13""><Data ss:Type=""String"">{0}</Data></Cell>", lblPrecioProyectadoTotal.Text))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""14""><Data ss:Type=""String"">{0}</Data></Cell>", txtPrecioProyectadoTotal.Text))
            fs.WriteLine("    </Row>")


            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub btnCostoHoy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCostoHoy.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim precioSubTotal As Double = 0.0
        Dim porcentajeIVA As Double = 0.0
        Dim iva As Double = 0.0
        Dim precioTotal As Double = 0.0

        Try
            porcentajeIVA = CDbl(txtPorcentajeIVA.Text) / 100
        Catch ex As Exception

        End Try

        Dim queryPrecioSubTotal As String

        'AAAGGGUUAAAAS!! ESTE QUERY PARECE EL MISMO QUE EL DE TODO ESTE "FORM" PERO LA FORMA DE CALCULAR LOS PRECIOS ES DISTINTA...

        queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT " & imodelid & " AS imodelid, iinputid, iupdatedate, supdatetime, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON ptfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT " & imodelid & " AS imodelid, iinputid, iupdatedate, supdatetime, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON ptfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*bp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT " & imodelid & " AS imodelid, iinputid, iupdatedate, supdatetime, dinputpricewithoutIVA, dinputprotectionpercentage, dinputfinalprice FROM baseprices ORDER BY iupdatedate DESC, supdatetime DESC) bp GROUP BY iinputid, imodelid) bp ON ptfi.imodelid = bp.imodelid AND i.iinputid = bp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
        "WHERE p.imodelid = " & imodelid

        precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

        iva = precioSubTotal * porcentajeIVA

        precioTotal = precioSubTotal + (precioSubTotal * porcentajeIVA)

        Cursor.Current = System.Windows.Forms.Cursors.Default

        MsgBox("Si este Modelo se realizara con los precios de hoy:" & Chr(13) & "Su Costo Total Previsto sería: " & FormatCurrency(precioSubTotal, 2, TriState.True, TriState.False, TriState.True) & Chr(13) & "Su IVA (" & porcentajeIVA * 100 & " %) sería: " & FormatCurrency(iva, 2, TriState.True, TriState.False, TriState.True) & Chr(13) & "Su Precio Total Previsto sería: " & FormatCurrency(precioTotal, 2, TriState.True, TriState.False, TriState.True), MsgBoxStyle.OkOnly, "Si se hiciera hoy... ")

    End Sub


    Private Sub btnGenerarExplosion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarExplosion.Click

        Dim queryPrecioSubTotal As String
        Dim precioSubTotal As Double = 0.0
        queryPrecioSubTotal = "SELECT SUM(ptf.dcardqty*((IF(costoMAT.costo IS NULL, 0, costoMAT.costo) + IF(costoMO.costo IS NULL, 0, costoMO.costo) + IF(costoEQ.costo IS NULL, 0, costoEQ.costo))*(1+ptf.dcardindirectcostspercentage)*(1+ptf.dcardgainpercentage))) AS dcardamount " & _
        "FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf " & _
        "JOIN cardlegacycategories ptflc ON ptf.scardlegacycategoryid = ptflc.scardlegacycategoryid " & _
        "JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " p ON p.imodelid = ptf.imodelid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, (costoMO.costo*ptfi.dcardinputqty) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid JOIN (SELECT ptfi.imodelid, ptfi.icardid AS icardid, 0 AS iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid ) AS costoMO ON ptfi.iinputid = costoMO.iinputid AND ptfi.icardid = costoMO.icardid GROUP BY ptfi.icardid, ptfi.imodelid) AS costoEQ ON ptf.imodelid = costoEQ.imodelid AND ptf.icardid = costoEQ.icardid " & _
        "JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, SUM(ptfi.dcardinputqty*pp.dinputfinalprice) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MANO DE OBRA' GROUP BY ptf.icardid, ptfi.imodelid) AS costoMO ON ptf.imodelid = costoMO.imodelid AND ptf.icardid = costoMO.icardid " & _
        "LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, IF(SUM(ptfi.dcardinputqty*pp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*pp.dinputfinalprice))+IF(SUM(ptfi.dcardinputqty*cipp.dinputfinalprice) IS NULL, 0, SUM(ptfi.dcardinputqty*cipp.dinputfinalprice)) AS costo FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards ptf LEFT JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi ON ptf.imodelid = ptfi.imodelid AND ptf.icardid = ptfi.icardid LEFT JOIN inputs i ON ptfi.iinputid = i.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) pp GROUP BY iinputid, imodelid) pp ON ptfi.imodelid = pp.imodelid AND i.iinputid = pp.iinputid LEFT JOIN (SELECT ptfi.imodelid, ptfi.icardid, ptfi.iinputid, cipp.iupdatedate, cipp.supdatetime, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputpricewithoutIVA, 0.00000 AS dinputprotectionpercentage, SUM(ptfci.dcompoundinputqty*cipp.dinputfinalprice) AS dinputfinalprice FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs ptfi JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs ptfci ON ptfci.imodelid = ptfi.imodelid AND ptfci.icardid = ptfi.icardid AND ptfci.iinputid = ptfi.iinputid LEFT JOIN (SELECT * FROM (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices ORDER BY iupdatedate DESC, supdatetime DESC) cipp GROUP BY iinputid, imodelid) cipp ON cipp.imodelid = ptfci.imodelid AND cipp.iinputid = ptfci.icompoundinputid GROUP BY ptfci.imodelid, ptfci.icardid, ptfi.iinputid) cipp ON ptfi.imodelid = cipp.imodelid AND ptfi.icardid = cipp.icardid AND i.iinputid = cipp.iinputid JOIN inputtypes it ON i.iinputid = it.iinputid WHERE it.sinputtypedescription = 'MATERIALES' GROUP BY ptfi.imodelid, ptfi.icardid ORDER BY ptfi.imodelid, ptfi.icardid, ptfi.iinputid) AS costoMAT ON ptf.imodelid = costoMAT.imodelid AND ptf.icardid = costoMAT.icardid " & _
        "WHERE p.imodelid = " & imodelid

        precioSubTotal = getSQLQueryAsDouble(0, queryPrecioSubTotal)

        If (precioSubTotal = 0.0) Then
            MsgBox("Aún NO has terminado de definir este Modelo. ¿Podrías completarlo?", MsgBoxStyle.OkOnly, "Modelo incompleto")
            Exit Sub
        End If

        Try

            Dim resultado As Boolean = False

            Dim fecha As String = ""
            Dim dayAux As String = ""
            Dim monthAux As String = ""
            Dim hourAux As String = ""
            Dim minuteAux As String = ""
            Dim secondAux As String = ""

            If DateTime.Today.Month.ToString.Length < 2 Then
                monthAux = "0" & DateTime.Today.Month
            Else
                monthAux = DateTime.Today.Month
            End If

            If DateTime.Today.Day.ToString.Length < 2 Then
                dayAux = "0" & DateTime.Today.Day
            Else
                dayAux = DateTime.Today.Day
            End If

            If Date.Now.Hour.ToString.Length < 2 Then
                hourAux = "0" & DateTime.Now.Hour
            Else
                hourAux = DateTime.Now.Hour
            End If

            If Date.Now.Minute.ToString.Length < 2 Then
                minuteAux = "0" & DateTime.Now.Minute
            Else
                minuteAux = DateTime.Now.Minute
            End If

            If Date.Now.Second.ToString.Length < 2 Then
                secondAux = "0" & DateTime.Now.Second
            Else
                secondAux = DateTime.Now.Second
            End If

            fecha = DateTime.Today.Year & monthAux & dayAux & hourAux & minuteAux & secondAux



            msSaveFileDialog.FileName = "Explosión de Insumos " & txtNombreModelo.Text & " " & fecha

            Try

                If Not My.Computer.FileSystem.DirectoryExists(txtRuta.Text) Then
                    My.Computer.FileSystem.CreateDirectory(txtRuta.Text)
                End If

                msSaveFileDialog.InitialDirectory = txtRuta.Text

            Catch ex As Exception

            End Try

            msSaveFileDialog.Filter = "Excel Files (*.xls) |*.xls"
            msSaveFileDialog.DefaultExt = "*.xls"

            If msSaveFileDialog.ShowDialog() = DialogResult.OK Then

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                resultado = ExportExplosionToExcel(msSaveFileDialog.FileName)

                Cursor.Current = System.Windows.Forms.Cursors.Default

                If resultado = True Then
                    MsgBox("Explosión de Insumos Exportada Correctamente!" & Chr(13) & "El archivo se abrirá al dar click en OK", MsgBoxStyle.OkOnly, "Exportación Completada")
                    System.Diagnostics.Process.Start(msSaveFileDialog.FileName)
                Else
                    MsgBox("No se ha podido exportar la Explosión de Insumos. Intente nuevamente.", MsgBoxStyle.OkOnly, "Error al exportar la Explosión de Insumos")
                End If

            End If

        Catch ex As Exception

        End Try

    End Sub


    Private Function ExportExplosionToExcel(ByVal pth As String) As Boolean

        Try

            Dim fs As New IO.StreamWriter(pth, False)
            fs.WriteLine("<?xml version=""1.0""?>")
            fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
            fs.WriteLine("<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"" xmlns:html=""http://www.w3.org/TR/REC-html40"">")

            ' Create the styles for the worksheet
            fs.WriteLine("  <Styles>")

            ' Style for the document name
            fs.WriteLine("  <Style ss:ID=""1"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""12"" ss:Bold=""1""></Font>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("   <Protection></Protection>")
            fs.WriteLine("  </Style>")

            ' Style for the column headers
            fs.WriteLine("   <Style ss:ID=""2"">")
            fs.WriteLine("   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""></Alignment>")
            fs.WriteLine("   <Borders>")
            fs.WriteLine("  <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("  <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""></Border>")
            fs.WriteLine("   </Borders>")
            fs.WriteLine("   <Font ss:FontName=""Arial"" ss:Size=""9"" ss:Bold=""1""></Font>")
            fs.WriteLine("  </Style>")


            ' Style for the left sided info
            fs.WriteLine("    <Style ss:ID=""9"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the right sided info
            fs.WriteLine("    <Style ss:ID=""10"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the middle sided info
            fs.WriteLine("    <Style ss:ID=""11"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""10""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("    </Style>")

            ' Style for the SUBtotals labels
            fs.WriteLine("    <Style ss:ID=""12"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FFCC00"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals labels
            fs.WriteLine("    <Style ss:ID=""13"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat></NumberFormat>")
            fs.WriteLine("    </Style>")

            ' Style for the totals
            fs.WriteLine("    <Style ss:ID=""14"">")
            fs.WriteLine("      <Font ss:FontName=""Arial"" ss:Size=""9""></Font>")
            fs.WriteLine("   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Center""></Alignment>")
            fs.WriteLine("   <Interior ss:Color=""#FF9900"" ss:Pattern=""Solid""></Interior>")
            fs.WriteLine("   <NumberFormat ss:Format=""Standard""></NumberFormat>")
            fs.WriteLine("    </Style>")

            fs.WriteLine("  </Styles>")

            ' Write the worksheet contents
            fs.WriteLine("<Worksheet ss:Name=""Hoja1"">")
            fs.WriteLine("  <Table ss:DefaultColumnWidth=""60"" ss:DefaultRowHeight=""15"">")

            'Write the model header info
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""494.25""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""63""/>")
            fs.WriteLine("   <Column ss:AutoFitWidth=""0"" ss:Width=""65.25"" ss:Span=""5""/>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine("  <Cell ss:MergeAcross=""7"" ss:StyleID=""1""><Data ss:Type=""String"">EXPLOSION DE INSUMOS</Data></Cell>")
            fs.WriteLine("   </Row>")

            fs.WriteLine("   <Row ss:AutoFitHeight=""0"">")
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", lblNombreDelModelo.Text.Trim))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", txtNombreModelo.Text.Trim))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")

            fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", "Fecha:"))
            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", convertYYYYMMDDtoDDhyphenMMhyphenYYYY(getMySQLDate()) & " " & getAppTime()))
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("  <Cell ss:StyleID=""9""></Cell>")
            fs.WriteLine("    </Row>")


            'Write the grid headers columns (taken out since columns are already defined)
            'For Each col As DataGridViewColumn In dgv.Columns
            '    If col.Visible Then
            '        fs.WriteLine(String.Format("    <Column ss:Width=""{0}""></Column>", col.Width))
            '    End If
            'Next



            'Write the grid headers
            fs.WriteLine("    <Row ss:AutoFitHeight=""0"" ss:Height=""45"">")

            For Each col As DataGridViewColumn In dgvExplosionDeInsumos.Columns
                If col.Visible Then
                    fs.WriteLine(String.Format("      <Cell ss:StyleID=""2""><Data ss:Type=""String"">{0}</Data></Cell>", col.HeaderText))
                End If
            Next


            fs.WriteLine("    </Row>")

            ' Write contents for each cell
            For Each row As DataGridViewRow In dgvExplosionDeInsumos.Rows

                If dgvExplosionDeInsumos.AllowUserToAddRows = True And row.Index = dgvExplosionDeInsumos.Rows.Count - 1 Then
                    Exit For
                End If

                Try

                    'If CDbl(row.Cells(10).Value.ToString) > 0 Then
                    fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
                    'Else
                    '   Continue For
                    'End If

                Catch ex As Exception
                    fs.WriteLine(String.Format("    <Row ss:AutoFitHeight=""0"">"))
                End Try

                For Each col As DataGridViewColumn In dgvExplosionDeInsumos.Columns

                    If col.Visible Then

                        If row.Cells(col.Name).Value.ToString = "" Then

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""></Cell>"))

                        Else

                            fs.WriteLine(String.Format("      <Cell ss:StyleID=""9""><Data ss:Type=""String"">{0}</Data></Cell>", row.Cells(col.Name).Value.ToString))

                        End If

                    End If

                Next col

                fs.WriteLine("    </Row>")

            Next row

            ' Close up the document
            fs.WriteLine("  </Table>")

            fs.WriteLine("  <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">")
            fs.WriteLine("   <PageSetup>")
            fs.WriteLine("  <Layout x:Orientation=""Landscape""/>")
            fs.WriteLine("  <Header x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <Footer x:Margin=""0.51181102362204722""/>")
            fs.WriteLine("  <PageMargins x:Bottom=""0.98425196850393704"" x:Left=""0.74803149606299213"" x:Right=""0.74803149606299213"" x:Top=""0.98425196850393704""/>")
            fs.WriteLine("   </PageSetup>")
            fs.WriteLine("   <Unsynced/>")
            fs.WriteLine("   <Print>")
            fs.WriteLine("  <ValidPrinterInfo/>")
            fs.WriteLine("  <PaperSizeIndex>9</PaperSizeIndex>")
            fs.WriteLine("  <Scale>65</Scale>")
            fs.WriteLine("  <HorizontalResolution>200</HorizontalResolution>")
            fs.WriteLine("  <VerticalResolution>200</VerticalResolution>")
            fs.WriteLine("   </Print>")
            fs.WriteLine("   <Zoom>75</Zoom>")
            fs.WriteLine("   <Selected/>")
            fs.WriteLine("   <Panes>")
            fs.WriteLine("  <Pane>")
            fs.WriteLine("   <Number>3</Number>")
            fs.WriteLine("   <ActiveRow>16</ActiveRow>")
            fs.WriteLine("   <ActiveCol>7</ActiveCol>")
            fs.WriteLine("  </Pane>")
            fs.WriteLine("   </Panes>")
            fs.WriteLine("   <ProtectObjects>False</ProtectObjects>")
            fs.WriteLine("   <ProtectScenarios>False</ProtectScenarios>")
            fs.WriteLine("  </WorksheetOptions>")


            fs.WriteLine("</Worksheet>")
            fs.WriteLine("</Workbook>")
            fs.Close()

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesProjectIsOpen As Integer = 1

        timesProjectIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid & "'")

        If timesProjectIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Modelo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesProjectIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(33) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards"
            queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs"
            queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs"
            queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices"
            'queriesNewId(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion"
            queriesNewId(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber"
            'queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts"
            queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition
            queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts"
            queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards"
            queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs"
            queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs"
            queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices"
            queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber"
            queriesNewId(16) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardsAux"
            queriesNewId(17) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & " SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(18) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(19) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(20) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(21) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(22) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            'queriesNewId(23) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            'queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & " SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(31) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(32) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                imodelid = imodelid + newIdAddition
            End If

        End If

        Dim baseid As Integer = 0

        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If imodelid = 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            imodelid = getSQLQueryAsInteger(0, "SELECT IF(MAX(imodelid) + 1 IS NULL, 1, MAX(imodelid) + 1) AS imodelid FROM models")


            Dim queriesDropCreate(35) As String

            queriesDropCreate(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0"
            queriesDropCreate(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " ( `imodelid` int(11) NOT NULL AUTO_INCREMENT, `smodelname` varchar(200) CHARACTER SET latin1 NOT NULL, `smodeltype` varchar(200) CHARACTER SET latin1 NOT NULL, `dmodellength` varchar(100) CHARACTER SET latin1 NOT NULL, `dmodelwidth` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `smodelfileslocation` varchar(1000) CHARACTER SET latin1 NOT NULL, `dmodelindirectpercentagedefault` decimal(20,5) NOT NULL, `dmodelgainpercentagedefault` decimal(20,5) NOT NULL, `dmodelIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0IndirectCosts"
            queriesDropCreate(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts" & " ( `imodelid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `smodelcostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodelcost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icostid`), KEY `modelid` (`imodelid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Cards"
            queriesDropCreate(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `legacycategoryid` (`scardlegacycategoryid`), KEY `legacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardInputs"
            queriesDropCreate(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardCompoundInputs"
            queriesDropCreate(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Prices"
            queriesDropCreate(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices" & " ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `inputid` (`iinputid`), KEY `modelid` (`imodelid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Explosion"
            'queriesDropCreate(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputrealqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Timber"
            queriesDropCreate(15) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0AdminCosts"
            'queriesDropCreate(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts ( `imodelid` int(11) NOT NULL, `iadmincostid` int(11) NOT NULL, `smodeladmincostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodeladmincost` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iadmincostid`), KEY `modelid` (`imodelid`), KEY `admincostid` (`iadmincostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs"
            queriesDropCreate(19) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(20) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs"
            queriesDropCreate(21) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(22) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards"
            queriesDropCreate(23) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(24) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts"
            queriesDropCreate(25) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(26) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices"
            queriesDropCreate(27) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(28) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid
            queriesDropCreate(29) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(30) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0Timber"
            queriesDropCreate(31) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(32) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardsAux"

            queriesDropCreate(33) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " VALUES (" & imodelid & ", '" & txtNombreModelo.Text & "', '" & validaTipoDeConstruccion() & "', " & txtLongitudVivienda.Text & ", " & txtAnchoVivienda.Text & ", '" & txtRuta.Text.Replace("\", "/") & "', " & txtPorcentajeIndirectosDefault.Text & ", " & txtPorcentajeUtilidadDefault.Text & ", " & txtPorcentajeIVA.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesDropCreate)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET smodelname = '" & txtNombreModelo.Text & "', smodeltype = '" & validaTipoDeConstruccion() & "', dmodellength = " & txtLongitudVivienda.Text & ", dmodelwidth = " & txtLongitudVivienda.Text & ", smodelfileslocation = '" & txtRuta.Text.Replace("\", "/") & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)

        End If

        Dim queries(50) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM models " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp WHERE models.imodelid = tp.imodelid) "

        queries(1) = "" & _
        "DELETE " & _
        "FROM modelindirectcosts " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic WHERE modelindirectcosts.imodelid = tpic.imodelid AND modelindirectcosts.icostid = tpic.icostid) "

        queries(2) = "" & _
        "DELETE " & _
        "FROM modelcards " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc WHERE modelcards.imodelid = tpc.imodelid AND modelcards.icardid = tpc.icardid) "

        queries(3) = "" & _
        "DELETE " & _
        "FROM modelcardinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci WHERE modelcardinputs.imodelid = tpci.imodelid AND modelcardinputs.icardid = tpci.icardid AND modelcardinputs.iinputid = tpci.iinputid) "

        queries(4) = "" & _
        "DELETE " & _
        "FROM modelcardcompoundinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci WHERE modelcardcompoundinputs.imodelid = tpcci.imodelid AND modelcardcompoundinputs.icardid = tpcci.icardid AND modelcardcompoundinputs.iinputid = tpcci.iinputid AND modelcardcompoundinputs.icompoundinputid = tpcci.icompoundinputid) "

        queries(5) = "" & _
        "DELETE " & _
        "FROM modelprices " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp WHERE modelprices.imodelid = tpp.imodelid AND modelprices.iinputid = tpp.iinputid AND modelprices.iupdatedate = tpp.iupdatedate AND modelprices.supdatetime = tpp.supdatetime) "

        'queries(6) = "" & _
        '"DELETE " & _
        '"FROM modelexplosion " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex WHERE modelexplosion.imodelid = tpex.imodelid AND modelexplosion.iinputid = tpex.iinputid) "

        queries(7) = "" & _
        "DELETE " & _
        "FROM modeltimber " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt WHERE modeltimber.imodelid = tpt.imodelid AND modeltimber.iinputid = tpt.iinputid) "

        'queries(47) = "" & _
        '"DELETE " & _
        '"FROM modeladmincosts " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac WHERE modeladmincosts.imodelid = tpac.imodelid AND modeladmincosts.iadmincostid = tpac.iadmincostid) "

        queries(8) = "" & _
        "UPDATE models p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp ON tp.imodelid = p.imodelid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.smodelname = tp.smodelname, p.smodeltype = tp.smodeltype, p.dmodellength = tp.dmodellength, p.dmodelwidth = tp.dmodelwidth, p.smodelfileslocation = tp.smodelfileslocation, p.dmodelindirectpercentagedefault = tp.dmodelindirectpercentagedefault, p.dmodelgainpercentagedefault = tp.dmodelgainpercentagedefault, p.dmodelIVApercentage = tp.dmodelIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queries(9) = "" & _
        "UPDATE modelindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic ON tpic.imodelid = pic.imodelid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.smodelcostname = tpic.smodelcostname, pic.dmodelcost = tpic.dmodelcost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queries(10) = "" & _
        "UPDATE modelcards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc ON tpc.imodelid = pc.imodelid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queries(11) = "" & _
        "UPDATE modelcardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci ON tpci.imodelid = pci.imodelid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queries(12) = "" & _
        "UPDATE modelcardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci ON tpcci.imodelid = pcci.imodelid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.iinputid = tpcci.iinputid, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queries(13) = "" & _
        "UPDATE modelprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp ON tpp.imodelid = pp.imodelid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        'queries(14) = "" & _
        '"UPDATE modelexplosion pex JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex ON tpex.imodelid = pex.imodelid AND tpex.iinputid = pex.iinputid SET pex.iupdatedate = tpex.iupdatedate, pex.supdatetime = tpex.supdatetime, pex.supdateusername = tpex.supdateusername, pex.dinputrealqty = tpex.dinputrealqty WHERE STR_TO_DATE(CONCAT(tpex.iupdatedate, ' ', tpex.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pex.iupdatedate, ' ', pex.supdatetime), '%Y%c%d %T') "

        queries(15) = "" & _
        "UPDATE modeltimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt ON tpt.imodelid = pt.imodelid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        'queries(48) = "" & _
        '"UPDATE modeladmincosts pac JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac ON tpac.imodelid = pac.imodelid AND tpac.iadmincostid = pac.iadmincostid SET pac.iupdatedate = tpac.iupdatedate, pac.supdatetime = tpac.supdatetime, pac.supdateusername = tpac.supdateusername, pac.smodeladmincostname = tpac.smodeladmincostname, pac.dmodeladmincost = tpac.dmodeladmincost WHERE STR_TO_DATE(CONCAT(tpac.iupdatedate, ' ', tpac.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pac.iupdatedate, ' ', pac.supdatetime), '%Y%c%d %T') "

        queries(16) = "" & _
        "INSERT INTO models " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM models p WHERE p.imodelid = tp.imodelid AND p.imodelid = " & imodelid & ") "

        queries(17) = "" & _
        "INSERT INTO modelindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic " & _
        "WHERE NOT EXISTS (SELECT * FROM modelindirectcosts pic WHERE pic.imodelid = tpic.imodelid AND pic.icostid = tpic.icostid AND pic.imodelid = " & imodelid & ") "

        queries(18) = "" & _
        "INSERT INTO modelcards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcards pc WHERE pc.imodelid = tpc.imodelid AND pc.icardid = tpc.icardid AND pc.imodelid = " & imodelid & ") "

        queries(19) = "" & _
        "INSERT INTO modelcardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardinputs pci WHERE pci.imodelid = tpci.imodelid AND pci.icardid = tpci.icardid AND pci.iinputid = tpci.iinputid AND pci.imodelid = " & imodelid & ") "

        queries(20) = "" & _
        "INSERT INTO modelcardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardcompoundinputs pcci WHERE pcci.imodelid = tpcci.imodelid AND pcci.icardid = tpcci.icardid AND pcci.iinputid = tpcci.iinputid AND pcci.icompoundinputid = tpcci.icompoundinputid AND pcci.imodelid = " & imodelid & ") "

        queries(21) = "" & _
        "INSERT INTO modelprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp " & _
        "WHERE NOT EXISTS (SELECT * FROM modelprices pp WHERE pp.imodelid = tpp.imodelid AND pp.iinputid = tpp.iinputid AND pp.iupdatedate = tpp.iupdatedate AND pp.supdatetime = tpp.supdatetime) "

        'queries(22) = "" & _
        '"INSERT INTO modelexplosion " & _
        '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex " & _
        '"WHERE NOT EXISTS (SELECT * FROM modelexplosion pex WHERE pex.imodelid = tpex.imodelid AND pex.iinputid = tpex.iinputid) "

        queries(23) = "" & _
        "INSERT INTO modeltimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt " & _
        "WHERE NOT EXISTS (SELECT * FROM modeltimber pt WHERE pt.imodelid = tpt.imodelid AND pt.iinputid = tpt.iinputid) "

        'queries(49) = "" & _
        '"INSERT INTO modeladmincosts " & _
        '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac " & _
        '"WHERE NOT EXISTS (SELECT * FROM modeladmincosts pac WHERE pac.imodelid = tpac.imodelid AND pac.iadmincostid = tpac.iadmincostid AND pac.imodelid = " & imodelid & ") "

        'queries(24) = "" & _
        '"DELETE " & _
        '"FROM base " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb WHERE base.ibaseid = tb.ibaseid) "

        'queries(25) = "" & _
        '"DELETE " & _
        '"FROM baseindirectcosts " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

        'queries(26) = "" & _
        '"DELETE " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queries(27) = "" & _
        '"DELETE " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queries(28) = "" & _
        '"DELETE " & _
        '"FROM basecardinputs " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

        'queries(29) = "" & _
        '"DELETE " & _
        '"FROM basecardcompoundinputs " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

        'queries(30) = "" & _
        '"DELETE " & _
        '"FROM baseprices " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        'queries(31) = "" & _
        '"DELETE " & _
        '"FROM basetimber " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

        queries(32) = "" & _
        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queries(33) = "" & _
        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queries(34) = "" & _
        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queries(35) = "" & _
        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queries(36) = "" & _
        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queries(37) = "" & _
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        queries(38) = "" & _
        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        queries(39) = "" & _
        "INSERT INTO base " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

        queries(40) = "" & _
        "INSERT INTO baseindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

        queries(41) = "" & _
        "INSERT INTO basecards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

        queries(42) = "" & _
        "INSERT INTO basecardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

        queries(43) = "" & _
        "INSERT INTO basecardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

        queries(44) = "" & _
        "INSERT INTO baseprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

        queries(45) = "" & _
        "INSERT INTO basetimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") "

        queries(46) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Modelo " & imodelid & ": " & txtNombreModelo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            btnRevisiones.Enabled = True
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Cursor.Current = System.Windows.Forms.Cursors.Default



    End Sub


    Private Sub btnGuardarYCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarYCerrar.Click

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesProjectIsOpen As Integer = 1

        timesProjectIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid & "'")

        If timesProjectIsOpen > 1 And isEdit = True Then

            If MsgBox("Otro usuario tiene abierto el mismo Modelo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Cursor.Current = System.Windows.Forms.Cursors.Default
                Exit Sub

            End If

        ElseIf timesProjectIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(33) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards"
            queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs"
            queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs"
            queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices"
            'queriesNewId(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion"
            queriesNewId(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber"
            'queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts"
            queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition
            queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts"
            queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards"
            queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs"
            queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs"
            queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices"
            queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber"
            queriesNewId(16) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardsAux"
            queriesNewId(17) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & " SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(18) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(19) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(20) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(21) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(22) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            'queriesNewId(23) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            'queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & " SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(31) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(32) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                imodelid = imodelid + newIdAddition
            End If

        End If

        Dim baseid As Integer = 0

        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If imodelid = 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            imodelid = getSQLQueryAsInteger(0, "SELECT IF(MAX(imodelid) + 1 IS NULL, 1, MAX(imodelid) + 1) AS imodelid FROM models")

            Dim queriesDropCreate(35) As String

            queriesDropCreate(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0"
            queriesDropCreate(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " ( `imodelid` int(11) NOT NULL AUTO_INCREMENT, `smodelname` varchar(200) CHARACTER SET latin1 NOT NULL, `smodeltype` varchar(200) CHARACTER SET latin1 NOT NULL, `dmodellength` varchar(100) CHARACTER SET latin1 NOT NULL, `dmodelwidth` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `smodelfileslocation` varchar(1000) CHARACTER SET latin1 NOT NULL, `dmodelindirectpercentagedefault` decimal(20,5) NOT NULL, `dmodelgainpercentagedefault` decimal(20,5) NOT NULL, `dmodelIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0IndirectCosts"
            queriesDropCreate(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts" & " ( `imodelid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `smodelcostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodelcost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icostid`), KEY `modelid` (`imodelid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Cards"
            queriesDropCreate(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `legacycategoryid` (`scardlegacycategoryid`), KEY `legacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardInputs"
            queriesDropCreate(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardCompoundInputs"
            queriesDropCreate(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Prices"
            queriesDropCreate(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices" & " ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `inputid` (`iinputid`), KEY `modelid` (`imodelid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Explosion"
            'queriesDropCreate(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputrealqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Timber"
            queriesDropCreate(15) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0AdminCosts"
            'queriesDropCreate(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts ( `imodelid` int(11) NOT NULL, `iadmincostid` int(11) NOT NULL, `smodeladmincostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodeladmincost` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iadmincostid`), KEY `modelid` (`imodelid`), KEY `admincostid` (`iadmincostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs"
            queriesDropCreate(19) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(20) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs"
            queriesDropCreate(21) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(22) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards"
            queriesDropCreate(23) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(24) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts"
            queriesDropCreate(25) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(26) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices"
            queriesDropCreate(27) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(28) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid
            queriesDropCreate(29) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(30) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0Timber"
            queriesDropCreate(31) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(32) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardsAux"

            queriesDropCreate(33) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " VALUES (" & imodelid & ", '" & txtNombreModelo.Text & "', '" & validaTipoDeConstruccion() & "', " & txtLongitudVivienda.Text & ", " & txtAnchoVivienda.Text & ", '" & txtRuta.Text.Replace("\", "/") & "', " & txtPorcentajeIndirectosDefault.Text & ", " & txtPorcentajeUtilidadDefault.Text & ", " & txtPorcentajeIVA.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesDropCreate)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET smodelname = '" & txtNombreModelo.Text & "', smodeltype = '" & validaTipoDeConstruccion() & "', dmodellength = " & txtLongitudVivienda.Text & ", dmodelwidth = " & txtLongitudVivienda.Text & ", smodelfileslocation = '" & txtRuta.Text.Replace("\", "/") & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)

        End If

        Dim queries(50) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM models " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp WHERE models.imodelid = tp.imodelid) "

        queries(1) = "" & _
        "DELETE " & _
        "FROM modelindirectcosts " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic WHERE modelindirectcosts.imodelid = tpic.imodelid AND modelindirectcosts.icostid = tpic.icostid) "

        queries(2) = "" & _
        "DELETE " & _
        "FROM modelcards " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc WHERE modelcards.imodelid = tpc.imodelid AND modelcards.icardid = tpc.icardid) "

        queries(3) = "" & _
        "DELETE " & _
        "FROM modelcardinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci WHERE modelcardinputs.imodelid = tpci.imodelid AND modelcardinputs.icardid = tpci.icardid AND modelcardinputs.iinputid = tpci.iinputid) "

        queries(4) = "" & _
        "DELETE " & _
        "FROM modelcardcompoundinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci WHERE modelcardcompoundinputs.imodelid = tpcci.imodelid AND modelcardcompoundinputs.icardid = tpcci.icardid AND modelcardcompoundinputs.iinputid = tpcci.iinputid AND modelcardcompoundinputs.icompoundinputid = tpcci.icompoundinputid) "

        queries(5) = "" & _
        "DELETE " & _
        "FROM modelprices " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp WHERE modelprices.imodelid = tpp.imodelid AND modelprices.iinputid = tpp.iinputid AND modelprices.iupdatedate = tpp.iupdatedate AND modelprices.supdatetime = tpp.supdatetime) "

        'queries(6) = "" & _
        '"DELETE " & _
        '"FROM modelexplosion " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex WHERE modelexplosion.imodelid = tpex.imodelid AND modelexplosion.iinputid = tpex.iinputid) "

        queries(7) = "" & _
        "DELETE " & _
        "FROM modeltimber " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt WHERE modeltimber.imodelid = tpt.imodelid AND modeltimber.iinputid = tpt.iinputid) "

        'queries(47) = "" & _
        '"DELETE " & _
        '"FROM modeladmincosts " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac WHERE modeladmincosts.imodelid = tpac.imodelid AND modeladmincosts.iadmincostid = tpac.iadmincostid) "

        queries(8) = "" & _
        "UPDATE models p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp ON tp.imodelid = p.imodelid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.smodelname = tp.smodelname, p.smodeltype = tp.smodeltype, p.dmodellength = tp.dmodellength, p.dmodelwidth = tp.dmodelwidth, p.smodelfileslocation = tp.smodelfileslocation, p.dmodelindirectpercentagedefault = tp.dmodelindirectpercentagedefault, p.dmodelgainpercentagedefault = tp.dmodelgainpercentagedefault, p.dmodelIVApercentage = tp.dmodelIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queries(9) = "" & _
        "UPDATE modelindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic ON tpic.imodelid = pic.imodelid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.smodelcostname = tpic.smodelcostname, pic.dmodelcost = tpic.dmodelcost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queries(10) = "" & _
        "UPDATE modelcards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc ON tpc.imodelid = pc.imodelid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queries(11) = "" & _
        "UPDATE modelcardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci ON tpci.imodelid = pci.imodelid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queries(12) = "" & _
        "UPDATE modelcardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci ON tpcci.imodelid = pcci.imodelid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.iinputid = tpcci.iinputid, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queries(13) = "" & _
        "UPDATE modelprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp ON tpp.imodelid = pp.imodelid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        'queries(14) = "" & _
        '"UPDATE modelexplosion pex JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex ON tpex.imodelid = pex.imodelid AND tpex.iinputid = pex.iinputid SET pex.iupdatedate = tpex.iupdatedate, pex.supdatetime = tpex.supdatetime, pex.supdateusername = tpex.supdateusername, pex.dinputrealqty = tpex.dinputrealqty WHERE STR_TO_DATE(CONCAT(tpex.iupdatedate, ' ', tpex.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pex.iupdatedate, ' ', pex.supdatetime), '%Y%c%d %T') "

        queries(15) = "" & _
        "UPDATE modeltimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt ON tpt.imodelid = pt.imodelid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        'queries(48) = "" & _
        '"UPDATE modeladmincosts pac JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac ON tpac.imodelid = pac.imodelid AND tpac.iadmincostid = pac.iadmincostid SET pac.iupdatedate = tpac.iupdatedate, pac.supdatetime = tpac.supdatetime, pac.supdateusername = tpac.supdateusername, pac.smodeladmincostname = tpac.smodeladmincostname, pac.dmodeladmincost = tpac.dmodeladmincost WHERE STR_TO_DATE(CONCAT(tpac.iupdatedate, ' ', tpac.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pac.iupdatedate, ' ', pac.supdatetime), '%Y%c%d %T') "

        queries(16) = "" & _
        "INSERT INTO models " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM models p WHERE p.imodelid = tp.imodelid AND p.imodelid = " & imodelid & ") "

        queries(17) = "" & _
        "INSERT INTO modelindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic " & _
        "WHERE NOT EXISTS (SELECT * FROM modelindirectcosts pic WHERE pic.imodelid = tpic.imodelid AND pic.icostid = tpic.icostid AND pic.imodelid = " & imodelid & ") "

        queries(18) = "" & _
        "INSERT INTO modelcards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcards pc WHERE pc.imodelid = tpc.imodelid AND pc.icardid = tpc.icardid AND pc.imodelid = " & imodelid & ") "

        queries(19) = "" & _
        "INSERT INTO modelcardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardinputs pci WHERE pci.imodelid = tpci.imodelid AND pci.icardid = tpci.icardid AND pci.iinputid = tpci.iinputid AND pci.imodelid = " & imodelid & ") "

        queries(20) = "" & _
        "INSERT INTO modelcardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardcompoundinputs pcci WHERE pcci.imodelid = tpcci.imodelid AND pcci.icardid = tpcci.icardid AND pcci.iinputid = tpcci.iinputid AND pcci.icompoundinputid = tpcci.icompoundinputid AND pcci.imodelid = " & imodelid & ") "

        queries(21) = "" & _
        "INSERT INTO modelprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp " & _
        "WHERE NOT EXISTS (SELECT * FROM modelprices pp WHERE pp.imodelid = tpp.imodelid AND pp.iinputid = tpp.iinputid AND pp.iupdatedate = tpp.iupdatedate AND pp.supdatetime = tpp.supdatetime) "

        'queries(22) = "" & _
        '"INSERT INTO modelexplosion " & _
        '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex " & _
        '"WHERE NOT EXISTS (SELECT * FROM modelexplosion pex WHERE pex.imodelid = tpex.imodelid AND pex.iinputid = tpex.iinputid) "

        queries(23) = "" & _
        "INSERT INTO modeltimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt " & _
        "WHERE NOT EXISTS (SELECT * FROM modeltimber pt WHERE pt.imodelid = tpt.imodelid AND pt.iinputid = tpt.iinputid) "

        'queries(49) = "" & _
        '"INSERT INTO modeladmincosts " & _
        '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac " & _
        '"WHERE NOT EXISTS (SELECT * FROM modeladmincosts pac WHERE pac.imodelid = tpac.imodelid AND pac.iadmincostid = tpac.iadmincostid AND pac.imodelid = " & imodelid & ") "

        'queries(24) = "" & _
        '"DELETE " & _
        '"FROM base " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb WHERE base.ibaseid = tb.ibaseid) "

        'queries(25) = "" & _
        '"DELETE " & _
        '"FROM baseindirectcosts " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

        'queries(26) = "" & _
        '"DELETE " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queries(27) = "" & _
        '"DELETE " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queries(28) = "" & _
        '"DELETE " & _
        '"FROM basecardinputs " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

        'queries(29) = "" & _
        '"DELETE " & _
        '"FROM basecardcompoundinputs " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

        'queries(30) = "" & _
        '"DELETE " & _
        '"FROM baseprices " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        'queries(31) = "" & _
        '"DELETE " & _
        '"FROM basetimber " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

        queries(32) = "" & _
        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queries(33) = "" & _
        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queries(34) = "" & _
        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queries(35) = "" & _
        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queries(36) = "" & _
        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queries(37) = "" & _
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        queries(38) = "" & _
        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        queries(39) = "" & _
        "INSERT INTO base " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

        queries(40) = "" & _
        "INSERT INTO baseindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

        queries(41) = "" & _
        "INSERT INTO basecards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

        queries(42) = "" & _
        "INSERT INTO basecardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

        queries(43) = "" & _
        "INSERT INTO basecardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

        queries(44) = "" & _
        "INSERT INTO baseprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

        queries(45) = "" & _
        "INSERT INTO basetimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") "

        queries(46) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Modelo " & imodelid & ": " & txtNombreModelo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then
            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub


    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        wasCreated = False

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub


    Private Sub btnRevisiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevisiones.Click

        If MsgBox("Revisar un Modelo automáticamente guarda sus cambios. ¿Deseas guardar este Modelo ahora?", MsgBoxStyle.YesNo, "Pregunta Guardado") = MsgBoxResult.No Then
            Exit Sub
        End If

        Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim timesProjectIsOpen As Integer = 1

        timesProjectIsOpen = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid & "'")

        If timesProjectIsOpen > 1 And isEdit = True Then

            Cursor.Current = System.Windows.Forms.Cursors.Default

            If MsgBox("Otro usuario tiene abierto el mismo Modelo. Guardar podría significar que esa persona perdiera sus cambios. ¿Deseas continuar guardando?", MsgBoxStyle.YesNo, "Confirmación Guardado") = MsgBoxResult.No Then

                Exit Sub

            Else

                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            End If

        ElseIf timesProjectIsOpen > 1 And isEdit = False Then

            Dim newIdAddition As Integer = 1

            Do While getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_NAME LIKE '%Model" & imodelid + newIdAddition & "'") > 1 And isEdit = False
                newIdAddition = newIdAddition + 1
            Loop

            'I got the new id (previousId + newIdAddition)

            Dim queriesNewId(33) As String

            queriesNewId(0) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition
            queriesNewId(1) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts"
            queriesNewId(2) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards"
            queriesNewId(3) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs"
            queriesNewId(4) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs"
            queriesNewId(5) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices"
            'queriesNewId(6) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion"
            queriesNewId(7) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber"
            'queriesNewId(8) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts"
            queriesNewId(9) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition
            queriesNewId(10) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts"
            queriesNewId(11) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards"
            queriesNewId(12) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs"
            queriesNewId(13) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs"
            queriesNewId(14) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices"
            queriesNewId(15) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber"
            queriesNewId(16) = "ALTER TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardsAux RENAME TO oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardsAux"
            queriesNewId(17) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & " SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(18) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "IndirectCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(19) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Cards SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(20) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(21) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "CardCompoundInputs SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(22) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Prices SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            'queriesNewId(23) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Explosion SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(24) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "Timber SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            'queriesNewId(25) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid + newIdAddition & "AdminCosts SET imodelid = " & imodelid + newIdAddition & " WHERE imodelid = " & imodelid
            queriesNewId(26) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & " SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(27) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "IndirectCosts SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(28) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Cards SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(29) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(30) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "CardCompoundInputs SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(31) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Prices SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid
            queriesNewId(32) = "UPDATE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid + newIdAddition & "Timber SET ibaseid = " & imodelid + newIdAddition & " WHERE ibaseid = " & imodelid

            If executeTransactedSQLCommand(0, queriesNewId) = True Then
                imodelid = imodelid + newIdAddition
            End If

        End If

        Dim baseid As Integer = 0

        baseid = getSQLQueryAsInteger(0, "SELECT ibaseid FROM base ORDER BY iupdatedate DESC, supdatetime DESC LIMIT 1")

        If baseid = 0 Then
            baseid = 99999
        End If

        If imodelid = 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            imodelid = getSQLQueryAsInteger(0, "SELECT IF(MAX(imodelid) + 1 IS NULL, 1, MAX(imodelid) + 1) AS imodelid FROM models")

            Dim queriesDropCreate(35) As String

            queriesDropCreate(0) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0"
            queriesDropCreate(1) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " ( `imodelid` int(11) NOT NULL AUTO_INCREMENT, `smodelname` varchar(200) CHARACTER SET latin1 NOT NULL, `smodeltype` varchar(200) CHARACTER SET latin1 NOT NULL, `dmodellength` varchar(100) CHARACTER SET latin1 NOT NULL, `dmodelwidth` varchar(100) COLLATE latin1_spanish_ci NOT NULL, `smodelfileslocation` varchar(1000) CHARACTER SET latin1 NOT NULL, `dmodelindirectpercentagedefault` decimal(20,5) NOT NULL, `dmodelgainpercentagedefault` decimal(20,5) NOT NULL, `dmodelIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`) USING BTREE, KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(2) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0IndirectCosts"
            queriesDropCreate(3) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts" & " ( `imodelid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `smodelcostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodelcost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icostid`), KEY `modelid` (`imodelid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(4) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Cards"
            queriesDropCreate(5) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `legacycategoryid` (`scardlegacycategoryid`), KEY `legacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(6) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardInputs"
            queriesDropCreate(7) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(8) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardCompoundInputs"
            queriesDropCreate(9) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs" & " ( `imodelid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `modelid` (`imodelid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(10) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Prices"
            queriesDropCreate(11) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices" & " ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `inputid` (`iinputid`), KEY `modelid` (`imodelid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(12) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Explosion"
            'queriesDropCreate(13) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputrealqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `modelid` (`imodelid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(14) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0Timber"
            queriesDropCreate(15) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber ( `imodelid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            'queriesDropCreate(16) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0AdminCosts"
            'queriesDropCreate(17) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts ( `imodelid` int(11) NOT NULL, `iadmincostid` int(11) NOT NULL, `smodeladmincostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dmodeladmincost` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`imodelid`,`iadmincostid`), KEY `modelid` (`imodelid`), KEY `admincostid` (`iadmincostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(18) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs"
            queriesDropCreate(19) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `icompoundinputid` int(11) NOT NULL, `scompoundinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcompoundinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`,`icompoundinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `compoundinputid` (`icompoundinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(20) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs"
            queriesDropCreate(21) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `scardinputunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardinputqty` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`,`iinputid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(22) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards"
            queriesDropCreate(23) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards ( `ibaseid` int(11) NOT NULL, `icardid` int(11) NOT NULL, `scardlegacycategoryid` varchar(10) CHARACTER SET latin1 NOT NULL, `scardlegacyid` varchar(10) CHARACTER SET latin1 NOT NULL, `scarddescription` varchar(1000) CHARACTER SET latin1 NOT NULL, `scardunit` varchar(50) CHARACTER SET latin1 NOT NULL, `dcardqty` decimal(20,5) NOT NULL, `dcardindirectcostspercentage` decimal(20,5) NOT NULL, `dcardgainpercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icardid`), KEY `baseid` (`ibaseid`), KEY `cardid` (`icardid`), KEY `cardlegacycategoryid` (`scardlegacycategoryid`), KEY `cardlegacyid` (`scardlegacyid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(24) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts"
            queriesDropCreate(25) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts ( `ibaseid` int(11) NOT NULL, `icostid` int(11) NOT NULL, `sbasecostname` varchar(500) CHARACTER SET latin1 NOT NULL, `dbasecost` decimal(20,5) NOT NULL, `dcompanyprojectedearnings` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`icostid`), KEY `baseid` (`ibaseid`), KEY `costid` (`icostid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(26) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices"
            queriesDropCreate(27) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputpricewithoutIVA` decimal(20,5) NOT NULL, `dinputprotectionpercentage` decimal(20,5) NOT NULL, `dinputfinalprice` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`,`iupdatedate`,`supdatetime`), KEY `baseid` (`ibaseid`), KEY `inputid` (`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(28) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid
            queriesDropCreate(29) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " ( `ibaseid` int(11) NOT NULL AUTO_INCREMENT, `sbasefileslocation` varchar(400) CHARACTER SET latin1 NOT NULL, `dbaseindirectpercentagedefault` decimal(20,5) NOT NULL, `dbasegainpercentagedefault` decimal(20,5) NOT NULL, `dbaseIVApercentage` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(30) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base0Timber"
            queriesDropCreate(31) = "CREATE TABLE oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber ( `ibaseid` int(11) NOT NULL, `iinputid` int(11) NOT NULL, `dinputtimberespesor` decimal(20,5) NOT NULL, `dinputtimberancho` decimal(20,5) NOT NULL, `dinputtimberlargo` decimal(20,5) NOT NULL, `dinputtimberpreciopiecubico` decimal(20,5) NOT NULL, `iupdatedate` int(11) NOT NULL, `supdatetime` varchar(11) CHARACTER SET latin1 NOT NULL, `supdateusername` varchar(100) CHARACTER SET latin1 NOT NULL, PRIMARY KEY (`ibaseid`,`iinputid`), KEY `updateuser` (`supdateusername`)) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_spanish_ci"

            queriesDropCreate(32) = "DROP TABLE IF EXISTS oversight.tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model0CardsAux"

            queriesDropCreate(33) = "INSERT INTO tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " VALUES (" & imodelid & ", '" & txtNombreModelo.Text & "', '" & validaTipoDeConstruccion() & "', " & txtLongitudVivienda.Text & ", " & txtAnchoVivienda.Text & ", '" & txtRuta.Text.Replace("\", "/") & "', " & txtPorcentajeIndirectosDefault.Text & ", " & txtPorcentajeUtilidadDefault.Text & ", " & txtPorcentajeIVA.Text & ", " & fecha & ", '" & hora & "', '" & susername & "')"

            executeTransactedSQLCommand(0, queriesDropCreate)

        Else

            executeSQLCommand(0, "UPDATE tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " SET smodelname = '" & txtNombreModelo.Text & "', smodeltype = '" & validaTipoDeConstruccion() & "', dmodellength = " & txtLongitudVivienda.Text & ", dmodelwidth = " & txtLongitudVivienda.Text & ", smodelfileslocation = '" & txtRuta.Text.Replace("\", "/") & "', iupdatedate = " & getMySQLDate() & ", supdatetime = '" & getAppTime() & "', supdateusername = '" & susername & "' WHERE imodelid = " & imodelid)

        End If

        Dim queries(50) As String

        queries(0) = "" & _
        "DELETE " & _
        "FROM models " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp WHERE models.imodelid = tp.imodelid) "

        queries(1) = "" & _
        "DELETE " & _
        "FROM modelindirectcosts " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic WHERE modelindirectcosts.imodelid = tpic.imodelid AND modelindirectcosts.icostid = tpic.icostid) "

        queries(2) = "" & _
        "DELETE " & _
        "FROM modelcards " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc WHERE modelcards.imodelid = tpc.imodelid AND modelcards.icardid = tpc.icardid) "

        queries(3) = "" & _
        "DELETE " & _
        "FROM modelcardinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci WHERE modelcardinputs.imodelid = tpci.imodelid AND modelcardinputs.icardid = tpci.icardid AND modelcardinputs.iinputid = tpci.iinputid) "

        queries(4) = "" & _
        "DELETE " & _
        "FROM modelcardcompoundinputs " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci WHERE modelcardcompoundinputs.imodelid = tpcci.imodelid AND modelcardcompoundinputs.icardid = tpcci.icardid AND modelcardcompoundinputs.iinputid = tpcci.iinputid AND modelcardcompoundinputs.icompoundinputid = tpcci.icompoundinputid) "

        queries(5) = "" & _
        "DELETE " & _
        "FROM modelprices " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp WHERE modelprices.imodelid = tpp.imodelid AND modelprices.iinputid = tpp.iinputid AND modelprices.iupdatedate = tpp.iupdatedate AND modelprices.supdatetime = tpp.supdatetime) "

        'queries(6) = "" & _
        '"DELETE " & _
        '"FROM modelexplosion " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex WHERE modelexplosion.imodelid = tpex.imodelid AND modelexplosion.iinputid = tpex.iinputid) "

        queries(7) = "" & _
        "DELETE " & _
        "FROM modeltimber " & _
        "WHERE imodelid = " & imodelid & " AND " & _
        "NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt WHERE modeltimber.imodelid = tpt.imodelid AND modeltimber.iinputid = tpt.iinputid) "

        'queries(47) = "" & _
        '"DELETE " & _
        '"FROM modeladmincosts " & _
        '"WHERE imodelid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac WHERE modeladmincosts.imodelid = tpac.imodelid AND modeladmincosts.iadmincostid = tpac.iadmincostid) "

        queries(8) = "" & _
        "UPDATE models p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp ON tp.imodelid = p.imodelid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.smodelname = tp.smodelname, p.smodeltype = tp.smodeltype, p.dmodellength = tp.dmodellength, p.dmodelwidth = tp.dmodelwidth, p.smodelfileslocation = tp.smodelfileslocation, p.dmodelindirectpercentagedefault = tp.dmodelindirectpercentagedefault, p.dmodelgainpercentagedefault = tp.dmodelgainpercentagedefault, p.dmodelIVApercentage = tp.dmodelIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queries(9) = "" & _
        "UPDATE modelindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic ON tpic.imodelid = pic.imodelid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.smodelcostname = tpic.smodelcostname, pic.dmodelcost = tpic.dmodelcost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queries(10) = "" & _
        "UPDATE modelcards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc ON tpc.imodelid = pc.imodelid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queries(11) = "" & _
        "UPDATE modelcardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci ON tpci.imodelid = pci.imodelid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queries(12) = "" & _
        "UPDATE modelcardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci ON tpcci.imodelid = pcci.imodelid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.iinputid = tpcci.iinputid, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queries(13) = "" & _
        "UPDATE modelprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp ON tpp.imodelid = pp.imodelid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        'queries(14) = "" & _
        '"UPDATE modelexplosion pex JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex ON tpex.imodelid = pex.imodelid AND tpex.iinputid = pex.iinputid SET pex.iupdatedate = tpex.iupdatedate, pex.supdatetime = tpex.supdatetime, pex.supdateusername = tpex.supdateusername, pex.dinputrealqty = tpex.dinputrealqty WHERE STR_TO_DATE(CONCAT(tpex.iupdatedate, ' ', tpex.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pex.iupdatedate, ' ', pex.supdatetime), '%Y%c%d %T') "

        queries(15) = "" & _
        "UPDATE modeltimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt ON tpt.imodelid = pt.imodelid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        'queries(48) = "" & _
        '"UPDATE modeladmincosts pac JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac ON tpac.imodelid = pac.imodelid AND tpac.iadmincostid = pac.iadmincostid SET pac.iupdatedate = tpac.iupdatedate, pac.supdatetime = tpac.supdatetime, pac.supdateusername = tpac.supdateusername, pac.smodeladmincostname = tpac.smodeladmincostname, pac.dmodeladmincost = tpac.dmodeladmincost WHERE STR_TO_DATE(CONCAT(tpac.iupdatedate, ' ', tpac.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pac.iupdatedate, ' ', pac.supdatetime), '%Y%c%d %T') "

        queries(16) = "" & _
        "INSERT INTO models " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & " tp " & _
        "WHERE NOT EXISTS (SELECT * FROM models p WHERE p.imodelid = tp.imodelid AND p.imodelid = " & imodelid & ") "

        queries(17) = "" & _
        "INSERT INTO modelindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "IndirectCosts tpic " & _
        "WHERE NOT EXISTS (SELECT * FROM modelindirectcosts pic WHERE pic.imodelid = tpic.imodelid AND pic.icostid = tpic.icostid AND pic.imodelid = " & imodelid & ") "

        queries(18) = "" & _
        "INSERT INTO modelcards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Cards tpc " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcards pc WHERE pc.imodelid = tpc.imodelid AND pc.icardid = tpc.icardid AND pc.imodelid = " & imodelid & ") "

        queries(19) = "" & _
        "INSERT INTO modelcardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardInputs tpci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardinputs pci WHERE pci.imodelid = tpci.imodelid AND pci.icardid = tpci.icardid AND pci.iinputid = tpci.iinputid AND pci.imodelid = " & imodelid & ") "

        queries(20) = "" & _
        "INSERT INTO modelcardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "CardCompoundInputs tpcci " & _
        "WHERE NOT EXISTS (SELECT * FROM modelcardcompoundinputs pcci WHERE pcci.imodelid = tpcci.imodelid AND pcci.icardid = tpcci.icardid AND pcci.iinputid = tpcci.iinputid AND pcci.icompoundinputid = tpcci.icompoundinputid AND pcci.imodelid = " & imodelid & ") "

        queries(21) = "" & _
        "INSERT INTO modelprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Prices tpp " & _
        "WHERE NOT EXISTS (SELECT * FROM modelprices pp WHERE pp.imodelid = tpp.imodelid AND pp.iinputid = tpp.iinputid AND pp.iupdatedate = tpp.iupdatedate AND pp.supdatetime = tpp.supdatetime) "

        'queries(22) = "" & _
        '"INSERT INTO modelexplosion " & _
        '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Explosion tpex " & _
        '"WHERE NOT EXISTS (SELECT * FROM modelexplosion pex WHERE pex.imodelid = tpex.imodelid AND pex.iinputid = tpex.iinputid) "

        queries(23) = "" & _
        "INSERT INTO modeltimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "Timber tpt " & _
        "WHERE NOT EXISTS (SELECT * FROM modeltimber pt WHERE pt.imodelid = tpt.imodelid AND pt.iinputid = tpt.iinputid) "

        'queries(49) = "" & _
        '"INSERT INTO modeladmincosts " & _
        '"SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Model" & imodelid & "AdminCosts tpac " & _
        '"WHERE NOT EXISTS (SELECT * FROM modeladmincosts pac WHERE pac.imodelid = tpac.imodelid AND pac.iadmincostid = tpac.iadmincostid AND pac.imodelid = " & imodelid & ") "

        'queries(24) = "" & _
        '"DELETE " & _
        '"FROM base " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb WHERE base.ibaseid = tb.ibaseid) "

        'queries(25) = "" & _
        '"DELETE " & _
        '"FROM baseindirectcosts " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic WHERE baseindirectcosts.ibaseid = tbic.ibaseid AND baseindirectcosts.icostid = tbic.icostid) "

        'queries(26) = "" & _
        '"DELETE " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queries(27) = "" & _
        '"DELETE " & _
        '"FROM basecards " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc WHERE basecards.ibaseid = tbc.ibaseid AND basecards.icardid = tbc.icardid) "

        'queries(28) = "" & _
        '"DELETE " & _
        '"FROM basecardinputs " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci WHERE basecardinputs.ibaseid = tbci.ibaseid AND basecardinputs.icardid = tbci.icardid AND basecardinputs.iinputid = tbci.iinputid) "

        'queries(29) = "" & _
        '"DELETE " & _
        '"FROM basecardcompoundinputs " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci WHERE basecardcompoundinputs.ibaseid = tbcci.ibaseid AND basecardcompoundinputs.icardid = tbcci.icardid AND basecardcompoundinputs.iinputid = tbcci.iinputid AND basecardcompoundinputs.icompoundinputid = tbcci.icompoundinputid) "

        'queries(30) = "" & _
        '"DELETE " & _
        '"FROM baseprices " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp WHERE baseprices.ibaseid = tbp.ibaseid AND baseprices.iinputid = tbp.iinputid AND baseprices.iupdatedate = tbp.iupdatedate AND baseprices.supdatetime = tbp.supdatetime) "

        'queries(31) = "" & _
        '"DELETE " & _
        '"FROM basetimber " & _
        '"WHERE ibaseid = " & imodelid & " AND " & _
        '"NOT EXISTS (SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt WHERE basetimber.ibaseid = tbt.ibaseid AND basetimber.iinputid = tbt.iinputid) "

        queries(32) = "" & _
        "UPDATE base p JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tp ON tp.ibaseid = p.ibaseid SET p.iupdatedate = tp.iupdatedate, p.supdatetime = tp.supdatetime, p.supdateusername = tp.supdateusername, p.sbasefileslocation = tp.sbasefileslocation, p.dbaseindirectpercentagedefault = tp.dbaseindirectpercentagedefault, p.dbasegainpercentagedefault = tp.dbasegainpercentagedefault, p.dbaseIVApercentage = tp.dbaseIVApercentage WHERE STR_TO_DATE(CONCAT(tp.iupdatedate, ' ', tp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T') "

        queries(33) = "" & _
        "UPDATE baseindirectcosts pic JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tpic ON tpic.ibaseid = pic.ibaseid AND tpic.icostid = pic.icostid SET pic.iupdatedate = tpic.iupdatedate, pic.supdatetime = tpic.supdatetime, pic.supdateusername = tpic.supdateusername, pic.sbasecostname = tpic.sbasecostname, pic.dbasecost = tpic.dbasecost, pic.dcompanyprojectedearnings = tpic.dcompanyprojectedearnings WHERE STR_TO_DATE(CONCAT(tpic.iupdatedate, ' ', tpic.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T') "

        queries(34) = "" & _
        "UPDATE basecards pc JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tpc ON tpc.ibaseid = pc.ibaseid AND tpc.icardid = pc.icardid SET pc.iupdatedate = tpc.iupdatedate, pc.supdatetime = tpc.supdatetime, pc.supdateusername = tpc.supdateusername, pc.scardlegacycategoryid = tpc.scardlegacycategoryid, pc.scardlegacyid = tpc.scardlegacyid, pc.scarddescription = tpc.scarddescription, pc.scardunit = tpc.scardunit, pc.dcardqty = tpc.dcardqty, pc.dcardindirectcostspercentage = tpc.dcardindirectcostspercentage, pc.dcardgainpercentage = tpc.dcardgainpercentage WHERE STR_TO_DATE(CONCAT(tpc.iupdatedate, ' ', tpc.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T') "

        queries(35) = "" & _
        "UPDATE basecardinputs pci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tpci ON tpci.ibaseid = pci.ibaseid AND tpci.icardid = pci.icardid AND tpci.iinputid = pci.iinputid SET pci.iupdatedate = tpci.iupdatedate, pci.supdatetime = tpci.supdatetime, pci.supdateusername = tpci.supdateusername, pci.scardinputunit = tpci.scardinputunit, pci.dcardinputqty = tpci.dcardinputqty WHERE STR_TO_DATE(CONCAT(tpci.iupdatedate, ' ', tpci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T') "

        queries(36) = "" & _
        "UPDATE basecardcompoundinputs pcci JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tpcci ON tpcci.ibaseid = pcci.ibaseid AND tpcci.icardid = pcci.icardid AND tpcci.iinputid = pcci.iinputid AND tpcci.icompoundinputid = pcci.icompoundinputid SET pcci.iupdatedate = tpcci.iupdatedate, pcci.supdatetime = tpcci.supdatetime, pcci.supdateusername = tpcci.supdateusername, pcci.scompoundinputunit = tpcci.scompoundinputunit, pcci.dcompoundinputqty = tpcci.dcompoundinputqty WHERE STR_TO_DATE(CONCAT(tpcci.iupdatedate, ' ', tpcci.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T') "

        queries(37) = "" & _
        "UPDATE baseprices pp JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tpp ON tpp.ibaseid = pp.ibaseid AND tpp.iinputid = pp.iinputid AND tpp.iupdatedate = pp.iupdatedate AND tpp.supdatetime = pp.supdatetime SET pp.iupdatedate = tpp.iupdatedate, pp.supdatetime = tpp.supdatetime, pp.supdateusername = tpp.supdateusername, pp.dinputpricewithoutIVA = tpp.dinputpricewithoutIVA, pp.dinputprotectionpercentage = tpp.dinputprotectionpercentage, pp.dinputfinalprice = tpp.dinputfinalprice WHERE STR_TO_DATE(CONCAT(tpp.iupdatedate, ' ', tpp.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pp.iupdatedate, ' ', pp.supdatetime), '%Y%c%d %T') "

        queries(38) = "" & _
        "UPDATE basetimber pt JOIN tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tpt ON tpt.ibaseid = pt.ibaseid AND tpt.iinputid = pt.iinputid SET pt.iupdatedate = tpt.iupdatedate, pt.supdatetime = tpt.supdatetime, pt.supdateusername = tpt.supdateusername, pt.dinputtimberespesor = tpt.dinputtimberespesor, pt.dinputtimberancho = tpt.dinputtimberancho, pt.dinputtimberlargo = tpt.dinputtimberlargo, pt.dinputtimberpreciopiecubico = tpt.dinputtimberpreciopiecubico WHERE STR_TO_DATE(CONCAT(tpt.iupdatedate, ' ', tpt.supdatetime), '%Y%c%d %T') > STR_TO_DATE(CONCAT(pt.iupdatedate, ' ', pt.supdatetime), '%Y%c%d %T') "

        queries(39) = "" & _
        "INSERT INTO base " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & " tb " & _
        "WHERE NOT EXISTS (SELECT * FROM base b WHERE b.ibaseid = tb.ibaseid AND b.ibaseid = " & baseid & ") "

        queries(40) = "" & _
        "INSERT INTO baseindirectcosts " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "IndirectCosts tbic " & _
        "WHERE NOT EXISTS (SELECT * FROM baseindirectcosts bic WHERE tbic.ibaseid = bic.ibaseid AND tbic.icostid = bic.icostid AND bic.ibaseid = " & baseid & ") "

        queries(41) = "" & _
        "INSERT INTO basecards " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Cards tbc " & _
        "WHERE NOT EXISTS (SELECT * FROM basecards bc WHERE tbc.ibaseid = bc.ibaseid AND tbc.icardid = bc.icardid AND bc.ibaseid = " & baseid & ") "

        queries(42) = "" & _
        "INSERT INTO basecardinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardInputs tbci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardinputs bci WHERE tbci.ibaseid = bci.ibaseid AND tbci.icardid = bci.icardid AND tbci.iinputid = bci.iinputid AND bci.ibaseid = " & baseid & ") "

        queries(43) = "" & _
        "INSERT INTO basecardcompoundinputs " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "CardCompoundInputs tbcci " & _
        "WHERE NOT EXISTS (SELECT * FROM basecardcompoundinputs bcci WHERE tbcci.ibaseid = bcci.ibaseid AND tbcci.icardid = bcci.icardid AND tbcci.iinputid = bcci.iinputid AND tbcci.icompoundinputid = bcci.icompoundinputid AND bcci.ibaseid = " & baseid & ") "

        queries(44) = "" & _
        "INSERT INTO baseprices " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Prices tbp " & _
        "WHERE NOT EXISTS (SELECT * FROM baseprices bp WHERE tbp.ibaseid = bp.ibaseid AND tbp.iinputid = bp.iinputid AND tbp.iupdatedate = bp.iupdatedate AND tbp.supdatetime = bp.supdatetime AND bp.ibaseid = " & baseid & ") "

        queries(45) = "" & _
        "INSERT INTO basetimber " & _
        "SELECT * FROM tmp" & susername.Substring(0, 1).ToUpper & susername.Substring(1) & "S" & susersession & "Base" & imodelid & "Timber tbt " & _
        "WHERE NOT EXISTS (SELECT * FROM basetimber bt WHERE tbt.ibaseid = bt.ibaseid AND tbt.iinputid = bt.iinputid AND bt.ibaseid = " & baseid & ") "

        queries(46) = "INSERT IGNORE INTO logs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', '" & susername & "', " & susersession & ", '" & suserip & "', '" & susermachinename & "', 'Guardó cambios al Modelo " & imodelid & ": " & txtNombreModelo.Text.Replace("--", "").Replace("'", "") & "', 'OK')"

        If executeTransactedSQLCommand(0, queries) = True Then

            MsgBox("Guardado exitosamente", MsgBoxStyle.OkOnly, "")
            wasCreated = True

            Dim br As New BuscaRevisiones

            br.susername = susername
            br.bactive = bactive
            br.bonline = bonline
            br.suserfullname = suserfullname
            br.suseremail = suseremail
            br.susersession = susersession
            br.susermachinename = susermachinename
            br.suserip = suserip

            br.isEdit = True

            br.srevisiondocument = "Modelo"
            br.sid = imodelid

            If Me.WindowState = FormWindowState.Maximized Then
                br.WindowState = FormWindowState.Maximized
            End If

            Me.Visible = False
            br.ShowDialog(Me)
            Me.Visible = True

        Else
            MsgBox("Hubo un error al Guardar. Probablemente un error de Red. Intenta nuevamente", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        wasCreated = True

        Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub
    

End Class