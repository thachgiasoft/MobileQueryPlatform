TRUNCATE TABLE tSysInfo
go
INSERT INTO tSysInfo(SECTION,OptionName,OptionValue)
SELECT 'SYS','ProjectName','�ƶ���ѯƽ̨'
UNION ALL
SELECT 'SYS','CompanyName','xxxx���޹�˾'
UNION ALL
SELECT 'SYS','AuthCode',''''