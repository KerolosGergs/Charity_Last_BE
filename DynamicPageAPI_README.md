# Dynamic Page Management API - Backend Implementation

## Overview
This document describes the complete backend implementation for the Dynamic Page Management module in the Charity Backend API. The implementation follows clean architecture principles with proper separation of concerns.

## Architecture

### Layer Structure
```
Charity_Last_BE/
├── DAL/                          # Data Access Layer
│   ├── Data/
│   │   ├── Models/
│   │   │   └── IdentityModels/
│   │   │       └── DynamicPage.cs
│   │   └── ApplicationDbContext.cs
│   └── Repositories/
│       ├── IDynamicPageRepository.cs
│       └── DynamicPageRepository.cs
├── BLL/                          # Business Logic Layer
│   ├── ServiceAbstraction/
│   │   └── IDynamicPageService.cs
│   └── Service/
│       └── DynamicPageService.cs
├── Shared/                       # Shared DTOs
│   └── DTOS/
│       └── DynamicPage/
│           └── DynamicPageDto.cs
└── Charity_BE/                   # API Layer
    └── Controllers/
        └── DynamicPageController.cs
```

## Database Models

### DynamicPage Entity
```csharp
public class DynamicPage
{
    public int Id { get; set; }
    public string PageName { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation Properties
    public virtual ICollection<DynamicPageItem> Items { get; set; }
    public virtual ApplicationUser? CreatedByUser { get; set; }
    public virtual ApplicationUser? UpdatedByUser { get; set; }
}
```

### DynamicPageItem Entity
```csharp
public class DynamicPageItem
{
    public int Id { get; set; }
    public int DynamicPageId { get; set; }
    public string Type { get; set; } // "text", "image_text", "file"
    public string Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? FileName { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public virtual DynamicPage DynamicPage { get; set; }
}
```

## API Endpoints

### Public Endpoints (No Authentication Required)
```
GET    /api/dynamicpage              # Get all pages
GET    /api/dynamicpage/active       # Get active pages only
GET    /api/dynamicpage/{id}         # Get page by ID
GET    /api/dynamicpage/slug/{slug}  # Get page by slug
```

### Admin Endpoints (Admin Role Required)
```
POST   /api/dynamicpage              # Create new page
PUT    /api/dynamicpage/{id}         # Update existing page
DELETE /api/dynamicpage/{id}         # Delete page
PATCH  /api/dynamicpage/{id}/toggle-active  # Toggle page status
POST   /api/dynamicpage/upload       # Upload file
DELETE /api/dynamicpage/delete-file  # Delete file
```

## Request/Response Examples

### Create Page Request
```json
{
  "pageName": "صفحة رضا العملاء",
  "description": "صفحة مخصصة لرضا العملاء",
  "slug": "customer-satisfaction",
  "items": [
    {
      "type": "text",
      "content": "محتوى النص هنا",
      "order": 0
    },
    {
      "type": "image_text",
      "content": "وصف الصورة",
      "imageUrl": "/uploads/dynamic-pages/image/abc123.jpg",
      "order": 1
    },
    {
      "type": "file",
      "content": "وصف الملف",
      "fileUrl": "/uploads/dynamic-pages/document/def456.pdf",
      "fileName": "document.pdf",
      "order": 2
    }
  ]
}
```

### Create Page Response
```json
{
  "success": true,
  "message": "Page created successfully",
  "data": {
    "id": 1,
    "pageName": "صفحة رضا العملاء",
    "description": "صفحة مخصصة لرضا العملاء",
    "slug": "customer-satisfaction",
    "isActive": true,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": null,
    "createdBy": "user-id",
    "updatedBy": null,
    "items": [
      {
        "id": 1,
        "dynamicPageId": 1,
        "type": "text",
        "content": "محتوى النص هنا",
        "imageUrl": null,
        "fileUrl": null,
        "fileName": null,
        "order": 0,
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": null
      }
    ]
  },
  "statusCode": 200,
  "errors": []
}
```

### File Upload Request
```
POST /api/dynamicpage/upload?fileType=image
Content-Type: multipart/form-data

file: [binary file data]
```

### File Upload Response
```json
{
  "success": true,
  "message": "File uploaded successfully",
  "data": {
    "url": "/uploads/dynamic-pages/image/abc123.jpg",
    "fileName": "original-file.jpg"
  },
  "statusCode": 200,
  "errors": []
}
```

## Validation Rules

### Page Validation
- **PageName**: Required, max 200 characters
- **Description**: Optional, max 500 characters
- **Slug**: Optional, max 100 characters, must be unique
- **Items**: Required, minimum 1 item

