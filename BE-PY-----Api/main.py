from sqlalchemy.sql import text
from fastapi import FastAPI, HTTPException, Depends
from fastapi.middleware.cors import CORSMiddleware
from sqlalchemy import create_engine, Column, String, Boolean, DateTime, ForeignKey, Numeric
from sqlalchemy.dialects.mssql import UNIQUEIDENTIFIER
from sqlalchemy.orm import sessionmaker, declarative_base, Session
from pydantic import BaseModel
import uuid
from datetime import datetime
import urllib

# ✅ Initialize FastAPI
app = FastAPI()

# ✅ Enable CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:4200"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# ✅ SQL Server Connection String
DATABASE_URL = "mssql+pyodbc:///?odbc_connect=" + urllib.parse.quote_plus(
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=VELLOSO-YOGA\\SQLEXPRESS;"
    "DATABASE=fatura;"
    "Trusted_Connection=yes;"
    "MARS_Connection=yes;"
    "TrustServerCertificate=yes"
)

# ✅ Initialize database
engine = create_engine(DATABASE_URL, echo=True)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()  # Keep for ORM mapping, but DO NOT create tables

# ✅ Dependency to Get Database Session
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# ✅ Define Pydantic Models for API Requests
class UserBase(BaseModel):
    name: str
    email: str
    nif: str
    password: str

class ReciboVerdeBase(BaseModel):
    user_id: str
    numero_unico: str
    num_documento: int
    situacao: str
    tipo_documento: str
    data_emissao: datetime
    valor_base: float
    valor_iva: float
    valor_irs: float
    valor_total_c_impostos: float
    importancia_recebida: float

class DeclaracaoIvaBase(BaseModel):
    user_id: str
    tipo: str
    situacao: str
    indicador_pagamento_reembolso: str
    base_tributavel_centimos: float
    imposto_liquidado_centimos: float
    imposto_dedutivel_centimos: float
    valor1: float
    valor2: float
    periodo: str
    data_recepcao: datetime
    numero_declaracao: str

class UserActionBase(BaseModel):
    user_id: str
    active: bool = True

# ✅ CRUD Endpoints

### **🟢 USERS CRUD**
@app.get("/users/")
def get_users(db: Session = Depends(get_db)):
    users = db.execute(text("SELECT * FROM Users")).fetchall()
    if not users:
        raise HTTPException(status_code=404, detail="No users found")
    return [dict(u._mapping) for u in users]

@app.post("/users/")
def create_user(user: UserBase, db: Session = Depends(get_db)):
    user_id = str(uuid.uuid4())
    query = text("""
        INSERT INTO Users (Id, Name, Email, NIF, Password) 
        VALUES (:id, :name, :email, :nif, :password)
    """)
    db.execute(query, {"id": user_id, "name": user.name, "email": user.email, "nif": user.nif, "password": user.password})
    db.commit()
    return {"id": user_id, **user.dict()}

@app.put("/users/{user_id}")
def update_user(user_id: str, user: UserBase, db: Session = Depends(get_db)):
    query = text("""
        UPDATE Users 
        SET Name = :name, Email = :email, NIF = :nif, Password = :password 
        WHERE Id = :user_id
    """)
    result = db.execute(query, {
        "user_id": user_id,
        "name": user.name,
        "email": user.email,
        "nif": user.nif,
        "password": user.password
    })
    db.commit()

    if result.rowcount == 0:
        raise HTTPException(status_code=404, detail="User not found")

    return {"message": "User updated successfully"}


@app.get("/users/{user_id}")
def get_user(user_id: str, db: Session = Depends(get_db)):
    user = db.execute(text("SELECT * FROM Users WHERE Id = :user_id"), {"user_id": user_id}).fetchone()
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return dict(user._mapping)

