# Tài liệu Phân quyền & Cấu trúc Hệ thống - Ecommerce Gemini GPT


### Thông tin chung của dự án
[Thông tin chung dự án](https://github.com/nguyenthinh28902/mini-project-ecommerce).

### Cấu hình xác thực tại Web
[Xem tiếp](https://github.com/nguyenthinh28902/ecommerce-cms-web).

### Xác thực tại identity
[Xem tiếp](https://github.com/nguyenthinh28902/ecommerce-identity-server-cms).

### Xác thực tại Getaway 
[Xem tiếp](https://github.com/nguyenthinh28902/ecommerce-api-gateway-cms).

### Xác thực tại Service (Product servcie)
[Xem tiếp](https://github.com/nguyenthinh28902/Ecom.ProductService).

Tài liệu này tóm tắt logic phân quyền và cấu trúc thực thể đã được tùy chỉnh để đáp ứng yêu cầu vận hành đa nhiệm giữa Tổng công ty và các Chi nhánh/Kho hàng.

## 1. Hệ thống Phân quyền (Roles & Permissions)

Hệ thống được thiết kế theo mô hình phân cấp dựa trên địa điểm làm việc (`Workplace`) và phòng ban (`Department`).

### 1.1. Các vai trò chính (Roles)
* **Admin (Hệ thống):** Toàn quyền truy cập (Full CRUD) trên mọi chức năng, dữ liệu của toàn bộ hệ thống mà không bị giới hạn bởi địa điểm.
* **Quản lý (Manager):**
    * **Tại Tổng công ty:** Quản lý vĩ mô toàn hệ thống.
    * **Tại Chi nhánh/Kho:** Có quyền quản trị hoàn toàn nhưng dữ liệu bị giới hạn (Data Scoping) trong phạm vi `WorkplaceId` của cơ sở đó.
* **Trưởng phòng (Department Head):** * Chỉ áp dụng tại **Tổng công ty**.
    * Có đặc quyền xem báo cáo chuyên sâu và giám sát hoạt động của các nhân sự thuộc cùng mã phòng ban (`DeptCode`) trên toàn hệ thống.

### 1.2. Danh sách Phòng ban (Departments)
Tên phòng ban đã được chuẩn hóa để đảm bảo tính chuyên nghiệp:
* **Kinh doanh (Business):** Phụ trách tư vấn, lên đơn hàng và phát triển thị trường.
* **Kế toán (Accountant):** Phụ trách hóa đơn, chứng từ và thu chi.
* **Quản trị Kho bãi (Warehouse):** Quản lý tồn kho tại chỗ (Cửa hàng) hoặc tồn kho tổng (Kho hàng).
* **Điều vận & Cung ứng (Logistics):** Điều phối hàng hóa giữa Tổng công ty, Kho và các Chi nhánh.

### 1.3. Logic quan hệ thực thể (Entity Relationships)
Dựa trên mô hình vận hành thực tế, các thực thể được ràng buộc bởi các mối quan hệ sau:

| Thực thể | Mối quan hệ | Giải thích logic |
| :--- | :---: | :--- |
| **Nhân sự - Nơi làm việc** | **1 : 1** | Mỗi nhân viên chỉ thuộc biên chế tại một địa điểm duy nhất (Tổng công ty, Chi nhánh hoặc Kho). |
| **Nhân sự - Phòng ban** | **1 : n** | Một nhân sự có thể kiêm nhiệm nhiều phòng ban (đa nhiệm) để tối ưu nguồn lực. |
| **Nơi làm việc - Phòng ban** | **n : n** | Một địa điểm có nhiều phòng ban hoạt động và một loại phòng ban có mặt ở nhiều nơi khác nhau. |
| **Phòng ban - Chức năng** | **1 : CRUD** | Phòng ban định nghĩa quyền hạn cụ thể (Thêm, Xem, Sửa, Xóa) trên từng module chức năng của hệ thống. |

---

## 2. Cấu trúc Kỹ thuật (Identity & Persistence)

Hệ thống sử dụng **ASP.NET Core Identity** với các tùy chỉnh để hỗ trợ mô hình đa nhiệm.

### 2.1. Thực thể Database tùy chỉnh
* **Kiểu dữ liệu khóa chính:** Toàn bộ hệ thống Identity sử dụng kiểu `int` để tối ưu hiệu suất truy vấn.
* **ApplicationDepartment (Role):** * Kế thừa từ `IdentityRole<int>`.
    * Sử dụng Enum `DepartmentCode` để định danh logic trong code thay vì dùng chuỗi.
* **UserDepartment (UserRole):** * Kế thừa từ `IdentityUserRole<int>`.
    * **Bổ sung thuộc tính:**
        * `IsDepartmentHead` (bool): Xác định quyền Trưởng phòng.
        * `IsPrimary` (bool): Đánh dấu phòng ban chính của nhân sự khi làm việc đa nhiệm.
