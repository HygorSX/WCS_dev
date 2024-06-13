CREATE TABLE PrinterMonitoring (
    Id INT PRIMARY KEY IDENTITY,
    Serial NVARCHAR(50) NOT NULL,
    DeviceManufacturer NVARCHAR(100),
    DeviceModel NVARCHAR(100),
    DeviceName NVARCHAR(100),
    TotalPaginas INT,
    TotalCopias INT,
    PorcentagemBlack INT,
    PorcentagemKitManutenção INT,
    PrinterStatus NVARCHAR(50),
    MacAddress NVARCHAR(50),
    DataHoraMonitoramento DATETIME DEFAULT GETDATE(),
    PorcentagemCyan INT,
    PorcentagemYellow INT,
    PorcentagemMagenta INT,
    PorcentagemFusor INT,
    PorcentagemBelt INT
);