@app.delete("/users/{user_id}")
def delete_user(user_id: str, db: Session = Depends(get_db)):
    db.execute(text("DELETE FROM Users WHERE Id = :user_id"), {"user_id": user_id})
    db.commit()
    return {"message": "User deleted successfully"}

### **🟢 RECIBOS VERDES CRUD**
@app.get("/recibos-verdes/{user_id}")
def get_recibos_verdes_by_user_id(user_id: str, db: Session = Depends(get_db)):
    recibos = db.execute(text("SELECT * FROM Invoices WHERE UserId = :user_id"), {"user_id": user_id}).fetchall()
    if not recibos:
        raise HTTPException(status_code=404, detail="No recibos verdes found for this user")
    return [dict(r._mapping) for r in recibos]

@app.post("/recibos-verdes/")
def create_recibo_verde(recibo: ReciboVerdeBase, db: Session = Depends(get_db)):
    recibo_id = str(uuid.uuid4())
    query = text("""
        INSERT INTO Invoices (Id, UserId, NumeroUnico, NumDocumento, Situacao, TipoDocumento, DataEmissao, 
        ValorBase, ValorIVA, ValorIRS, ValorTotalCImpostos, ImportanciaRecebida) 
        VALUES (:id, :user_id, :numero_unico, :num_documento, :situacao, :tipo_documento, :data_emissao, 
        :valor_base, :valor_iva, :valor_irs, :valor_total_c_impostos, :importancia_recebida)
    """)
    db.execute(query, {"id": recibo_id, **recibo.dict()})
    db.commit()
    return {"id": recibo_id, **recibo.dict()}

### **🟢 DECLARAÇÕES IVA CRUD**
@app.get("/declaracoes-iva/{user_id}")
def get_declaracoes_iva_by_user_id(user_id: str, db: Session = Depends(get_db)):
    declaracoes = db.execute(text("SELECT * FROM TaxDeclarations WHERE UserId = :user_id"), {"user_id": user_id}).fetchall()
    if not declaracoes:
        raise HTTPException(status_code=404, detail="No declarações IVA found for this user")
    return [dict(d._mapping) for d in declaracoes]

@app.post("/declaracoes-iva/")
def create_declaracao_iva(declaracao: DeclaracaoIvaBase, db: Session = Depends(get_db)):
    declaracao_id = str(uuid.uuid4())
    query = text("""
        INSERT INTO TaxDeclarations (Id, UserId, Tipo, Situacao, IndicadorPagamentoReembolso, BaseTributavelCentimos, 
        ImpostoLiquidadoCentimos, ImpostoDedutivelCentimos, Valor1, Valor2, Periodo, DataRececao, NumeroDeclaracao) 
        VALUES (:id, :user_id, :tipo, :situacao, :indicador_pagamento_reembolso, :base_tributavel_centimos, 
        :imposto_liquidado_centimos, :imposto_dedutivel_centimos, :valor1, :valor2, :periodo, :data_recepcao, :numero_declaracao)
    """)
    db.execute(query, {"id": declaracao_id, **declaracao.dict()})
    db.commit()
    return {"id": declaracao_id, **declaracao.dict()}

### **🟢 USER ACTIONS CRUD**
@app.get("/user-actions/{user_id}")
def get_user_actions(user_id: str, db: Session = Depends(get_db)):
    actions = db.execute(text("SELECT * FROM UserActions WHERE UserId = :user_id"), {"user_id": user_id}).fetchall()
    return [dict(a._mapping) for a in actions]

@app.post("/user-actions/")
def create_user_action(action: UserActionBase, db: Session = Depends(get_db)):
    action_id = str(uuid.uuid4())
    query = text("""
        INSERT INTO UserActions (Id, UserId, Active, RequestedAt) 
        VALUES (:id, :user_id, 1, :requested_at)
    """)
    db.execute(query, {"id": action_id, "user_id": action.user_id, "active": action.active, "requested_at": datetime.utcnow()})
    db.commit()
    return {"id": action_id, **action.dict()}
