Imports Microsoft.VisualBasic
Imports System.Net.NetworkInformation
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Management
Imports System.Data
Imports System.Net.Mail
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices


Public Module Utilities


    Private Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As String, ByRef hWnd As Long, ByRef Msg As Long, ByRef wParam As Long, ByRef lParam As Long) As Long

    <DllImport("kernel32", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
    Public Function LoadLibrary(ByVal libraryName As String) As IntPtr
    End Function

    <DllImport("kernel32", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
    Public Function GetProcAddress(ByVal hwnd As IntPtr, ByVal procedureName As String) As IntPtr
    End Function

    Private Delegate Function IsWow64ProcessDelegate(<[In]()> ByVal handle As IntPtr, <Out()> ByRef isWow64Process As Boolean) As Boolean



    Dim eax As Long
    Dim ebx As Long
    Dim ecx As Long
    Dim edx As Long
    Private m_CPUAsm As String


    '========================================================================================
    'SQL Connections


    'Public servidor As String = "192.168.1.96"
    ''Dim servidor As String = "localhost"
    'Public dataLocation As String = ""
    'Public programUpdatesLocation As String = ""
    'Dim usuario As String = "memozebadua"
    'Dim password As String = "DLF1594G"
    'Dim database As String = "oversight"

    'Public servidor As String = "oversight.db.7462515.hostedresource.com"
    Public servidor As String = "localhost"
    Public dataLocation As String = ""
    Public programUpdatesLocation As String = ""
    Dim usuario As String = "oversight"
    Dim password As String = "MemoZebadua4"
    Dim database As String = "oversight"




    Public Function defineDBServer() As Boolean

        loadProgramSettings()

        If getSystemStatus(servidor) = "System Online" Then
            Return True
        Else
            Return False
        End If

    End Function


    Public Function loadProgramSettings() As Boolean

        Dim mac2 As Settings = Settings.GetAssemConfig()

        If mac2.rutaArchivos = "" Or mac2.rutaArchivos Is DBNull.Value Then
            dataLocation = "C:\"
        Else
            dataLocation = mac2.rutaArchivos
        End If

        If mac2.rutaActualizacion = "" Or mac2.rutaActualizacion Is DBNull.Value Then
            programUpdatesLocation = "C:\Actualizaciones"
        Else
            programUpdatesLocation = mac2.rutaActualizacion
        End If

        If mac2.dbLocationIPOrMachineName = "" Or mac2.dbLocationIPOrMachineName Is DBNull.Value Then
            servidor = "localhost"
        Else
            servidor = mac2.dbLocationIPOrMachineName
        End If

    End Function


    Public Function StringConnection() As String

        Return "Server=" & servidor & ";Database=" & database & ";Port=3306;" & _
        "Uid=" & usuario & ";Pwd=" & password & ";Connect Timeout=30;Allow User Variables=True;"

    End Function


    Public Function RootStringConnection() As String

        Dim rootuser As String = "memozebadua"
        Dim rootpassword As String = "omudes"

        Return "Server=" & servidor & ";Database=" & database & ";" & _
        "Uid=" & rootuser & ";Pwd=" & rootpassword & ";Connect Timeout=30;Allow User Variables=True;"

    End Function


    '========================================================================================
    'Online Checks


    Public Function getSystemStatus(ByVal servidor As String) As String

        Dim output As String = ""

        If TryPing(servidor) = False Then
            Return "Server Offline"
        Else
            If isMySQLOnline(0) = False Then
                Return "MySQL Offline"
            Else
                If isSystemOnline(0) = False Then
                    Return "System Offline"
                Else
                    Return "System Online"
                End If
            End If
        End If

    End Function


    Public Function isMySQLOnline(ByVal rootAccessRequired As Integer) As Boolean

        Dim result As String = ""

        result = getSQLQueryAsString(0, "SELECT now()")

        If result.Equals("") Then
            Return False
        Else
            Return True
        End If

    End Function


    Public Function isSystemOnline(ByVal rootAccessRequired As Integer) As Boolean

        Dim result As Integer = 0

        result = getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM countriespersistent")

        If result = 0 Then
            Return False
        Else
            Return True
        End If

    End Function



    '========================================================================================
    ' Program Updates Functions


    Public Function updateOversightIfNewVersionIsAvailable() As Boolean

        Try

            If programUpdatesLocation.Contains("//") Then

                If TryPing(programUpdatesLocation.Substring(0, programUpdatesLocation.Replace("//", "").IndexOf("/") + 2).Replace("//", "")) = False Then
                    Return False
                End If

            End If

            Dim f() As String = Directory.GetFiles(programUpdatesLocation)

            For i As Integer = 0 To UBound(f)

                If Path.GetFileName(f(i).Replace("/", "\")) = "Oversight.exe" Then

                    Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(programUpdatesLocation, Path.GetFileName(f(i))))

                    If myFileVersionInfo.ProductMajorPart > My.Application.Info.Version.Major Then

                        'MsgBox("Hay una nueva versión de Oversight. Se abrirá una ventana de copiado que cerrará el programa y lo actualizará el programa.", MsgBoxStyle.OkOnly)
                        'Return updateOversightFiles()

                        MsgBox("Hay una nueva versión de Oversight. Actualiza el programa y así podrás continuar.", MsgBoxStyle.OkOnly)

                        Try
                            System.Diagnostics.Process.Start(programUpdatesLocation)
                            System.Diagnostics.Process.Start(Application.StartupPath)
                        Catch ex As Exception

                        End Try

                    ElseIf myFileVersionInfo.ProductMajorPart = My.Application.Info.Version.Major Then

                        If myFileVersionInfo.ProductMinorPart > My.Application.Info.Version.Minor Then

                            'MsgBox("Hay una nueva versión de Oversight. Se abrirá una ventana de copiado que cerrará el programa y lo actualizará el programa.", MsgBoxStyle.OkOnly)
                            'Return updateOversightFiles()
                            MsgBox("Hay una nueva versión de Oversight. Actualiza el programa y así podrás continuar.", MsgBoxStyle.OkOnly)

                            Try
                                System.Diagnostics.Process.Start(programUpdatesLocation)
                                System.Diagnostics.Process.Start(Application.StartupPath)
                            Catch ex As Exception

                            End Try

                        ElseIf myFileVersionInfo.ProductMinorPart = My.Application.Info.Version.Minor Then

                            If myFileVersionInfo.ProductBuildPart > My.Application.Info.Version.Build Then

                                'MsgBox("Hay una nueva versión de Oversight. Se abrirá una ventana de copiado que cerrará el programa y lo actualizará el programa.", MsgBoxStyle.OkOnly)
                                'Return updateOversightFiles()
                                MsgBox("Hay una nueva versión de Oversight. Actualiza el programa y así podrás continuar.", MsgBoxStyle.OkOnly)

                                Try
                                    System.Diagnostics.Process.Start(programUpdatesLocation)
                                    System.Diagnostics.Process.Start(Application.StartupPath)
                                Catch ex As Exception

                                End Try

                            ElseIf myFileVersionInfo.ProductBuildPart = My.Application.Info.Version.Build Then

                                If myFileVersionInfo.ProductPrivatePart > My.Application.Info.Version.Revision Then

                                    'MsgBox("Hay una nueva versión de Oversight. Se abrirá una ventana de copiado que cerrará el programa y lo actualizará el programa.", MsgBoxStyle.OkOnly)
                                    'Return updateOversightFiles()
                                    MsgBox("Hay una nueva versión de Oversight. Actualiza el programa y así podrás continuar.", MsgBoxStyle.OkOnly)

                                    Try
                                        System.Diagnostics.Process.Start(programUpdatesLocation)
                                        System.Diagnostics.Process.Start(Application.StartupPath)
                                    Catch ex As Exception

                                    End Try

                                ElseIf myFileVersionInfo.ProductPrivatePart = My.Application.Info.Version.Revision Then

                                    Return False

                                Else

                                    Return False

                                End If

                            Else

                                Return False

                            End If

                        Else

                            Return False

                        End If

                    Else

                        Return False

                    End If

                End If

            Next i

        Catch ex As Exception

            Return False

        End Try

    End Function


    Private Function updateOversightFiles() As Boolean

        Try

            Dim comando As String = ""
            comando = "C:\Windows\System32\xcopy.exe " & programUpdatesLocation.Replace("/", "\") & " " & Application.StartupPath & " /K /H /E /Z /C /Y"
            'MsgBox(comando)
            Shell(comando, AppWinStyle.NormalFocus, False, 1)

            System.Environment.Exit(0)

        Catch ex As Exception

            Return False

        End Try

    End Function


    Public Function IsOS64Bit() As Boolean
        If IntPtr.Size = 8 OrElse (IntPtr.Size = 4 AndAlso Is32BitProcessOn64BitProcessor()) Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Function Is32BitProcessOn64BitProcessor() As Boolean
        Dim fnDelegate As IsWow64ProcessDelegate = GetIsWow64ProcessDelegate()

        If fnDelegate Is Nothing Then
            Return False
        End If

        Dim isWow64 As Boolean
        Dim retVal As Boolean = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, isWow64)

        If retVal = False Then
            Return False
        End If

        Return isWow64
    End Function


    Private Function GetIsWow64ProcessDelegate() As IsWow64ProcessDelegate
        Dim handle As IntPtr = LoadLibrary("kernel32")

        If handle <> IntPtr.Zero Then
            Dim fnPtr As IntPtr = GetProcAddress(handle, "IsWow64Process")

            If fnPtr <> IntPtr.Zero Then
                Return DirectCast(Marshal.GetDelegateForFunctionPointer(DirectCast(fnPtr, IntPtr), GetType(IsWow64ProcessDelegate)), IsWow64ProcessDelegate)
            End If
        End If

        Return Nothing
    End Function



    '========================================================================================
    'SQL Helpers


    Public Function getSQLQueryAsDataset(ByVal rootAccessRequired As Integer, ByVal query As String) As DataSet

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objDA As New MySqlDataAdapter(query, objCon)
        Dim dsDatos As New DataSet

        Try
            objDA.Fill(dsDatos)
        Catch ex As Exception
            'Nothing
        Finally
            objCon.Close()
            objCon.Dispose()
            objDA.Dispose()
        End Try

        Return dsDatos

    End Function


    Public Function get2SQLQueriesInSameDataset(ByVal rootAccessRequired As Integer, ByVal query As String, ByVal query2 As String, ByVal nivel1PK As String, ByVal nivel2PK As String) As DataSet

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objDA As New MySqlDataAdapter(query, objCon)
        Dim dsDatos As New DataSet

        Try

            objDA.Fill(dsDatos, "Nivel1")
            objDA = New MySqlDataAdapter(query2, objCon)
            objDA.Fill(dsDatos, "Nivel2")
            dsDatos.Relations.Add("Children", dsDatos.Tables(0).Columns(nivel1PK), dsDatos.Tables(1).Columns(nivel2PK))

        Catch ex As Exception

            'Nothing
            Dim exa As String
            exa = ex.ToString

        Finally

            objCon.Close()
            objCon.Dispose()
            objDA.Dispose()

        End Try

        Return dsDatos

    End Function


    Public Function getSQLQueryAsString(ByVal rootAccessRequired As Integer, ByVal query As String) As String

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objCmd As New MySqlCommand(query, objCon)
        Dim objRdr As MySqlDataReader

        Try

            objCon.Open()
            objRdr = objCmd.ExecuteReader
            objRdr.Read()
            If objRdr.HasRows Then
                Return objRdr(0)
            Else
                Return ""
            End If

        Catch ex As Exception
            Return ""
        Finally
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
        End Try

    End Function


    Public Function getSQLQueryAsInteger(ByVal rootAccessRequired As Integer, ByVal query As String) As Integer

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objCmd As New MySqlCommand(query, objCon)
        Dim objRdr As MySqlDataReader

        Try

            objCon.Open()
            objRdr = objCmd.ExecuteReader
            objRdr.Read()
            If objRdr.HasRows Then
                Return CInt(objRdr(0))
            Else
                Return 0
            End If

        Catch ex As Exception
            Return 0
        Finally
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
        End Try

    End Function


    Public Function getSQLQueryAsDouble(ByVal rootAccessRequired As Integer, ByVal query As String) As Double

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objCmd As New MySqlCommand(query, objCon)
        Dim objRdr As MySqlDataReader

        Try

            objCon.Open()
            objRdr = objCmd.ExecuteReader
            objRdr.Read()
            If objRdr.HasRows Then
                Return CDbl(objRdr(0))
            Else
                Return 0.0
            End If

        Catch ex As Exception
            Return 0.0
        Finally
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
        End Try

    End Function


    Public Function getSQLQueryAsBoolean(ByVal rootAccessRequired As Integer, ByVal query As String) As Boolean

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objCmd As New MySqlCommand(query, objCon)
        Dim objRdr As MySqlDataReader

        Try

            objCon.Open()
            objRdr = objCmd.ExecuteReader
            objRdr.Read()
            If objRdr.HasRows Then
                Return CBool(objRdr(0))
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        Finally
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
        End Try

    End Function


    Public Function getSQLQueryAsPagedDataSet(ByVal rootAccessRequired As Integer, ByVal query As String, ByVal currentPageIndex As Integer, ByVal pagingSize As Integer) As DataSet

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objCmd As New MySqlDataAdapter(query, objCon)
        Dim dsDatos As New DataSet

        Try
            objCmd.Fill(dsDatos, currentPageIndex, pagingSize, "mirrorforsearch")
        Catch ex As Exception
            'Nothing
        Finally
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
        End Try

        Return dsDatos

    End Function


    Public Function getSQLQueryAsDataTable(ByVal rootAccessRequired As Integer, ByVal query As String) As DataTable

        Dim objCon As MySqlConnection
        If rootAccessRequired = 0 Then
            objCon = New MySqlConnection(StringConnection())
        Else
            objCon = New MySqlConnection(RootStringConnection())
        End If
        Dim objCmd As New MySqlDataAdapter(query, objCon)
        Dim dsDatos As New DataTable

        Try
            objCmd.Fill(dsDatos)
        Catch ex As Exception
            'Nothing
        Finally
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
        End Try

        Return dsDatos

    End Function


    Public Function executeSQLCommand(ByVal rootAccessRequired As Integer, ByVal query As String) As Boolean

        Try
            Dim objCon As MySqlConnection
            If rootAccessRequired = 0 Then
                objCon = New MySqlConnection(StringConnection())
            Else
                objCon = New MySqlConnection(RootStringConnection())
            End If
            Dim objCmd As New MySqlCommand(query, objCon)
            objCon.Open()
            objCmd.CommandText = query
            objCmd.Connection = objCon
            objCmd.ExecuteNonQuery()
            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()
            Return True
        Catch ex As Exception

            If ex.InnerException Is Nothing Then
                executeSQLCommand(0, "INSERT IGNORE INTO errorlogs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', 'The following query produced an exception: " & preventSQLInjection(query) & " : " & ex.ToString.Replace("'", "") & "')")
            Else
                executeSQLCommand(0, "INSERT IGNORE INTO errorlogs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', 'The following query produced an exception: " & preventSQLInjection(query) & " : " & ex.ToString.Replace("'", "") & " Inner Exception: " & ex.InnerException.ToString.Replace("'", "") & "')")
            End If

            'MsgBox(ex.ToString)

            Return False

        End Try

    End Function


    Public Function executeTransactedSQLCommand(ByVal rootAccessRequired As Integer, ByVal queries As String()) As Boolean

        Dim i As Integer = 0

        Try
            Dim objCon As MySqlConnection
            If rootAccessRequired = 0 Then
                objCon = New MySqlConnection(StringConnection())
            Else
                objCon = New MySqlConnection(RootStringConnection())
            End If

            Dim objCmd As MySqlCommand

            objCmd = New MySqlCommand("START TRANSACTION", objCon)
            objCon.Open()
            objCmd.CommandText = "START TRANSACTION"
            objCmd.Connection = objCon
            objCmd.ExecuteNonQuery()

            For i = 0 To queries.Length - 1

                If queries(i) Is DBNull.Value Or queries(i) = "" Then
                    Continue For
                End If

                objCmd.CommandText = queries(i)
                objCmd.Connection = objCon
                objCmd.ExecuteNonQuery()

            Next i

            objCmd.CommandText = "COMMIT"
            objCmd.Connection = objCon
            objCmd.ExecuteNonQuery()

            objCon.Close()
            objCon.Dispose()
            objCmd.Dispose()

            Return True

        Catch ex As Exception

            If ex.InnerException Is Nothing Then
                executeSQLCommand(0, "INSERT IGNORE INTO errorlogs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', 'The following query produced an exception: " & preventSQLInjection(queries(i)) & " : " & ex.ToString.Replace("'", "") & "')")
            Else
                executeSQLCommand(0, "INSERT IGNORE INTO errorlogs VALUES (" & getMySQLDate() & ", '" & getAppTime() & "', 'The following query produced an exception: " & preventSQLInjection(queries(i)) & " : " & ex.ToString.Replace("'", "") & " Inner Exception: " & ex.InnerException.ToString.Replace("'", "") & "')")
            End If

            'MsgBox(ex.ToString)

            Dim errorAtIteration As Integer = 0
            Dim verQueries As String = queries.Length
            errorAtIteration = i

            Dim objCon As MySqlConnection
            If rootAccessRequired = 0 Then
                objCon = New MySqlConnection(StringConnection())
            Else
                objCon = New MySqlConnection(RootStringConnection())
            End If

            Try

                Dim objCmd As MySqlCommand
                objCmd = New MySqlCommand("ROLLBACK", objCon)
                objCon.Open()
                objCmd.CommandText = "ROLLBACK"
                objCmd.Connection = objCon
                objCmd.ExecuteNonQuery()

            Catch ex2 As Exception

            End Try

            Return False

        End Try

    End Function


    Public Function preventSQLInjection(ByVal value As String) As String

        Return value.Replace("'", "''")

    End Function


    '========================================================================================
    'Object Fillers


    'Public Function fillDatagridPaged(ByVal rootAccessRequired As Integer, ByVal dg As DataGrid, ByVal query As String, ByVal currentPageIndex As Integer, ByVal pagingSize As Integer) As Boolean

    '    Dim objds As DataSet = getSQLQueryAsPagedDataSet(rootAccessRequired, query, currentPageIndex, pagingSize)
    '    dg.DataSource = objds.Tables(0).DefaultView
    '    dg.DataBind()
    '    Return True

    'End Function


    Public Function setDataGridView(ByVal dgv As DataGridView, ByVal query As String, ByVal sololectura As Boolean) As DataTable

        Dim objCon As New MySqlConnection(StringConnection())
        Dim objDA As MySqlDataAdapter
        Dim objCmd As New MySqlCommand(query, objCon)
        Dim myBindingSource As New BindingSource
        Dim dsDatos As New DataSet

        Try

            objDA = New MySqlDataAdapter(objCmd)

            objDA.Fill(dsDatos)

            myBindingSource.DataSource = dsDatos.Tables(0).DefaultView

            dgv.DataSource = myBindingSource
            dgv.ReadOnly = sololectura

            'Returning DataTable for Charting sourcing purposes
            Return dsDatos.Tables(0)

        Catch ex As Exception

            Dim dtDummy As New DataTable
            Return dtDummy

        End Try


    End Function


    '========================================================================================
    'Functions




    'Public Function randomizeMainPagePhoto(ByVal lang As String, ByVal img As Image) As Boolean

    '    Dim luckyNumber1 As Integer
    '    Dim limitNumber1 As Integer
    '    Dim dsImages As Data.DataSet

    '    Randomize()

    '    limitNumber1 = getSQLQueryAsInteger(0, "SELECT MAX(iimageid) FROM houseimages")
    '    luckyNumber1 = CInt(Int((limitNumber1 - 1 + 1) * Rnd() + 1))

    '    If luckyNumber1 <= 0 Then
    '        luckyNumber1 = 1
    '    ElseIf luckyNumber1 > limitNumber1 Then
    '        luckyNumber1 = limitNumber1
    '    End If

    '    dsImages = getSQLQueryAsDataset(0, "SELECT iimageid, simageurl, simagedescription_" & lang & " FROM houseimages WHERE iimageid = " & luckyNumber1)
    '    img = dsImages.Tables(0).Rows(0).Item(1)
    '    img.ToolTip = dsImages.Tables(0).Rows(0).Item(2)

    '    Return True

    'End Function


    'Public Function randomizeAppPagePhoto(ByVal lang As String, ByVal img As System.Web.UI.WebControls.Image) As Boolean

    '    Dim luckyNumber1 As Integer
    '    Dim limitNumber1 As Integer
    '    Dim dsImages As Data.DataSet

    '    Randomize()

    '    limitNumber1 = getSQLQueryAsInteger(0, "SELECT MAX(iimageid) FROM housesmallimages")
    '    luckyNumber1 = CInt(Int((limitNumber1 - 1 + 1) * Rnd() + 1))

    '    If luckyNumber1 <= 0 Then
    '        luckyNumber1 = 1
    '    ElseIf luckyNumber1 > limitNumber1 Then
    '        luckyNumber1 = limitNumber1
    '    End If

    '    dsImages = getSQLQueryAsDataset(0, "SELECT iimageid, simageurl, simagedescription_" & lang & " FROM housesmallimages WHERE iimageid = " & luckyNumber1)
    '    img.ImageUrl = dsImages.Tables(0).Rows(0).Item(1)
    '    img.ToolTip = dsImages.Tables(0).Rows(0).Item(2)

    '    Return True

    'End Function


    Public Function GetTxtFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String

        Dim strContents As String = ""
        Dim objReader As StreamReader

        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents

        Catch Ex As Exception

            ErrInfo = Ex.Message
            Return strContents

        End Try

    End Function


    Public Function SaveTextToTxtFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

        Dim bAns As Boolean = False
        Dim objReader As StreamWriter

        Try


            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True

        Catch Ex As Exception

            ErrInfo = Ex.Message

        End Try

        Return bAns

    End Function


    '========================================================================================
    'Encryption Functions


    Dim dict() As Integer = New Integer() {97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57}


    Public Function SHA512Protect(ByVal plainText As String) As String

        Dim data(plainText.Length - 1) As Byte
        Dim result() As Byte
        Dim shaM As New SHA512Managed()
        result = shaM.ComputeHash(data)

        Return Convert.ToBase64String(result)

    End Function


    Public Function MD5Protect(ByVal plainText As String) As String

        Dim hash As HashAlgorithm
        hash = New MD5CryptoServiceProvider

        Dim plainTextBytes As Byte()
        plainTextBytes = Encoding.UTF8.GetBytes(plainText)
        Dim plainTextWithSaltBytes() As Byte = New Byte(plainTextBytes.Length - 1) {}

        Dim I As Integer
        For I = 0 To plainTextBytes.Length - 1
            plainTextWithSaltBytes(I) = plainTextBytes(I)
        Next I

        Dim hashBytes As Byte()
        hashBytes = hash.ComputeHash(plainTextWithSaltBytes)
        Dim hashWithSaltBytes() As Byte = New Byte(hashBytes.Length - 1) {}

        For I = 0 To hashBytes.Length - 1
            hashWithSaltBytes(I) = hashBytes(I)
        Next I

        Dim hashValue As String
        hashValue = Convert.ToBase64String(hashWithSaltBytes)

        Return hashValue

    End Function


    Public Function base64Encode(ByVal sData As String) As String

        Try
            Dim encData_byte As Byte() = New Byte(sData.Length - 1) {}

            encData_byte = System.Text.Encoding.UTF8.GetBytes(sData)

            Dim encodedData As String = Convert.ToBase64String(encData_byte)


            Return encodedData
        Catch ex As Exception
            Throw New Exception("Error in base64Encode" + ex.Message)
        End Try

    End Function


    Public Function base64Decode(ByVal sData As String) As String

        Dim encoder As New System.Text.UTF8Encoding()

        Dim utf8Decode As System.Text.Decoder = encoder.GetDecoder()

        Dim todecode_byte As Byte() = Convert.FromBase64String(sData)

        Dim charCount As Integer = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length)

        Dim decoded_char As Char() = New Char(charCount - 1) {}

        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0)

        Dim result As String = New [String](decoded_char)

        Return result

    End Function


    Public Function VignereProtect(ByVal textToEncrypt As String) As String
        Dim i As Integer
        Dim cSal As String
        Dim cMsg As Integer
        Dim cCla As Integer
        Dim pCla As Integer
        Dim key As String = "Oversight"

        pCla = 1
        cSal = ""

        For i = 1 To Len(textToEncrypt)
            cMsg = getPositionInDictionary(Mid(textToEncrypt, i, 1))
            cCla = getPositionInDictionary(Mid(key, pCla, 1))
            cSal = cSal + getCharacterFromDictionary((adjustNumberToEncrypt(cCla + cMsg)))
            pCla = pCla + 1
            If pCla > Len(key) Then
                pCla = 1
            End If
        Next i
        Return cSal
    End Function


    Function getPositionInDictionary(ByVal car As Char) As Integer
        Dim i, salida As Integer

        salida = -1
        For i = 0 To dict.Length - 1
            If dict(i) = Asc(car) Then
                Return i
            End If
        Next i
        Return salida

    End Function


    Function getCharacterFromDictionary(ByVal pos As Integer) As String
        Return Chr(dict(pos))
    End Function


    Public Function adjustNumberToEncrypt(ByVal number As Integer) As Integer
        If number > (dict.Length - 1) Then
            adjustNumberToEncrypt = number - dict.Length
        Else
            adjustNumberToEncrypt = number
        End If

    End Function


    Public Function Encrypt(ByVal plainText As String, ByVal hashAlgorithm As String, ByVal saltBytes() As Byte) As String

        ' If salt is not specified, generate it on the fly.
        If (saltBytes Is Nothing) Then

            ' Define min and max salt sizes.
            Dim minSaltSize As Integer
            Dim maxSaltSize As Integer

            minSaltSize = 4
            maxSaltSize = 8

            ' Generate a random number for the size of the salt.
            Dim random As Random
            random = New Random()

            Dim saltSize As Integer
            saltSize = random.Next(minSaltSize, maxSaltSize)

            ' Allocate a byte array, which will hold the salt.
            saltBytes = New Byte(saltSize - 1) {}

            ' Initialize a random number generator.
            Dim rng As RNGCryptoServiceProvider
            rng = New RNGCryptoServiceProvider()

            ' Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes)
        End If

        ' Convert plain text into a byte array.
        Dim plainTextBytes As Byte()
        plainTextBytes = Encoding.UTF8.GetBytes(plainText)

        ' Allocate array, which will hold plain text and salt.
        Dim plainTextWithSaltBytes() As Byte = _
            New Byte(plainTextBytes.Length + saltBytes.Length - 1) {}

        ' Copy plain text bytes into resulting array.
        Dim I As Integer
        For I = 0 To plainTextBytes.Length - 1
            plainTextWithSaltBytes(I) = plainTextBytes(I)
        Next I

        ' Append salt bytes to the resulting array.
        For I = 0 To saltBytes.Length - 1
            plainTextWithSaltBytes(plainTextBytes.Length + I) = saltBytes(I)
        Next I

        ' Because we support multiple hashing algorithms, we must define
        ' hash object AS a common (abstract) base class. We will specify the
        ' actual hashing algorithm class later during object creation.
        Dim hash As HashAlgorithm

        ' Make sure hashing algorithm name is specified.
        If (hashAlgorithm Is Nothing) Then
            hashAlgorithm = ""
        End If

        ' Initialize appropriate hashing algorithm class.
        Select Case hashAlgorithm.ToUpper()

            Case "SHA1"
                hash = New SHA1Managed()

            Case "SHA256"
                hash = New SHA256Managed()

            Case "SHA384"
                hash = New SHA384Managed()

            Case "SHA512"
                hash = New SHA512Managed()

            Case Else
                hash = New MD5CryptoServiceProvider()

        End Select

        ' Compute hash value of our plain text with appended salt.
        Dim hashBytes As Byte()
        hashBytes = hash.ComputeHash(plainTextWithSaltBytes)

        ' Create array which will hold hash and original salt bytes.
        Dim hashWithSaltBytes() As Byte = _
                                   New Byte(hashBytes.Length + _
                                            saltBytes.Length - 1) {}

        ' Copy hash bytes into resulting array.
        For I = 0 To hashBytes.Length - 1
            hashWithSaltBytes(I) = hashBytes(I)
        Next I

        ' Append salt bytes to the result.
        For I = 0 To saltBytes.Length - 1
            hashWithSaltBytes(hashBytes.Length + I) = saltBytes(I)
        Next I

        ' Convert result into a base64-encoded string.
        Dim hashValue As String
        hashValue = Convert.ToBase64String(hashWithSaltBytes)

        ' Return the result.
        Encrypt = hashValue

    End Function


    Public Function EncryptText(ByVal what As String) As String

        Dim encryptSalt As String = "NaCL - Oversight"
        Dim encoding As New System.Text.ASCIIEncoding()

        Return Encrypt(what, "SHA512", encoding.GetBytes(encryptSalt))

    End Function


    '========================================================================================
    ' Time Functions


    Public Function getAppTime() As String

        'Get time HH:mm:SS style from the application

        Return getMySQLTime()

        'Dim tempStr1 As String
        'Dim tempStr2 As String

        'tempStr1 = ""

        'With Date.Now

        '    tempStr2 = .Hour
        '    If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
        '    tempStr1 = tempStr1 & tempStr2 & ":"
        '    tempStr2 = .Minute
        '    If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
        '    tempStr1 = tempStr1 & tempStr2 & ":"
        '    tempStr2 = .Second
        '    If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
        '    tempStr1 &= tempStr2

        'End With

        'Return tempStr1.Trim

    End Function


    Public Function getMachineDate() As String

        'Get date YYYYMMDD style from the application

        Dim tempStr1 As String
        Dim tempStr2 As String
        Dim output As String

        tempStr1 = ""

        With Date.Now

            tempStr1 &= .Year
            tempStr2 = .Month
            If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
            tempStr1 &= tempStr2
            tempStr2 = .Day
            If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
            tempStr1 = tempStr1 & tempStr2 & " "
            tempStr2 = .Hour
            If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
            tempStr1 = tempStr1 & tempStr2 & ":"
            tempStr2 = .Minute
            If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
            tempStr1 = tempStr1 & tempStr2 & ":"
            tempStr2 = .Second
            If Len(tempStr2) = 1 Then tempStr2 = "0" & tempStr2
            tempStr1 &= tempStr2

        End With

        output = Left(tempStr1, 4) & "-" & Right(Left(tempStr1, 6), 2) & "-" & Right(Left(tempStr1, 8), 2) & " "
        Return output.Trim.Replace("-", "").Replace("/", "").Replace("\", "")

    End Function


    Public Function getMySQLDate() As String

        Dim output As String = ""

        output = getSQLQueryAsString(0, "SELECT DATE_FORMAT(now(), '%Y%m%d')")
        'output = output.Replace("-", "").Replace("/", "").Replace("\", "").Substring(0, output.IndexOf(" "))

        Return output

    End Function


    Public Function getMySQLTime() As String

        Dim output As String = ""

        output = getSQLQueryAsString(0, "SELECT DATE_FORMAT(now(), '%T')")
        'output = output.Replace("-", "").Replace("/", "").Replace("\", "").Substring(output.IndexOf(" ")).Trim.Substring(0, output.IndexOf("."))

        Return output

    End Function


    Public Function convertDDdashMMdashYYYYspaceHHcolonMMcolonSStoYYYYdashMMdashDDspaceHHcolonMMcolonSS(ByVal giventext As String) As String

        Dim output As String = ""
        Dim tempYear As String = ""
        Dim tempMonth As String = ""
        Dim tempDay As String = ""

        Dim tempTime As String = ""
        Dim tempHour As String = ""
        Dim tempMin As String = ""
        Dim tempSec As String = ""

        Dim tmpDouble As Double = 0.0

        Dim hasTime As Boolean = False
        Dim is24hrs As Boolean = False
        Dim isAM As Boolean = False

        With giventext

            If .Contains(":") Then

                hasTime = True

                If .Contains("m") Then

                    is24hrs = False

                    giventext = .Trim
                    giventext = .Replace(".000", "").Replace(".00", "").Replace(":", "")

                    If Right(giventext, 5) = " a.m." Then
                        isAM = True
                    End If

                    giventext = giventext.Substring(0, giventext.Length - 5)
                    tempSec = Right(giventext, 2)
                    giventext = giventext.Substring(0, giventext.Length - 2)
                    tempMin = Right(giventext, 2)
                    giventext = giventext.Substring(0, giventext.Length - 2)
                    tempHour = Right(giventext, 2)

                    Try
                        tmpDouble = CDbl(tempHour)
                    Catch ex As Exception

                    End Try

                    If isAM = False Then
                        If tmpDouble <> 12 Then
                            tempHour = tmpDouble + 12
                        End If
                    End If

                    giventext = giventext.Substring(0, giventext.Length - 3) ' 2 + el espacio

                    tempYear = Right(giventext, 4)
                    giventext = giventext.Substring(0, giventext.Length - 4)
                    tempMonth = Right(giventext, 3)
                    tempMonth = tempMonth.Replace("-", "").Replace("/", "")
                    giventext = giventext.Substring(0, giventext.Length - 3)
                    tempDay = giventext.Replace("-", "").Replace("/", "")

                    If Len(tempMonth) = 1 Then tempMonth = "0" & tempMonth

                    If Len(tempDay) = 1 Then tempDay = "0" & tempDay

                    If Len(tempDay) = 0 Then tempDay = tempMonth

                Else

                    giventext = .Trim
                    tempTime = Right(giventext, 8) '9 con el espacio
                    giventext = giventext.Substring(0, giventext.Length - 9)
                    tempYear = Right(giventext, 4)
                    giventext = giventext.Substring(0, giventext.Length - 4)
                    tempMonth = Right(giventext, 3)
                    tempMonth = tempMonth.Replace("-", "").Replace("/", "")
                    giventext = giventext.Substring(0, giventext.Length - 3)
                    tempDay = giventext.Replace("-", "").Replace("/", "")

                    If Len(tempMonth) = 1 Then tempMonth = "0" & tempMonth

                    If Len(tempDay) = 1 Then tempDay = "0" & tempDay

                    If Len(tempDay) = 0 Then tempDay = tempMonth

                End If


            Else

                giventext = .Trim
                tempYear = Right(giventext, 4)
                giventext = giventext.Replace(tempYear, "")
                tempMonth = Right(giventext, 3)
                tempMonth = tempMonth.Replace("-", "").Replace("/", "")
                giventext = giventext.Replace(Right(giventext, 3), "")
                tempDay = giventext.Replace("-", "").Replace("/", "")

                If Len(tempMonth) = 1 Then tempMonth = "0" & tempMonth

                If Len(tempDay) = 1 Then tempDay = "0" & tempDay

                If Len(tempDay) = 0 Then tempDay = tempMonth

            End If

        End With

        If hasTime = True Then

            If is24hrs = False Then
                output = tempYear & "-" & tempMonth & "-" & tempDay & " " & tempHour & ":" & tempMin & ":" & tempSec
            Else
                output = tempYear & "-" & tempMonth & "-" & tempDay & " " & tempTime
            End If

        Else
            output = tempYear & "-" & tempMonth & "-" & tempDay & " " & "00:00:00"
        End If

        Return output

    End Function


    Public Function convertDDdashMMdashYYYYtoYYYYMMDD(ByVal giventext As String) As String

        Dim output As String = ""
        Dim tempYear As String = ""
        Dim tempMonth As String = ""
        Dim tempDay As String = ""

        With giventext

            giventext = giventext.Replace(".000", "").Replace(".00", "")

            If giventext.Contains("m") Then
                giventext = giventext.Substring(0, giventext.Length - 14)
            Else

                If giventext.Contains(":") Then
                    giventext = giventext.Substring(0, giventext.Length - 9)
                End If

            End If

            tempYear &= Right(giventext, 4)
            giventext = giventext.Replace(tempYear, "")
            tempMonth = Right(giventext, 3)
            tempMonth = tempMonth.Replace("-", "").Replace("/", "")
            giventext = giventext.Replace(Right(giventext, 3), "")
            tempDay = giventext.Replace("-", "").Replace("/", "")

            If Len(tempMonth) = 1 Then tempMonth = "0" & tempMonth

            If Len(tempDay) = 1 Then tempDay = "0" & tempDay

        End With

        output = tempYear & tempMonth & tempDay
        Return output

    End Function


    Public Function convertYYYYMMDDtoDDhyphenMMhyphenYYYY(ByVal giventext As String) As String

        Dim output As String = ""
        Dim tempYear As String = ""
        Dim tempMonth As String = ""
        Dim tempDay As String = ""

        With giventext

            tempYear &= Left(giventext, 4)
            giventext = giventext.Replace(tempYear, "")
            tempMonth = Left(giventext, 2)
            giventext = giventext.Substring(2)
            tempDay = giventext

            If Len(tempMonth) = 1 Then tempMonth = "0" & tempMonth

            If Len(tempDay) = 1 Then tempDay = "0" & tempDay

        End With

        output = tempDay & "-" & tempMonth & "-" & tempYear
        Return output

    End Function


    Public Function convertYYYYMMDDtoYYYYhyphenMMhyphenDD(ByVal giventext As String) As String

        Dim output As String = ""
        Dim tempYear As String = ""
        Dim tempMonth As String = ""
        Dim tempDay As String = ""

        With giventext

            tempYear &= Left(giventext, 4)
            giventext = giventext.Replace(tempYear, "")
            tempMonth = Left(giventext, 2)
            giventext = giventext.Substring(2)
            tempDay = giventext

            If Len(tempMonth) = 1 Then tempMonth = "0" & tempMonth

            If Len(tempDay) = 1 Then tempDay = "0" & tempDay

        End With

        output = tempYear & "-" & tempMonth & "-" & tempDay
        Return output

    End Function


    Public Function howManyDaysBetween(ByVal date1 As Date, ByVal date2 As Date) As Integer

        Dim days As Long
        days = DateDiff(DateInterval.Day, date1, date2)
        Return days

    End Function


    '========================================================================================
    ' Convertion Functions


    Public Function Number2Word(ByVal pNum As Double) As String

        Try

            Dim ar1(19) As String
            Dim ar2(10) As String
            Dim ar3(5) As String
            ar1(0) = ""
            ar1(1) = "Uno"
            ar1(2) = "Dos"
            ar1(3) = "Tres"
            ar1(4) = "Cuatro"
            ar1(5) = "Cinco"
            ar1(6) = "Seis"
            ar1(7) = "Siete"
            ar1(8) = "Ocho"
            ar1(9) = "Nueve"
            ar1(10) = "Diez"
            ar1(11) = "Once"
            ar1(12) = "Doce"
            ar1(13) = "Trece"
            ar1(14) = "Catorce"
            ar1(15) = "Quince"
            ar1(16) = "Diez y Seis"
            ar1(17) = "Diez y Siete"
            ar1(18) = "Diez y Ocho"
            ar1(19) = "Diez y Nueve"
            ar2(1) = "Diez"
            ar2(2) = "Veinte"
            ar2(3) = "Treinta"
            ar2(4) = "Cuarenta"
            ar2(5) = "Cincuenta"
            ar2(6) = "Sesenta"
            ar2(7) = "Setenta"
            ar2(8) = "Ochenta"
            ar2(9) = "Noventa"
            ar3(0) = ""
            ar3(1) = "Cien"
            ar3(2) = "Mil"
            ar3(3) = "Cien Mil"
            ar3(4) = "10 Millones"


            Dim tmpNum As Double
            Dim tmpInt As Long
            Dim tmpDml As Integer
            Dim strWord As String = ""
            Dim strWordpart As String
            Dim tmppart As Integer
            Dim lcount As Integer

            tmpNum = pNum
            tmpInt = Int(tmpNum)
            tmpDml = (tmpNum - tmpInt) * 100

            'Integer conversion
            If tmpInt < 0 Or tmpInt > 999999999 Then
                MsgBox("Numero fuera de Rango" & vbCrLf & "Error de Conversion a Texto")
                Return "<<PRECIO EN LETRAS>>"
            End If
            If tmpInt = 0 Then
                strWord = "Cero"
            ElseIf tmpInt < 20 Then
                strWord = ar1(tmpInt)
            Else
                lcount = -1
                Do While tmpInt > 0
                    strWordpart = ""
                    If lcount < 0 Or lcount > 0 Then
                        tmppart = Right(Trim(Str(tmpInt)), 2)
                        If Val(tmpInt) >= 10 Then
                            tmpInt = Val(Left(Trim(Str(tmpInt)), Len(Trim(Str(tmpInt))) - 2))
                        Else
                            tmpInt = Val(Left(Trim(Str(tmpInt)), Len(Trim(Str(tmpInt))) - 1))
                        End If
                    ElseIf lcount = 0 Then
                        tmppart = Right(Trim(Str(tmpInt)), 1)
                        tmpInt = Val(Left(Trim(Str(tmpInt)), Len(Trim(Str(tmpInt))) - 1))
                    End If
                    lcount = lcount + 1
                    If tmppart < 20 Then
                        strWordpart = ar1(tmppart)
                    Else
                        strWordpart = ar2(Int(tmppart / 10)) & " " & ar1(tmppart Mod 10)
                    End If
                    If Not Trim(strWordpart) = "" Then
                        strWord = strWordpart & " " & ar3(lcount) & " " & strWord
                    End If
                Loop
            End If
            strWord = "" & strWord

            ' Decimal part conversion
            ' This will be added to string only if paise part exists.
            strWordpart = ""
            If tmpDml < 20 Then
                strWordpart = ar1(tmpDml)
            Else
                strWordpart = ar2(Int(tmpDml / 10)) & " " & ar1(tmpDml Mod 10)
            End If
            If Not Trim(strWordpart) = "" Then
                strWord = Trim(strWord) & " y " & strWordpart
            Else
                strWord = Trim(strWord)
            End If

            ' Retun final output
            Number2Word = strWord
            Exit Function

        Catch ex As Exception

            MsgBox("Numero fuera de Rango" & vbCrLf & "Error de Conversion a Texto")
            Return "<<PRECIO EN LETRAS>>"

        End Try

    End Function


    Public Function ConvertNumbersToWords(ByVal n1 As String, ByVal paraContrato As Boolean)

        n1 = n1.Replace(",", "")

        If paraContrato = True Then

            Dim tmpWords As String = ""
            tmpWords = ConvertNumber2Words(n1)

            If tmpWords = "" Then
                Return "<<PRECIO EN LETRAS>>"
            Else
                Return tmpWords
            End If

        Else

            Return ConvertNumber2Words(n1)

        End If

    End Function


    Private Function ConvertNumber2Words(ByVal strn1 As String) As String

        Dim substr1 As String
        Dim substr2 As String

        If strn1.IndexOf(".") > 0 Then
            substr1 = strn1.Substring(0, strn1.IndexOf("."))
            substr2 = strn1.Substring(strn1.IndexOf(".") + 1)
        Else
            substr1 = strn1
            substr2 = "00"
        End If

        Dim n1 As Integer = substr1.Length

        Select Case n1

            Case 1

                Return SplitDigits(Int32.Parse(substr1), "Hundreds") & " pesos " & substr2 & "/100 M.N."

            Case 2

                Return SplitDigits(Int32.Parse(substr1), "Hundreds") & " pesos " & substr2 & "/100 M.N."

            Case 3

                Return SplitDigits(Int32.Parse(substr1), "Hundreds") & " pesos " & substr2 & "/100 M.N."

            Case 4

                Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."

            Case 5

                Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."

            Case 6

                Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."

            Case 7

                If Int32.Parse(substr1.Substring(1, n1 - 6)) < 10 Then
                    If Int32.Parse(substr1.Substring(1, n1 - 6)) < 2 Then
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millón " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    Else
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    End If
                Else
                    Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                End If

            Case 8

                If Int32.Parse(substr1.Substring(1, n1 - 6)) < 10 Then
                    If Int32.Parse(substr1.Substring(1, n1 - 6)) < 2 Then
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millón " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    Else
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    End If
                Else
                    Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                End If

            Case 9

                If Int32.Parse(substr1.Substring(1, n1 - 6)) < 10 Then
                    If Int32.Parse(substr1.Substring(1, n1 - 6)) < 2 Then
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millón " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    Else
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    End If
                Else
                    Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 6)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                End If

            Case 10

                If Int32.Parse(substr1.Substring(1, n1 - 9)) < 10 Then
                    If Int32.Parse(substr1.Substring(1, n1 - 9)) < 2 Then
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billón " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    Else
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    End If
                Else
                    Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                End If

            Case 11

                If Int32.Parse(substr1.Substring(1, n1 - 9)) < 10 Then
                    If Int32.Parse(substr1.Substring(1, n1 - 9)) < 2 Then
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billón " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    Else
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    End If
                Else
                    Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                End If

            Case 12

                If Int32.Parse(substr1.Substring(1, n1 - 9)) < 10 Then
                    If Int32.Parse(substr1.Substring(1, n1 - 9)) < 2 Then
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billón " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    Else
                        Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                    End If
                Else
                    Return SplitDigits(Int32.Parse(substr1.Substring(0, n1 - 9)), "Billions") & " Billones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 9, 3)), "Millions") & " Millones " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 6, 3)), "Thousands") & " Mil " & SplitDigits(Int32.Parse(substr1.Substring(n1 - 3, 3)), "Hundreds") & " pesos " & substr2 & "/100 M.N."
                End If

            Case Else

                Return ""

        End Select

    End Function


    Private Function SplitDigits(ByVal n2 As Integer, ByVal convertingUnits As String) As String

        Dim Ones(9) As String
        Ones(0) = ""

        If convertingUnits = "Hundreds" Then
            Ones(1) = "Uno"
        Else
            Ones(1) = "Un"
        End If

        Ones(2) = "Dos"
        Ones(3) = "Tres"
        Ones(4) = "Cuatro"
        Ones(5) = "Cinco"
        Ones(6) = "Seis"
        Ones(7) = "Siete"
        Ones(8) = "Ocho"
        Ones(9) = "Nueve"

        Dim Teens(9) As String
        Teens(0) = "Diez"
        Teens(1) = "Once"
        Teens(2) = "Doce"
        Teens(3) = "Trece"
        Teens(4) = "Catorce"
        Teens(5) = "Quince"
        Teens(6) = "Diez y Seis"
        Teens(7) = "Diez y Siete"
        Teens(8) = "Diez y Ocho"
        Teens(9) = "Diez y Nueve"

        Dim Tys(9) As String
        Tys(0) = ""
        Tys(1) = ""
        Tys(2) = "Veinte"
        Tys(3) = "Treinta"
        Tys(4) = "Cuarenta"
        Tys(5) = "Cincuenta"
        Tys(6) = "Sesenta"
        Tys(7) = "Setenta"
        Tys(8) = "Ochenta"
        Tys(9) = "Noventa"

        Dim Hds(9) As String
        Hds(0) = ""
        Hds(1) = "Cien"
        Hds(2) = "Doscientos"
        Hds(3) = "Trescientos"
        Hds(4) = "Cuatrocientos"
        Hds(5) = "Quinientos"
        Hds(6) = "Seiscientos"
        Hds(7) = "Setecientos"
        Hds(8) = "Ochocientos"
        Hds(9) = "Novecientos"

        Dim strCientos As String = ""
        Dim strDecenas As String = ""

        Dim tmpCientos As Integer
        Dim tmpDecenas As Integer

        Dim tmpDecenasPart1 As Integer
        Dim tmpDecenasPart2 As Integer

        Try

            tmpCientos = Math.Floor(n2 / 100)

            strCientos = Hds(tmpCientos)

            tmpDecenas = Math.Floor(Decimal.Remainder(n2, 100))

            If tmpCientos = 1 And tmpDecenas > 0 Then
                strCientos = "Ciento"
            End If

            If tmpDecenas < 10 Then
                strDecenas = Ones(tmpDecenas)
            ElseIf tmpDecenas > 9 And tmpDecenas < 20 Then

                tmpDecenasPart1 = Math.Floor(tmpDecenas / 10)
                tmpDecenasPart2 = Math.Floor(Decimal.Remainder(tmpDecenas, 10))

                If tmpDecenasPart1 > 0 And tmpDecenasPart2 = 0 Then
                    strDecenas = Teens(0)
                ElseIf tmpDecenasPart2 > 0 Then
                    strDecenas = Teens(tmpDecenasPart2)
                End If

            ElseIf tmpDecenas > 19 Then

                tmpDecenasPart1 = Math.Floor(tmpDecenas / 10)
                tmpDecenasPart2 = Math.Floor(Decimal.Remainder(tmpDecenas, 10))

                If tmpDecenasPart1 > 0 And tmpDecenasPart2 = 0 Then
                    strDecenas = Tys(0)
                ElseIf tmpDecenasPart2 > 0 Then
                    strDecenas = Tys(tmpDecenasPart1) & " y " & Ones(tmpDecenasPart2)
                End If

            End If

            If strCientos = "" And strDecenas = "" Then

                Return "Cero"

            Else

                If strCientos = "" Then
                    Return strDecenas
                Else
                    Return strCientos & " " & strDecenas
                End If

            End If

        Catch ex As Exception

            Return ""

        End Try

    End Function


    '========================================================================================
    ' Security Functions


    Private Function TryPing(ByVal Host As String) As Boolean
        Dim pingSender As New Ping
        Dim reply As PingReply
        Try
            reply = pingSender.Send(Host)
            If reply.Status = IPStatus.Success Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Sub verifySuspiciousData()


        If getSQLQueryAsInteger(0, "SELECT COUNT(pcci.iprojectid) AS conteo FROM projectcardcompoundinputs pcci JOIN projects p ON pcci.iprojectid = p.iprojectid WHERE p.iprojectrealclosingdate > 0 AND STR_TO_DATE(CONCAT(pcci.iupdatedate, ' ', pcci.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectrealclosingdate, ' ', p.sprojectrealclosingtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectCardCompoundInputs)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (ProjectCardCompoundInputs). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(pci.iprojectid) AS conteo FROM projectcardinputs pci JOIN projects p ON pci.iprojectid = p.iprojectid WHERE p.iprojectrealclosingdate > 0 AND STR_TO_DATE(CONCAT(pci.iupdatedate, ' ', pci.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectrealclosingdate, ' ', p.sprojectrealclosingtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectCardInputs)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (ProjectCardInputs). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(pc.iprojectid) AS conteo FROM projectcards pc JOIN projects p ON pc.iprojectid = p.iprojectid WHERE p.iprojectrealclosingdate > 0 AND STR_TO_DATE(CONCAT(pc.iupdatedate, ' ', pc.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectrealclosingdate, ' ', p.sprojectrealclosingtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectCards)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (ProjectCards). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(pic.iprojectid) AS conteo FROM projectindirectcosts pic JOIN projects p ON pic.iprojectid = p.iprojectid WHERE p.iprojectrealclosingdate > 0 AND STR_TO_DATE(CONCAT(pic.iupdatedate, ' ', pic.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectrealclosingdate, ' ', p.sprojectrealclosingtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (ProjectIndirectCosts)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (ProjectIndirectCosts). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(sip.iprojectid) AS conteo FROM supplierinvoiceprojects sip JOIN projects p ON sip.iprojectid = p.iprojectid WHERE p.iprojectrealclosingdate > 0 AND STR_TO_DATE(CONCAT(sip.iupdatedate, ' ', sip.supdatetime), '%Y%c%d %T')  < STR_TO_DATE(CONCAT(p.iprojectrealclosingdate, ' ', p.sprojectrealclosingtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (SupplierInvoiceProjects))', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (SupplierInvoiceProjects). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM supplierinvoiceinputs WHERE dsupplierinvoiceinputtotalprice <= 0 OR dsupplierinvoiceinputunitprice <= 0") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (SupplierInvoiceInputs)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (SupplierInvoiceInputs). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM payments WHERE dpaymentamount <= 0") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (Payments)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (Payments). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

        If getSQLQueryAsInteger(0, "SELECT COUNT(*) AS conteo FROM projects p WHERE p.iprojectrealclosingdate > 0 AND STR_TO_DATE(CONCAT(p.iupdatedate, ' ', p.supdatetime), '%Y%c%d %T')  > STR_TO_DATE(CONCAT(p.iprojectrealclosingdate, ' ', p.sprojectrealclosingtime), '%Y%c%d %T')") > 0 Then

            Dim fecha As Integer = getMySQLDate()
            Dim hora As String = getAppTime()

            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES (" & fecha & ", '" & hora & "', 'SYSTEM', 0, '127.0.0.1', 'Myself', 'Movimiento fuera de tiempo; probable intrusión o encubrimiento (Projects)', 'Detected/Reported')")

            Dim dsUsuariosSysAdmin As DataSet

            dsUsuariosSysAdmin = getSQLQueryAsDataset(0, "SELECT susername FROM userspecialattributes WHERE bsysadmin = 1")

            If dsUsuariosSysAdmin.Tables(0).Rows.Count > 0 Then

                For i = 0 To dsUsuariosSysAdmin.Tables(0).Rows.Count - 1
                    executeSQLCommand(0, "INSERT INTO messages (susername, susersession, smessage, bread, imessagedate, smessagetime, smessagecreatorusername, iupdatedate, supdatetime, supdateusername) VALUES ('" & dsUsuariosSysAdmin.Tables(0).Rows(i).Item(0) & "', 0, 'Ha sido detectado un movimiento sospechoso (Projects). ¿Podrías revisarlo?', 0, " & fecha & ", '" & hora & "', 'SYSTEM', " & fecha & ", '" & hora & "', 'SYSTEM')")
                Next i

            End If

        End If

    End Sub


    Public Sub closeTimedOutConnections()

        Dim fecha As Integer = getMySQLDate()
        Dim hora As String = getAppTime()

        Dim MinutesSinceLastRun As Integer = getSQLQueryAsInteger(0, "" & _
        "SELECT " & _
        "MINUTE(TIMEDIFF(STR_TO_DATE(CONCAT(" & fecha & ", ' ', '" & hora & "'), '%Y%c%d %T'), STR_TO_DATE(CONCAT(l.iupdatedate, ' ', l.supdatetime), '%Y%c%d %T'))) " & _
        "FROM ( " & _
        "SELECT * FROM (SELECT * FROM longquerieslogs WHERE squeryname = 'Timeout Query' ORDER BY iupdatedate DESC, supdatetime DESC) l GROUP BY squeryname) l ")

        If MinutesSinceLastRun < 15 Then
            Exit Sub
        End If

        Dim queryTimedOutConnections As String = ""

        Dim dsTimedOutConnections As DataSet

        queryTimedOutConnections = "" & _
        "SELECT l.supdateusername, l.susersession, l.iupdatedate, l.supdatetime, s.ilogindate, s.slogintime " & _
        "FROM sessions s " & _
        "JOIN (SELECT iupdatedate, supdatetime, supdateusername, susersession, MINUTE(TIMEDIFF(STR_TO_DATE(CONCAT(" & fecha & ", ' ', '" & hora & "'), '%Y%c%d %T'), STR_TO_DATE(CONCAT(l.iupdatedate, ' ', l.supdatetime), '%Y%c%d %T'))) FROM (SELECT * FROM (SELECT * FROM logs ORDER BY iupdatedate DESC, supdatetime DESC) l GROUP BY supdateusername, susersession ORDER BY iupdatedate DESC, supdatetime DESC) l WHERE MINUTE(TIMEDIFF(STR_TO_DATE(CONCAT(" & fecha & ", ' ', '" & hora & "'), '%Y%c%d %T'), STR_TO_DATE(CONCAT(l.iupdatedate, ' ', l.supdatetime), '%Y%c%d %T'))) > 30 ORDER BY iupdatedate DESC, supdatetime DESC, supdateusername ASC, CONVERT(susersession, DECIMAL) ASC) l ON s.susername = l.supdateusername AND s.susersession = l.susersession " & _
        "AND s.bloggedinsuccesfully = 1 AND s.ilogoutdate IS NULL AND l.supdateusername <> 'SYSTEM' " & _
        "GROUP BY s.susername, s.susersession " & _
        "ORDER BY l.iupdatedate DESC, l.supdatetime DESC "

        dsTimedOutConnections = getSQLQueryAsDataset(0, queryTimedOutConnections)

        If dsTimedOutConnections.Tables(0).Rows.Count > 0 Then

            Dim queriesLogout(3) As String

            For i = 0 To dsTimedOutConnections.Tables(0).Rows.Count - 1

                If getSQLQueryAsInteger(0, "SELECT COUNT(*) FROM sessions WHERE susername = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("supdateusername") & "' AND ilogoutdate IS NULL") = 1 Then
                    queriesLogout(0) = "UPDATE users SET bonline = 0 WHERE susername = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("supdateusername") & "'"
                End If

                queriesLogout(1) = "UPDATE sessions SET btimedout = 1, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("supdateusername") & "' AND susersession = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("susersession") & "'  AND ilogindate = " & dsTimedOutConnections.Tables(0).Rows(i).Item("ilogindate") & "  AND slogintime = '" & dsTimedOutConnections.Tables(0).Rows(i).Item("slogintime") & "'"

                queriesLogout(2) = "INSERT IGNORE INTO longquerieslogs VALUES (" & fecha & ", '" & hora & "', 'Timeout Query') "

                executeTransactedSQLCommand(0, queriesLogout)

            Next i

        Else

            executeSQLCommand(0, "INSERT IGNORE INTO longquerieslogs VALUES (" & fecha & ", '" & hora & "', 'Timeout Query') ")

        End If

    End Sub


    Public Function verifyIfUserHashIsCorrect(ByVal datehash As String, ByVal emailhash As String) As Boolean

        If EncryptText(getMySQLDate()) <> datehash Then Return ""

        If getSQLQueryAsBoolean(0, "SELECT sresethash FROM resets where sresethash = '" & datehash & "'") = True Then Return ""

        Dim dsUsers As DataSet = getSQLQueryAsDataset(0, "SELECT susername, suseremail FROM users")

        For i As Integer = 0 To dsUsers.Tables(0).Rows.Count - 1

            If EncryptText(dsUsers.Tables(0).Rows(i).Item("suseremail")) = emailhash Then
                Return dsUsers.Tables(0).Rows(i).Item("susername")
            End If

        Next i

        Return ""

    End Function


    'Public Function verifyIfUserIsReallyLogged(ByVal page As System.Web.UI.Page) As Boolean

    '    Dim dsUserData As DataSet

    '    If page.Session("username") = "" Or page.Session("username") = "Anonymous" Then
    '        Exit Function
    '    ElseIf page.Session("username") <> "" And page.Session("username") <> "Anonymous" And (page.Session("realName") = "" Or page.Session("nickname") = "" Or page.Session("desiredName") = "" Or page.Session("email") = "" Or page.Session("preferredLanguage") = "" Or CStr(page.Session("employee?")) = "" Or CStr(page.Session("approved?")) = "" Or CStr(page.Session("lockedout?")) = "" Or CStr(page.Session("online?")) = "" Or CStr(page.Session("userlevel")) = "" Or page.Session("parentlogin") = "") Then
    '        cleanSession(page)
    '        Exit Function
    '    Else
    '        dsUserData = getSQLQueryAsDataset(0, "SELECT susername, CONCAT(suserfullname, ' ', suserlastname) AS suserrealname, susernickname, suserdesiredname, suseremail, suserpreferredlanguage, bemployee, bapproved, blockedout, bonline, iuserlevel, suserprofiledept, suserparentlogin FROM users u JOIN userprofiles up ON u.ilastuserprofileidapplied = up.iuserprofileid WHERE susername = '" & page.Session("username") & "' LIMIT 1")
    '    End If

    '    If page.Session("username") <> "" And page.Session("username") <> "Anonymous" And (page.Session("realName") <> dsUserData.Tables(0).Rows(0).Item("suserrealname") Or page.Session("nickname") <> dsUserData.Tables(0).Rows(0).Item("susernickname") Or page.Session("desiredName") <> dsUserData.Tables(0).Rows(0).Item("suserdesiredname") Or page.Session("email") <> dsUserData.Tables(0).Rows(0).Item("suseremail") Or CStr(page.Session("employee?")) <> dsUserData.Tables(0).Rows(0).Item("bemployee") Or CStr(page.Session("approved?")) <> dsUserData.Tables(0).Rows(0).Item("bapproved") Or CStr(page.Session("lockedout?")) <> dsUserData.Tables(0).Rows(0).Item("blockedout") Or CStr(page.Session("userlevel")) <> dsUserData.Tables(0).Rows(0).Item("iuserlevel") Or page.Session("parentlogin") <> dsUserData.Tables(0).Rows(0).Item("suserparentlogin") Or page.Session("userdepartment") <> dsUserData.Tables(0).Rows(0).Item("suserprofiledept")) Then
    '        cleanSession(page)
    '    Else
    '        Return True
    '    End If

    'End Function


    'Public Function createUserSessionVariables(ByVal page As System.Web.UI.Page, ByVal who As String) As Boolean

    '    Dim dsUserData As DataSet
    '    dsUserData = getSQLQueryAsDataset(0, "SELECT susername, CONCAT(suserfullname, ' ', suserlastname) AS suserrealname, susernickname, suserdesiredname, suseremail, suserpreferredlanguage, bemployee, bapproved, blockedout, bonline, iuserlevel, suserprofiledept, suserparentlogin FROM users u JOIN userprofiles up ON u.ilastuserprofileidapplied = up.iuserprofileid WHERE susername = '" & who & "' LIMIT 1")
    '    page.Session("username") = dsUserData.Tables(0).Rows(0).Item("susername")
    '    page.Session("realName") = dsUserData.Tables(0).Rows(0).Item("suserrealname")
    '    page.Session("nickname") = dsUserData.Tables(0).Rows(0).Item("susernickname")
    '    page.Session("desiredName") = dsUserData.Tables(0).Rows(0).Item("suserdesiredname")
    '    page.Session("email") = dsUserData.Tables(0).Rows(0).Item("suseremail")
    '    page.Session("preferredLanguage") = dsUserData.Tables(0).Rows(0).Item("suserpreferredlanguage")
    '    page.Session("employee?") = dsUserData.Tables(0).Rows(0).Item("bemployee")
    '    page.Session("approved?") = dsUserData.Tables(0).Rows(0).Item("bapproved")
    '    page.Session("lockedout?") = dsUserData.Tables(0).Rows(0).Item("blockedout")
    '    page.Session("online?") = dsUserData.Tables(0).Rows(0).Item("bonline")
    '    page.Session("userlevel") = dsUserData.Tables(0).Rows(0).Item("iuserlevel")
    '    page.Session("userdepartment") = dsUserData.Tables(0).Rows(0).Item("suserprofiledept")
    '    page.Session("parentlogin") = dsUserData.Tables(0).Rows(0).Item("suserparentlogin")

    '    Return True

    'End Function


    'Public Sub cleanSession(ByVal page As System.Web.UI.Page)

    '    page.Session("username") = "Anonymous"
    '    page.Session("realName") = ""
    '    page.Session("nickname") = ""
    '    page.Session("desiredName") = ""
    '    page.Session("email") = ""
    '    page.Session("preferredLanguage") = "esmx"
    '    page.Session("employee?") = ""
    '    page.Session("approved?") = ""
    '    page.Session("lockedout?") = ""
    '    page.Session("online?") = ""
    '    page.Session("userlevel") = ""
    '    page.Session("parentlogin") = ""

    'End Sub


    'Public Function createNewSessionID() As Boolean

    '    Try
    '        Dim Manager As New SessionState.SessionIDManager()
    '        Dim NewID As String = Manager.CreateSessionID(HttpContext.Current)
    '        Dim OldID As String = HttpContext.Current.Session.SessionID
    '        Dim IsAdded As Boolean = True
    '        Manager.SaveSessionID(HttpContext.Current, NewID, False, IsAdded)
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try

    'End Function


    'Public Function removeLockOutFromUser(ByVal origen As System.Web.UI.Page, ByVal who As String) As Boolean

    '    Dim fecha As Integer = 0
    '    Dim hora As String = "00:00:00"

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & who & "','" & origen.Session.SessionID & "', 'Login Permises Granted again after Lock Out','127.0.0.1','1'," & fecha & ",'" & hora & "')")
    '    Return executeSQLCommand(0, "UPDATE users SET blockedout = 0 WHERE susername = '" & who & "'")

    'End Function


    'Public Function removeLockOutFromUsers(ByVal origen As System.Web.UI.Page)

    '    Dim fecha As Integer = 0
    '    Dim hora As String = "00:00:00"

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    Dim dsLockedUsers As DataSet
    '    dsLockedUsers = getSQLQueryAsDataset(0, "SELECT s.susername FROM sessions s JOIN users u ON s.susername = u.susername WHERE TIMEDIFF(CONCAT(left(ilogindate,4), '-', right(left(ilogindate,6),2), '-', right(ilogindate,2), ' ', slogintime), now()) > '-00:10:00' AND u.blockedout = 1")

    '    For i As Integer = 0 To dsLockedUsers.Tables(0).Rows.Count - 1
    '        executeSQLCommand(0, "UPDATE users SET blockedout = 0 WHERE susername = '" & dsLockedUsers.Tables(0).Rows(i).Item(0) & "'")
    '        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & dsLockedUsers.Tables(0).Rows(i).Item(0) & "', '" & origen.Session.SessionID & "', 'Login Permises Granted again after Lock Out','127.0.0.1','1'," & fecha & ",'" & hora & "')")
    '    Next

    '    Return True

    'End Function


    'Public Function verifyIfItIsHackingAttempts(ByVal origen As System.Web.UI.Page) As Boolean

    '    Dim fecha As Integer = 0
    '    Dim hora As String = "00:00:00"

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    Dim dsCountUsersBeingHacked As DataSet
    '    dsCountUsersBeingHacked = getSQLQueryAsDataset(0, "SELECT susername, susersession, count(susername) AS conteo FROM sessions WHERE TIMEDIFF(CONCAT(left(ilogindate,4), '-', right(left(ilogindate,6),2), '-', right(ilogindate,2), ' ', slogintime), now()) < '-00:10:00' AND ilogoutdate is null AND slogouttime is null GROUP BY susername")
    '    Dim dsUsersSessions As DataSet
    '    Dim dsITEmployees As DataSet
    '    dsITEmployees = getSQLQueryAsDataset(0, "SELECT DISTINCT susername FROM users u JOIN userprofiles up ON u.ilastuserprofileidapplied = up.iuserprofileid WHERE up.suserprofiledept = 'IT'")

    '    For i As Integer = 0 To dsCountUsersBeingHacked.Tables(0).Rows.Count - 1

    '        If dsCountUsersBeingHacked.Tables(0).Rows(i).Item(2) > 5 Then

    '            executeSQLCommand(0, "UPDATE users SET blockedout = 1 WHERE susername = '" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "'")
    '            Dim strRealLoggedUserSession As String = getSQLQueryAsString(0, "SELECT s.susersession FROM sessions s JOIN logs l ON s.susername = l.susername AND s.ilogindate = l.ilogdate AND s.slogintime = l.slogtime WHERE s.bloggedinsuccesfully = 1 AND s.susername = '" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "'")
    '            dsUsersSessions = getSQLQueryAsDataset(0, "SELECT susername, susersession FROM sessions WHERE TIMEDIFF(CONCAT(left(ilogindate,4), '-', right(left(ilogindate,6),2), '-', right(ilogindate,2), ' ', slogintime), now()) < '-00:10:00' AND ilogoutdate is null AND slogouttime is null AND susername = '" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "'")

    '            For j As Integer = 0 To dsUsersSessions.Tables(0).Rows.Count - 1

    '                If dsUsersSessions.Tables(0).Rows(j).Item(1) = strRealLoggedUserSession Then
    '                    executeSQLCommand(0, "UPDATE sessions SET blockedout = 1, bkickedout = 0, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "' AND susersession = '" & strRealLoggedUserSession & "'")
    '                    executeSQLCommand(0, "INSERT INTO usernotices (susername, susersession, susernoticereason, bseen, ieventdate, seventtime) VALUES('" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "','" & strRealLoggedUserSession & "', 'Account is being hacked AS we speak', '0'," & fecha & ",'" & hora & "')")
    '                    If dsITEmployees.Tables(0).Rows.Count > 0 Then
    '                        For k As Integer = 0 To dsITEmployees.Tables(0).Rows.Count - 1
    '                            executeSQLCommand(0, "INSERT INTO usernotices (susername, susersession, susernoticereason, bseen, ieventdate, seventtime) VALUES('" & dsITEmployees.Tables(0).Rows(k).Item(0) & "','" & strRealLoggedUserSession & "', 'User account is being hacked!', '0'," & fecha & ",'" & hora & "')")
    '                        Next
    '                    End If
    '                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "', '" & origen.Session.SessionID & "', 'Locked out preventively - His account was being hacked. Real (Valid) User Notified (Session : " & strRealLoggedUserSession & ")','127.0.0.1','1'," & fecha & ",'" & hora & "')")
    '                Else
    '                    executeSQLCommand(0, "UPDATE sessions SET bkickedout = 1, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "' AND susersession = '" & dsUsersSessions.Tables(0).Rows(j).Item(0) & "'")
    '                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & dsCountUsersBeingHacked.Tables(0).Rows(i).Item(0) & "', '" & origen.Session.SessionID & "', 'Kicked out for attempting to hack the user account','127.0.0.1','1'," & fecha & ",'" & hora & "')")
    '                End If

    '            Next j

    '        End If

    '    Next i

    'End Function


    'Public Function alertITIfRepeatedAttempts(ByVal origen As System.Web.UI.Page) As Boolean

    '    Dim fecha As Integer = 0
    '    Dim hora As String = "00:00:00"

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    Dim dsCountHackingUsers As DataSet
    '    dsCountHackingUsers = getSQLQueryAsDataset(0, "SELECT susername, susersession, count(susername) AS conteo FROM logs WHERE sactiondone like 'Unauthorized Access Attempt%' AND sresult = '0' AND TIMEDIFF(CONCAT(left(ilogdate,4), '-', right(left(ilogdate,6),2), '-', right(ilogdate,2), ' ', slogtime), now()) < '-00:10:00' GROUP BY susername")
    '    Dim dsUsersSessions As DataSet
    '    Dim dsITEmployees As DataSet
    '    dsITEmployees = getSQLQueryAsDataset(0, "SELECT DISTINCT susername FROM users u JOIN userprofiles up ON u.ilastuserprofileidapplied = up.iuserprofileid WHERE up.suserprofiledept = 'IT'")

    '    For i As Integer = 0 To dsCountHackingUsers.Tables(0).Rows.Count - 1

    '        If dsCountHackingUsers.Tables(0).Rows(i).Item(2) > 5 Then

    '            executeSQLCommand(0, "UPDATE users SET blockedout = 1 WHERE susername = '" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "'")
    '            Dim strRealLoggedUserSession As String = getSQLQueryAsString(0, "SELECT s.susersession FROM sessions s JOIN logs l ON s.susername = l.susername AND s.ilogindate = l.ilogdate AND s.slogintime = l.slogtime WHERE s.bloggedinsuccesfully = 1 AND s.susername = '" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "'")
    '            dsUsersSessions = getSQLQueryAsDataset(0, "SELECT susername, susersession FROM sessions WHERE TIMEDIFF(CONCAT(left(ilogindate,4), '-', right(left(ilogindate,6),2), '-', right(ilogindate,2), ' ', slogintime), now()) < '-00:10:00' AND ilogoutdate is null AND slogouttime is null AND susername = '" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "'")

    '            For j As Integer = 0 To dsUsersSessions.Tables(0).Rows.Count - 1

    '                If dsUsersSessions.Tables(0).Rows(j).Item(1) = strRealLoggedUserSession Then
    '                    executeSQLCommand(0, "UPDATE sessions SET blockedout = 1, bkickedout = 0, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "' AND susersession = '" & strRealLoggedUserSession & "'")
    '                    executeSQLCommand(0, "INSERT INTO usernotices (susername, susersession, susernoticereason, bseen, ieventdate, seventtime) VALUES('" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "','" & strRealLoggedUserSession & "', 'Account is being hacked AS we speak', '0'," & fecha & ",'" & hora & "')")
    '                    If dsITEmployees.Tables(0).Rows.Count > 0 Then
    '                        For k As Integer = 0 To dsITEmployees.Tables(0).Rows.Count - 1
    '                            executeSQLCommand(0, "INSERT INTO usernotices (susername, susersession, susernoticereason, bseen, ieventdate, seventtime) VALUES('" & dsITEmployees.Tables(0).Rows(k).Item(0) & "','" & strRealLoggedUserSession & "', 'User account is being hacked!', '0'," & fecha & ",'" & hora & "')")
    '                        Next
    '                    End If
    '                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "', '" & origen.Session.SessionID & "', 'Locked out preventively - His account was being hacked. Real (Valid) User Notified (Session : " & strRealLoggedUserSession & ")','127.0.0.1','1'," & fecha & ",'" & hora & "')")
    '                Else
    '                    executeSQLCommand(0, "UPDATE sessions SET bkickedout = 1, ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "' AND susersession = '" & dsUsersSessions.Tables(0).Rows(j).Item(0) & "'")
    '                    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & dsCountHackingUsers.Tables(0).Rows(i).Item(0) & "', '" & origen.Session.SessionID & "', 'Kicked out for attempting to hack the user account','127.0.0.1','1'," & fecha & ",'" & hora & "')")
    '                End If

    '            Next j

    '        End If

    '    Next i

    'End Function.


    'Public Function ActivateDeactivateSystem(ByVal who As String) As Boolean

    '    Dim dsLang As DataSet
    '    Dim status As Integer
    '    Dim fecha As Integer = getMySQLDate()
    '    Dim hora As String = getAppTime()

    '    status = getSQLQueryAsInteger(0, "SELECT bactive FROM modules WHERE smoduleid = 'SystemStatus'")

    '    If status = 1 Then
    '        status = 0
    '    Else
    '        status = 1
    '    End If

    '    executeSQLCommand(0, "UPDATE modules SET bactive = " & status & ", sgrantingusername = '" & who & "', igrantingdate = " & fecha & ", sgrantingtime = '" & hora & "' WHERE smoduleid = 'SystemStatus'")

    '    dsLang = getSQLQueryAsDataset(0, "SELECT slangid FROM languages")

    '    For i = 1 To dsLang.Tables(0).Rows.Count
    '        executeSQLCommand(0, "UPDATE labels SET " & dsLang.Tables(0).Rows(i).Item(0) & " = CONCAT(" & dsLang.Tables(0).Rows(i).Item(0) & ", '" & fecha & " " & hora & " CST') WHERE spageid = 'default.aspx' AND slabelid = 'lblSystemOut'")
    '    Next

    '    Return True

    'End Function


    'Public Function switchModuleStatus(ByVal who As String, ByVal smoduleid As String) As Boolean

    '    Dim status As Integer
    '    Dim fecha As Integer = getMySQLDate()
    '    Dim hora As String = getAppTime()

    '    status = getSQLQueryAsInteger(0, "SELECT bactive FROM modules WHERE smoduleid = '" & smoduleid & "'")

    '    If status = 1 Then
    '        status = 0
    '    Else
    '        status = 1
    '    End If

    '    executeSQLCommand(0, "UPDATE modules SET bactive = " & status & ", sgrantingusername = '" & who & "', igrantingdate = " & fecha & ", sgrantingtime = '" & hora & "' WHERE smoduleid = '" & smoduleid & "'")

    'End Function


    '========================================================================================
    ' Licensing Functions


    Private Function GetCPUSerialNo(ByVal eax As Long, ByVal edx As Long) As String

        GetCPUSerialNo = Right("00000000" & Hex(edx), 8) & "-" & Right("00000000" & Hex(eax), 8)

    End Function


    Private Function CPUAsm() As String

        If m_CPUAsm = "" Then

            Dim Asm As String = ""
            Asm = Asm & Chr(&H56)                            '56        push   esi
            Asm = Asm & Chr(&H55)                            '55        push   ebp
            Asm = Asm & Chr(&H8B) & Chr(&HEC)                '8B EC     mov  ebp,esp
            Asm = Asm & Chr(&H8B) & Chr(&H75) & Chr(&HC)     '8B 75 0C  mov  esi,dword ptr [ebp+0Ch]
            Asm = Asm & Chr(&H8B) & Chr(&H6)                 '8B 06     mov  eax,dword ptr [esi]
            Asm = Asm & Chr(&HF) & Chr(&HA2)                 '0F A2     cpuid
            Asm = Asm & Chr(&H8B) & Chr(&H75) & Chr(&HC)     '8B 75 0C  mov  esi,dword ptr [ebp+0Ch]
            Asm = Asm & Chr(&H89) & Chr(&H6)                 '89 06     mov  dword ptr [esi],eax
            Asm = Asm & Chr(&H8B) & Chr(&H75) & Chr(&H10)    '8B 75 10  mov  esi,dword ptr [ebp+10h]
            Asm = Asm & Chr(&H89) & Chr(&H1E)                '89 1E     mov  dword ptr [esi],ebx
            Asm = Asm & Chr(&H8B) & Chr(&H75) & Chr(&H14)    '8B 75 14  mov  esi,dword ptr [ebp+14h]
            Asm = Asm & Chr(&H89) & Chr(&HE)                 '89 0E     mov  dword ptr [esi],ecx
            Asm = Asm & Chr(&H8B) & Chr(&H75) & Chr(&H18)    '8B 75 18  mov  esi,dword ptr [ebp+18h]
            Asm = Asm & Chr(&H89) & Chr(&H16)                '89 16     mov  dword ptr [esi],edx
            Asm = Asm & Chr(&H5D)                            '5D        pop  ebp
            Asm = Asm & Chr(&H5E)                            '5E        pop  esi
            Asm = Asm & Chr(&HC2) & Chr(&H10) & Chr(&H0)     'C2 10 00  ret  10h
            m_CPUAsm = Asm

        End If

        CPUAsm = m_CPUAsm

    End Function


    Public Function CPUSerial() As String

        eax = 0

        CallWindowProc(CPUAsm, eax, ebx, ecx, edx)

        If eax > 0 Then

            eax = 1
            CallWindowProc(CPUAsm, eax, ebx, ecx, edx)
            Return GetCPUSerialNo(eax, edx)

        End If

        Return ""

    End Function


    Public Function verifyLicense(ByVal AtStart As Boolean, ByVal silent As Boolean) As Boolean

        If getSQLQueryAsString(0, "SELECT slicense FROM companyinfo") = EncryptText(getSQLQueryAsString(0, "SELECT CONCAT(" & CPUSerial() & ", " & getMySQLDate() & ", " & getMySQLDate() + 10000 & ") AS savedsubscription FROM companyinfo")) Then

            Return True

        Else

            If silent = False Then

                Dim asus As New AgregarLicencia

                asus.AtStart = AtStart

                asus.Show()

                If asus.DialogResult = DialogResult.OK Then
                    Return True
                Else
                    Return False
                End If

            Else

                Return False

            End If

        End If

    End Function


    '========================================================================================
    ' Logger Functions


    'Public Function logLogin(ByVal origen As System.Web.UI.Page, ByVal username As String, ByVal ip As String, ByVal dbUsername As String)

    '    Dim lok As String = 0
    '    Dim resultOK As Boolean = False

    '    '-4 = User not existent, wrong password
    '    '-3 = Inactive
    '    '-2 = Not approved
    '    '-1 = Locked out
    '    '0 = Already Logged In
    '    'Else = Username

    '    If dbUsername.Equals("-4") = True Then
    '        resultOK = False
    '    ElseIf dbUsername.Equals("-3") = True Then
    '        resultOK = False
    '    ElseIf dbUsername.Equals("-2") = True Then
    '        resultOK = False
    '    ElseIf dbUsername.Equals("-1") = True Then
    '        resultOK = False
    '    ElseIf dbUsername.Equals("0") = True Then
    '        resultOK = False
    '    Else
    '        resultOK = True
    '    End If

    '    If resultOK Then lok = "1"
    '    If Not resultOK Then lok = "0"

    '    username = username.Replace("'", "").Replace("\", "").Replace("--", "")

    '    Dim pedazos() As String = origen.Request.FilePath.Split("/")
    '    Dim page As String = pedazos(pedazos.Length - 1)

    '    Dim fecha As Integer = 0
    '    Dim hora As String = "00:00:00"

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    'Log the Login Attempt
    '    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & username & "','" & origen.Session.SessionID & "', 'Login Attempt from Page " & page & " - Result : " & resultOK & "','" & ip & "','" & lok & "'," & fecha & ",'" & hora & "')")

    '    If resultOK Then

    '        'Log the sucessful login session
    '        executeSQLCommand(0, "INSERT INTO sessions VALUES('" & username & "','" & origen.Session.SessionID & "',1,0,0,0,'" & ip & "'," & fecha & ",'" & hora & "', NULL, NULL)")
    '        executeSQLCommand(0, "UPDATE users SET bonline = '1' WHERE susername = '" & username & "'")

    '        'Logout idle users
    '        logoutIdleUsers(origen)

    '        'Verify if there are any user locked out that need their permises granted again
    '        removeLockOutFromUsers(origen)

    '        Return True

    '    Else
    '        'Log the unsucessful login session
    '        executeSQLCommand(0, "INSERT INTO sessions VALUES('" & username & "','" & origen.Session.SessionID & "',0,0,0,0,'" & ip & "'," & fecha & ",'" & hora & "', NULL, NULL)")

    '        'Check if someone is trying to hack the account
    '        verifyIfItIsHackingAttempts(origen)

    '        'Logout idle users
    '        logoutIdleUsers(origen)

    '        'Verify if there are any user locked out that need their permises granted again
    '        removeLockOutFromUsers(origen)

    '        Return False

    '    End If

    'End Function


    'Public Function logLogout(ByVal origen As System.Web.UI.Page, ByVal username As String, ByVal ip As String) As Boolean

    '    'Logout current user

    '    Dim pedazos() As String = origen.Request.FilePath.Split("/")
    '    Dim page As String = pedazos(pedazos.Length - 1)

    '    Dim fecha As Integer = 0
    '    Dim hora As String = "00:00:00"

    '    fecha = getMySQLDate()
    '    hora = getAppTime()

    '    executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & username & "','" & origen.Session.SessionID & "', 'Logout from Page " & page & "','" & ip & "','1'," & fecha & ",'" & hora & "')")

    '    executeSQLCommand(0, "UPDATE sessions SET ilogoutdate = " & fecha & ", slogouttime = '" & hora & "' WHERE susername = '" & username & "' AND susersession = '" & origen.Session.SessionID & "'")

    '    executeSQLCommand(0, "UPDATE users SET bonline = '0' WHERE susername = '" & username & "'")

    '    'Logout idle users
    '    logoutIdleUsers(origen)

    '    'Verify if there are any user locked out that need their permises granted again
    '    removeLockOutFromUsers(origen)


    'End Function


    'Public Function logRescueAttempt(ByVal origen As System.Web.UI.Page, ByVal email As String, ByVal ip As String, ByVal rescueanswer As String, ByVal providedanswer As String) As Boolean

    '    email = email.Replace("'", "").Replace("--", "").Trim

    '    Dim pedazos() As String = origen.Request.FilePath.Split("/")
    '    Dim page As String = pedazos(pedazos.Length - 1)

    '    Dim fecha As Integer = getMySQLDate()
    '    Dim hora As String = getAppTime()

    '    Dim savedAnswer As String = EncryptText(providedanswer)

    '    If rescueanswer <> savedAnswer Then
    '        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & origen.Session("username") & "','" & origen.Session.SessionID & "', 'Rescue Attempt from Page " & page & " - Result : False ','" & ip & "','0'," & fecha & ",'" & hora & "')")
    '        Return False
    '    Else
    '        executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & origen.Session("username") & "','" & origen.Session.SessionID & "', 'Rescue Attempt from Page " & page & " - Result : True ','" & ip & "','1'," & fecha & ",'" & hora & "')")
    '        Dim sentEmail As Boolean = False
    '        sentEmail = sendPlainMail(email, getMessage(origen, "LostPasswordEmailSubject"), getMessage(origen, "LostPasswordEmailBody") & savedAnswer)
    '        If sentEmail = True Then
    '            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & origen.Session("username") & "','" & origen.Session.SessionID & "', 'Rescue Email Sent to " & email & "','" & ip & "','1'," & fecha & ",'" & hora & "')")
    '            Return True
    '        Else
    '            executeSQLCommand(0, "INSERT IGNORE INTO logs VALUES('" & origen.Session("username") & "','" & origen.Session.SessionID & "', 'Rescue Email Sent to " & email & "','" & ip & "','0'," & fecha & ",'" & hora & "')")
    '            Return False
    '        End If
    '    End If

    'End Function



    '========================================================================================
    ' Mail Functions


    Public Function sendPlainMail(ByVal toEmail As String, ByVal subject As String, ByVal body As String) As Boolean

        Try

            Dim email As New MailMessage()
            email.[To].Add(toEmail)
            Dim maFrom As New MailAddress("support@riodorado.com")
            email.From = maFrom
            email.Body = body
            email.IsBodyHtml = False
            email.Subject = subject

            Dim smtp As New SmtpClient("smtp.gmail.com", 587)
            'or 25
            smtp.EnableSsl = True
            smtp.Credentials = New System.Net.NetworkCredential("support@riodorado.com", "Supp0rt")
            smtp.Send(email)

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function


    Public Function sendHTMLMail(ByVal toEmail As String, ByVal subject As String, ByVal body As String) As Boolean

        Try

            Dim email As New MailMessage()
            email.[To].Add(toEmail)
            Dim maFrom As New MailAddress("support@riodorado.com")
            email.From = maFrom
            email.Body = body
            email.IsBodyHtml = True
            email.Subject = subject

            Dim smtp As New SmtpClient("smtp.gmail.com", 587)
            'or 25
            smtp.EnableSsl = True
            smtp.Credentials = New System.Net.NetworkCredential("support@riodorado.com", "Supp0rt")
            smtp.Send(email)

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function


End Module