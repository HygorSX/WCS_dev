CREATE TABLE PrinterMonitoring (
    Id INT PRIMARY KEY IDENTITY,
    Serial NVARCHAR(50) NOT NULL,
    DeviceManufacturer NVARCHAR(100),
    DeviceModel NVARCHAR(100),
    QuantidadeTotalImpressora INT,
    PorcentagemBlack INT,
    PorcentagemCyan INT,
    PorcentagemYellow INT,
    PorcentagemMagenta INT,
    PorcentagemFusor INT,
    PorcentagemBelt INT,
    PorcentagemKitManutenção INT,
    PrinterStatus NVARCHAR(50),
    DataHoraMonitoramento DATETIME DEFAULT GETDATE()
);
