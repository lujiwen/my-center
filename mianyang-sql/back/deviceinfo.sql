--------------------------------------------------------
--  文件已创建 - 星期三-二月-24-2016   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Table DEVICEINFO
--------------------------------------------------------

  CREATE TABLE "MIANYANG"."DEVICEINFO" 
   (	"D_ID" NUMBER(38,0), 
	"TYPE" VARCHAR2(30 BYTE), 
	"CABID" NUMBER(38,0), 
	"SUBSYSTEMSERIAL" NUMBER(38,0), 
	"SUBSYSTEMNAME" VARCHAR2(30 BYTE), 
	"HIGHTHRESHOLD" BINARY_FLOAT, 
	"LOWTHRESHOLD" BINARY_FLOAT, 
	"DEVLOCALADDRESS" NUMBER(38,0), 
	"INTERFACEID" NUMBER(38,0), 
	"CORRECTFACTOR" BINARY_FLOAT, 
	"DATAUNIT" VARCHAR2(20 BYTE), 
	"INPUTARG1" BINARY_FLOAT, 
	"INPUTARG2" BINARY_FLOAT, 
	"INPUTARG3" BINARY_FLOAT, 
	"BUILDINGID" NUMBER(38,0), 
	"HANDLETYPEINSYSTEM" VARCHAR2(45 BYTE)
   ) PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE "USERS" ;
REM INSERTING into MIANYANG.DEVICEINFO
SET DEFINE OFF;
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (5,'6517A',2,1,'特排管道测量装置','1000000.0','1000.0',0,6,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (6,'6517B',2,2,'二级容器测量装置','1000000.0','1000.0',0,2,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (7,'6517A',2,3,'手套箱测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (8,'6517B',2,4,'操作间测量装置','10000.0','1000.0',0,4,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (10,'Quality',2,1,'特排管道测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'Quality');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (11,'Quality',2,2,'二级容器测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'Quality');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (12,'Quality',2,3,'手套箱测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'Quality');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (13,'Quality',2,4,'操作间测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'Quality');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (14,'Presure',2,2,'二级容器测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (15,'Presure',2,3,'手套箱测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (16,'Presure',2,4,'操作间测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (17,'Presure',2,1,'特排管道测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (19,'Pump',2,2,'二级容器测量装置','0.0','0.0',3,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (20,'Pump',2,3,'手套箱测量装置','0.0','0.0',4,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (21,'Pump',2,4,'操作间测量装置','0.0','0.0',5,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (22,'Pump',2,1,'特排管道测量装置','0.0','0.0',2,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (23,'6517A',3,1,'二级容器测量装置','1000000.0','1000.0',0,2,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (24,'6517B',3,2,'手套箱测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (26,'XH3127',3,2,'操作间测量装置','10000.0','1000.0',0,4,'0.9','Bg/m^3','0.0','0.0','0.0',2,'XH31253127');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (27,'XH3125',3,1,'二级容器测量装置','10000.0','1000.0',0,4,'0.9','Bg/m^3','0.0','0.0','0.0',2,'XH31253127');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (29,'Presure',3,1,'二级容器测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (30,'Presure',3,2,'手套箱测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (32,'Pump',3,1,'二级容器测量装置','0.0','0.0',2,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (33,'Pump',3,2,'手套箱测量装置','0.0','0.0',3,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (35,'6517B',4,1,'二级容器测量装置','1000000.0','1000.0',0,2,'0.9','Bg/m^3','0.0','0.0','0.0',2,'6517AB');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (37,'Quality',4,2,'手套箱测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'Quality');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (38,'Quality',4,1,'二级容器测量装置','10000.0','1000.0',0,3,'0.9','Bg/m^3','0.0','0.0','0.0',2,'Quality');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (39,'Presure',4,1,'二级容器测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (40,'Presure',4,2,'手套箱测量装置','10000.0','1000.0',0,0,'0.9','V','0.0','0.0','0.0',2,'Presure');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (41,'Pump',4,1,'二级容器测量装置','0.0','0.0',2,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
Insert into MIANYANG.DEVICEINFO (D_ID,TYPE,CABID,SUBSYSTEMSERIAL,SUBSYSTEMNAME,HIGHTHRESHOLD,LOWTHRESHOLD,DEVLOCALADDRESS,INTERFACEID,CORRECTFACTOR,DATAUNIT,INPUTARG1,INPUTARG2,INPUTARG3,BUILDINGID,HANDLETYPEINSYSTEM) values (42,'Pump',4,2,'手套箱测量装置','0.0','0.0',3,0,'0.0','0','0.0','0.0','0.0',2,'Pump');
--------------------------------------------------------
--  DDL for Index DEVICEINFO_PK
--------------------------------------------------------

  CREATE UNIQUE INDEX "MIANYANG"."DEVICEINFO_PK" ON "MIANYANG"."DEVICEINFO" ("D_ID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE "USERS" ;
--------------------------------------------------------
--  Constraints for Table DEVICEINFO
--------------------------------------------------------

  ALTER TABLE "MIANYANG"."DEVICEINFO" ADD CONSTRAINT "DEVICEINFO_PK" PRIMARY KEY ("D_ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE "USERS"  ENABLE;
 
  ALTER TABLE "MIANYANG"."DEVICEINFO" MODIFY ("D_ID" NOT NULL ENABLE);
