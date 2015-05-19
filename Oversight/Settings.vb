Imports System.Xml.Serialization
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection

Public NotInheritable Class Settings

    Public dbLocationIPOrMachineName As String
    Public rutaArchivos As String
    Public rutaActualizacion As String
    Public fecha As String
    Public hora As String
    Public Name As String
    
    'private static readonly string file;
    Private Shared ReadOnly isoFileName As String


    Shared Sub New()
        'file = Assembly.GetExecutingAssembly().Location + ".xml";
        isoFileName = "OversightSettings.xml"
    End Sub


    Public Sub New()
        Name = Assembly.GetExecutingAssembly().GetName().Name
        rutaArchivos = "C:\"
        rutaActualizacion = "C:\Actualizaciones\"
    End Sub


    Public Shared Function Reset() As Settings

        Dim mac As New Settings()
        Settings.SetAppConfig(mac)
        Return mac

    End Function


    Public Shared Function GetAssemConfig() As Settings

        Dim isoStore As IsolatedStorageFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly Or IsolatedStorageScope.User, Nothing, Nothing)

        If Not Settings.ISOFileExists(isoStore, isoFileName) Then
            Return New Settings()
        End If

        Dim xml As String = Settings.ReadFromISOFile(isoStore, isoFileName)

        Try
            Dim mac As Settings = Settings.FromXmlString(xml)
            Return mac
        Catch
            ' Xml not valid - probably corrupted. Rewrite it with defaults.
            Return Settings.Reset()
        End Try

    End Function


    Public Shared Sub SetAppConfig(ByVal appConfig As Settings)

        If appConfig Is Nothing Then
            Throw New ArgumentNullException("appConfig")
        End If

        Dim xml As String = appConfig.ToXmlString()

        Dim isoStore As IsolatedStorageFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly Or IsolatedStorageScope.User, Nothing, Nothing)

        Settings.WriteToISOFile(isoStore, isoFileName, xml)

    End Sub


    ' public static MyAppConfig Load()
    ' {
    ' using(StreamReader sr = new StreamReader(MyAppConfig.file))
    ' {
    ' string xml = sr.ReadToEnd();
    ' MyAppConfig mac = MyAppConfig.FromXmlString(xml);
    ' return mac;
    ' }
    ' }

    ' public void Save()
    ' {
    ' string myXml = this.ToXmlString();
    ' using(StreamWriter sw = new StreamWriter(MyAppConfig.file))
    ' {
    ' sw.Write(myXml);
    ' }
    ' }

    Public Function ToXmlString() As String

        Dim data As String = Nothing
        Dim ser As New XmlSerializer(GetType(Settings))

        Using sw As New StringWriter()

            ser.Serialize(sw, Me)
            sw.Flush()

            data = sw.ToString()

            Return data

        End Using

    End Function


    Public Shared Function FromXmlString(ByVal xmlString As String) As Settings

        If xmlString Is Nothing Then
            Throw New ArgumentNullException("xmlString")
        End If

        Dim mac As Settings = Nothing
        Dim ser As New XmlSerializer(GetType(Settings))

        Using sr As New StringReader(xmlString)

            mac = DirectCast(ser.Deserialize(sr), Settings)

        End Using

        Return mac

    End Function


    Private Shared Function ISOFileExists(ByVal isoStore As IsolatedStorageFile, ByVal fileName As String) As Boolean

        If isoStore Is Nothing Then
            Throw New ArgumentNullException("isoStore")
        End If

        If fileName Is Nothing OrElse fileName.Length = 0 Then
            Return False
        End If

        Dim names As String() = isoStore.GetFileNames("*")

        For Each name As String In names

            If String.Compare(name, fileName, True) = 0 Then

                Return True

            End If

        Next

        Return False

    End Function


    Private Shared Sub WriteToISOFile(ByVal isoStore As IsolatedStorageFile, ByVal fileName As String, ByVal data As String)

        ' Assign the writer to the store and the file TestStore.
        Using writer As New StreamWriter(New IsolatedStorageFileStream(fileName, FileMode.Create, isoStore))
            ' Have the writer write "Hello Isolated Storage" to the store.
            writer.Write(data)
        End Using

    End Sub


    Private Shared Function ReadFromISOFile(ByVal isoStore As IsolatedStorageFile, ByVal fileName As String) As String

        Dim sb As String = Nothing
        ' This code opens the TestStore.txt file and reads the string.
        Using reader As New StreamReader(New IsolatedStorageFileStream(fileName, FileMode.Open, isoStore))
            sb = reader.ReadToEnd()
        End Using

        Return sb.ToString()

    End Function

End Class