### Item Validation
- **Type**: Required, must be "text", "image_text", or "file"
- **Content**: Required
- **Order**: Required, integer

### File Upload Validation
- **Images**: JPG, JPEG, PNG, GIF (max 5MB)
- **Documents**: PDF, DOC, DOCX, JPG, JPEG, PNG (max 10MB)

## Business Logic Features

### Automatic Slug Generation
If no slug is provided, the system automatically generates a URL-friendly slug from the page name:
- Removes special characters
- Converts to lowercase
- Replaces spaces with hyphens
- Removes multiple hyphens

### File Management
- Files are stored in organized directories: `/uploads/dynamic-pages/{type}/`
- Unique filenames are generated using GUIDs
- File validation includes type and size checks
- Automatic cleanup when pages are deleted

### Audit Trail
- Tracks creation and modification timestamps
- Records user who created/modified pages
- Maintains history of changes

## Security Features

### Authentication & Authorization
- Public endpoints for reading pages
- Admin-only access for CRUD operations
- JWT token-based authentication
- Role-based authorization (Admin role required)

### Input Validation
- Model validation using Data Annotations
- Custom business rule validation
- SQL injection prevention through Entity Framework
- XSS prevention through proper encoding

### File Security
- File type validation
- File size limits
- Secure file storage outside web root
- Path traversal prevention

## Error Handling

### Standardized Error Responses
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "statusCode": 400,
  "errors": [
    "Specific error details"
  ]
}
```

### HTTP Status Codes
- **200**: Success
- **201**: Created
- **400**: Bad Request (validation errors)
- **401**: Unauthorized
- **403**: Forbidden
- **404**: Not Found
- **500**: Internal Server Error

## Database Configuration

### Entity Framework Configuration
```csharp
// In ApplicationDbContext.OnModelCreating()
builder.Entity<DynamicPage>()
    .HasOne(dp => dp.CreatedByUser)
    .WithMany()
    .HasForeignKey(dp => dp.CreatedBy)
    .OnDelete(DeleteBehavior.Restrict);

builder.Entity<DynamicPage>()
    .HasMany(dp => dp.Items)
    .WithOne(item => item.DynamicPage)
    .HasForeignKey(item => item.DynamicPageId)
    .OnDelete(DeleteBehavior.Cascade);
```

### Migration
```bash
dotnet ef migrations add AddDynamicPages --project DAL --startup-project Charity_BE
dotnet ef database update --project DAL --startup-project Charity_BE
```

## Dependency Injection

### Service Registration
```csharp
// In Program.cs
builder.Services.AddScoped<IDynamicPageRepository, DynamicPageRepository>();
builder.Services.AddScoped<IDynamicPageService, DynamicPageService>();
```

## Testing

### Sample Data
The system includes sample data for testing:
- Customer Satisfaction Page
- Medical Services Page
- Educational Programs Page

### Test Scenarios
1. **CRUD Operations**: Create, read, update, delete pages
2. **File Upload**: Upload images and documents
3. **Validation**: Test input validation rules
4. **Authorization**: Test role-based access
5. **Slug Generation**: Test automatic slug creation
6. **Error Handling**: Test various error scenarios

## Performance Considerations

### Database Optimization
- Proper indexing on frequently queried fields
- Eager loading of related entities
- Efficient query patterns

### File Storage
- Organized file structure
- Efficient file naming
- Proper cleanup procedures

### Caching Strategy
- Consider implementing caching for frequently accessed pages
- Cache invalidation on page updates

## Future Enhancements

### Planned Features
- [ ] Pagination for large page lists
- [ ] Search and filtering capabilities
- [ ] Page versioning and history
- [ ] Bulk operations
- [ ] Page templates
- [ ] Content scheduling
- [ ] Analytics and reporting
- [ ] Multi-language support

### Technical Improvements
- [ ] Implement caching layer
- [ ] Add comprehensive logging
- [ ] Implement rate limiting
- [ ] Add API documentation (Swagger)
- [ ] Performance monitoring
- [ ] Automated testing suite

## Deployment

### Requirements
- .NET 8.0
- SQL Server or compatible database
- File storage for uploads
- JWT configuration

### Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;..."
  },
  "JWT": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  }
}
```

### File Storage
Ensure the `/uploads/dynamic-pages/` directory exists and is writable by the application.

## Support

For questions or issues related to the Dynamic Page Management API:
1. Check the API documentation
2. Review error logs
3. Test with sample data
4. Contact the development team

## License
This implementation is part of the Charity Backend project and follows the same licensing terms. 