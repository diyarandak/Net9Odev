# 🎵 Net9Odev - Müzik Platformu REST API

**.NET 9** ile geliştirilmiş, **JWT Authentication**, **Soft Delete** ve **Serilog** entegrasyonuna sahip profesyonel bir müzik yönetim sistemi backend projesi.

---

## 📋 İçindekiler
- [Özellikler](#-özellikler)
- [Teknolojiler](#-teknolojiler)
- [Mimari Yapı](#-mimari-yapı)
- [Kurulum](#-kurulum)
- [API Endpoint'leri](#-api-endpointleri)
- [Response Örnekleri](#-response-örnekleri)
- [Kullanıcı Bilgileri](#-varsayılan-kullanıcılar)
- [Veritabanı Şeması](#-veritabanı-şeması)

---

## ✨ Özellikler

### 🏗️ Mimari ve Tasarım
- **Katmanlı Mimari:** Controller → Service → Data Layer
- **Hibrit API Yaklaşımı:**
    - **Controller-based:** User, Album, Artist (Karmaşık CRUD)
    - **Minimal API:** Song, Label, Concert (Hafif ve hızlı)
- **DTO Pattern:** Entity'ler doğrudan expose edilmez
- **Standart API Response:** `{ success, message, data }` formatı

### 🔒 Güvenlik
- **JWT Authentication:** Bearer Token tabanlı kimlik doğrulama
- **BCrypt Password Hashing:** Şifreler güvenli şekilde saklanır
- **Role-based Authorization:** Admin/User yetkilendirme

### 💾 Veritabanı
- **SQLite** ile Entity Framework Core
- **Soft Delete:** Fiziksel silme yerine `IsDeleted` flag'i
- **Global Query Filter:** Silinmiş kayıtlar otomatik filtrelenir
- **Auto Timestamps:** `CreatedAt` ve `UpdatedAt` otomatik güncellenir
- **Seed Data:** İlk çalıştırmada Admin ve User otomatik oluşur

### 📊 Loglama
- **Serilog:** Konsol + dosya loglaması
- **Global Exception Middleware:** Tüm hatalar yakalanır ve loglanır

---

## 🛠️ Teknolojiler

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| .NET | 9.0 | Framework |
| Entity Framework Core | 9.0 | ORM |
| SQLite | 9.0 | Veritabanı |
| BCrypt.Net-Next | 4.0.3 | Şifre Hash'leme |
| JWT Bearer | 9.0.0 | Authentication |
| Serilog | 10.0.0 | Loglama |
| Swashbuckle (Swagger) | 6.6.2 | API Dokümantasyonu |

---

## 🏛️ Mimari Yapı

```
Net9Odev/
├── Controllers/          # Controller-based endpoints
│   ├── UserController.cs
│   ├── AlbumController.cs
│   └── ArtistController.cs
├── Services/            # Business Logic Layer
│   ├── IUserService.cs / UserService.cs
│   ├── IAlbumService.cs / AlbumService.cs
│   └── ...
├── Data/               # Data Access Layer
│   ├── AppDbContext.cs
│   └── DataSeeder.cs
├── Entities/           # Database Models
│   ├── User.cs
│   ├── Album.cs
│   └── ...
├── DTOs/              # Data Transfer Objects
│   ├── UserDtos.cs
│   ├── AlbumDtos.cs
│   └── ApiResponse.cs
├── Middleware/        # Custom Middleware
│   └── GlobalExceptionMiddleware.cs
└── Program.cs        # Startup + Minimal API
```

---

## 🚀 Kurulum

### 1️⃣ Gereksinimleri Yükleyin
```bash
# .NET 9 SDK yüklü olmalı
dotnet --version  # 9.0.x çıkmalı
```

### 2️⃣ Projeyi Klonlayın
```bash
git clone <repo-url>
cd Net9Odev
```

### 3️⃣ Bağımlılıkları Yükleyin
```bash
dotnet restore
```

### 4️⃣ Veritabanını Oluşturun
```bash
# Migration'lar zaten mevcut, direkt uygulayın
dotnet ef database update
```

### 5️⃣ Projeyi Çalıştırın
```bash
dotnet run
```

### 6️⃣ Swagger'a Gidin
Tarayıcıda açın: **http://localhost:5004/swagger**

---

## 🔗 API Endpoint'leri

### 👤 User Management (Controller)

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| POST | `/api/User/register` | Yeni kullanıcı kaydı | ❌ |
| POST | `/api/User/login` | Giriş yap, token al | ❌ |
| GET | `/api/User` | Tüm kullanıcıları listele | ❌ |
| PUT | `/api/User/{id}` | Kullanıcı güncelle | ✅ |
| DELETE | `/api/User/{id}` | Kullanıcı sil (soft) | ✅ |

### 🎤 Artist Management (Controller)

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Artist` | Sanatçıları listele | ❌ |
| GET | `/api/Artist/{id}` | Sanatçı detayı | ❌ |
| POST | `/api/Artist` | Sanatçı ekle | ✅ |
| PUT | `/api/Artist/{id}` | Sanatçı güncelle | ✅ |
| DELETE | `/api/Artist/{id}` | Sanatçı sil (soft) | ✅ |

### 💿 Album Management (Controller)

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/Album` | Albümleri listele | ❌ |
| GET | `/api/Album/{id}` | Albüm detayı | ❌ |
| POST | `/api/Album` | Albüm ekle | ✅ |
| PUT | `/api/Album/{id}` | Albüm güncelle | ✅ |
| DELETE | `/api/Album/{id}` | Albüm sil (soft) | ✅ |

### 🎵 Song Management (Minimal API)

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/song` | Şarkıları listele | ❌ |
| GET | `/api/song/{id}` | Şarkı detayı | ❌ |
| POST | `/api/song` | Şarkı ekle | ✅ |
| PUT | `/api/song/{id}` | Şarkı güncelle | ✅ |
| DELETE | `/api/song/{id}` | Şarkı sil (soft) | ✅ |

### 🏢 Label Management (Minimal API)

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/label` | Şirketleri listele | ❌ |
| GET | `/api/label/{id}` | Şirket detayı | ❌ |
| POST | `/api/label` | Şirket ekle | ✅ |
| PUT | `/api/label/{id}` | Şirket güncelle | ✅ |
| DELETE | `/api/label/{id}` | Şirket sil (soft) | ✅ |

### 🎭 Concert Management (Minimal API)

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| GET | `/api/concert` | Konserleri listele | ❌ |
| GET | `/api/concert/{id}` | Konser detayı | ❌ |
| POST | `/api/concert` | Konser ekle | ✅ |
| PUT | `/api/concert/{id}` | Konser güncelle | ✅ |
| DELETE | `/api/concert/{id}` | Konser sil (soft) | ✅ |

> **Not:** ✅ işareti olan endpoint'ler **Authorization: Bearer {token}** header'ı gerektirir.

---

## 📄 Response Örnekleri

### ✅ Başarılı Login Response
**Request:**
```bash
POST /api/User/login
{
  "email": "admin@music.com",
  "password": "123"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Giriş başarılı",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### ✅ Başarılı Albüm Listesi
**Request:**
```bash
GET /api/Album
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Albümler listelendi",
  "data": [
    {
      "id": 1,
      "name": "Test Album",
      "price": 29.99,
      "releaseDate": "2024-01-01T00:00:00Z",
      "artistId": 1,
      "createdAt": "2024-12-13T10:30:00Z"
    }
  ]
}
```

### ❌ Hatalı Response (404 Not Found)
```json
{
  "success": false,
  "message": "Albüm bulunamadı",
  "data": null
}
```

### ❌ Hatalı Response (401 Unauthorized)
```json
{
  "success": false,
  "message": "Sunucu hatası: Authorization header eksik",
  "data": null
}
```

---

## 👤 Varsayılan Kullanıcılar

İlk çalıştırmada otomatik oluşturulur:

| Rol | Email | Şifre | Yetki |
|-----|-------|-------|-------|
| **Admin** | `admin@music.com` | `123` | Tüm CRUD işlemleri |
| **User** | `user@music.com` | `123` | Sadece okuma (GET) |

### 🔐 Token Alma (Örnek)
```bash
curl -X POST "http://localhost:5004/api/User/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@music.com","password":"123"}'
```

### 🔑 Token Kullanımı
```bash
curl -X POST "http://localhost:5004/api/Album" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {yukarıdaki-token}" \
  -d '{
    "name": "Yeni Album",
    "price": 39.99,
    "releaseDate": "2024-01-01T00:00:00Z",
    "artistId": 1
  }'
```

---

## 🗄️ Veritabanı Şeması

### Entity İlişkileri
```
Label (1) ───────< (N) Artist (1) ───────< (N) Album (1) ───────< (N) Song
                         │
                         └──────────< (N) Concert

User (Bağımsız - Authentication için)
```

### Tablolar ve Alanlar

#### 📌 Users
- `Id` (PK)
- `FullName`
- `Email` (Unique)
- `Password` (BCrypt Hash)
- `Role` (Admin/User)
- `CreatedAt`, `UpdatedAt`, `IsDeleted`

#### 📌 Labels (Plak Şirketleri)
- `Id` (PK)
- `Name`
- `Country`
- `CreatedAt`, `UpdatedAt`, `IsDeleted`

#### 📌 Artists (Sanatçılar)
- `Id` (PK)
- `Name`
- `Bio`
- `LabelId` (FK → Labels)
- `CreatedAt`, `UpdatedAt`, `IsDeleted`

#### 📌 Albums
- `Id` (PK)
- `Name`
- `Price`
- `ReleaseDate`
- `ArtistId` (FK → Artists)
- `CreatedAt`, `UpdatedAt`, `IsDeleted`

#### 📌 Songs
- `Id` (PK)
- `Name`
- `DurationSeconds`
- `TrackNumber`
- `AlbumId` (FK → Albums)
- `CreatedAt`, `UpdatedAt`, `IsDeleted`

#### 📌 Concerts
- `Id` (PK)
- `Venue` (Mekan)
- `City`
- `Date`
- `ArtistId` (FK → Artists)
- `CreatedAt`, `UpdatedAt`, `IsDeleted`

---


## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

## 👨‍💻 Geliştirici

**Diyar Andak**