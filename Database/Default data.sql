TRUNCATE TABLE tSysInfo
go
INSERT INTO dbo.tSysCfg
        ( Section, OptionName, OptionValue )
VALUES  ( 'SYS', -- Section - char(128)
          'Company', -- OptionName - char(128)
          'Ĭ�Ϲ�˾'  -- OptionValue - char(5000)
          )
          
INSERT INTO dbo.tSysCfg
        ( Section, OptionName, OptionValue )
VALUES  ( 'SYS', -- Section - char(128)
          'License', -- OptionName - char(128)
          ''  -- OptionValue - char(5000)
          )