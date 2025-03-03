from sqlalchemy.sql import text
from fastapi import FastAPI, HTTPException, Depends
from sqlalchemy import create_engine, Column, String, Boolean, DateTime, ForeignKey, Numeric
from sqlalchemy.orm import sessionmaker, declarative_base, Session
from pydantic import BaseModel
import uuid
from datetime import datetime
import urllib

# ✅ Initialize FastAPI
app = FastAPI()

# ✅ SQL Server Connection String
DATABASE_URL = "mssql+pyodbc:///?odbc_connect=" + urllib.parse.quote_plus(
    "DRIVER={ODBC Driver 17 for SQL Server};"
    "SERVER=SQL1002.site4now.net;"
    "DATABASE=db_ab37b3_faturapp;"
    "UID=db_ab37b3_faturapp_admin;"
    "PWD=sS4V2FPC;"
    "TrustServerCertificate=yes"
)

# ✅ Initialize database
engine = create_engine(DATABASE_URL, echo=True)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()

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

# ✅ CRUD Endpoints
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
