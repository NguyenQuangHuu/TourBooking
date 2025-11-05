# **Dự án Đặt Tour du lịch và hệ thống quản lý đặt tour du lịch**  
## Công nghệ sử dụng:  
    C#, DOTNET, ADONET, JWT, async/await...
    Hệ thống sẽ áp dụng Domain Driven Design theo khuyến khích từ đội ngũ lập trình của DOTNET.
    Ngoài ra, hệ thống kết hợp Design Pattern Mediator và CQRS để tăng sự linh hoạt cũng như giảm sự phụ thuộc ở các tầng.
## Mục tiêu:
    Xây dựng dự án đặt tour du lịch để tìm hiểu thêm về nghiệp vụ xây dựng API bằng asp.net  
## Mô tả chức năng:
### Đối với người dùng là khách hàng hoặc khách vãng lai: 
    Có thể xem tour du lịch đang được triển khai trong hệ thống 
    và có thể đặt tour nếu đã đăng ký và sử dụng tài khoản của hệ thống cung cấp để đặt.
### Đối với người dùng là nhân viên hệ thống:
    Ở đây, nhân viên có sẽ được chia thành nhiều cấp ở nhiều phòng ban khác nhau
    Ví dụ: 
     - Nhân viên Sale: Có thể thay khách hàng đặt tour, có thể xem tour được hiển thị ở hệ thống 
     và phải có tài khoản được cấp để dùng nhưng tính năng này.
     - Nhân viên Marketing: Đăng bài tạo tương tác cho hệ thống, giới thiệu các kinh nghiệm du lịch...
     - Nhân viên điều hành tour: Sẽ được phân công để phụ trách tour khi tour được triển khai.
     - Quản lý/Admin hệ thống: Tạo tour, quản lý tour và triển khai tour cho hệ thống, phân công công việc, 
        xem báo cáo, thống kê...
## Tìm hiểu về Domain Driven Design
    DDD là một nguyên lý thiết kế hệ thống chung được đưa ra để lập trình viên tập trung xử lý ở tầng domain
    Sử dụng các thuật ngữ chung để xây dựng nghệ thống như Ubitous language, context boundary, value object...

## Mô tả nghiệp vụ và chức năng trong hệ thống

### Khách vãng lai (Không có tài khoản)
    Truy cập website:
        => Xem danh sách dịch vụ đang được triển khai của hệ thống
        => Xem chi tiết dịch vụ
        => Đăng ký tài khoản để trở thành người dùng của hệ thống
        => Xem các thông tin cơ bản được hiển thị bên trong website ở màn hình trang chủ (Liên hệ/Thông tin công ty/...)
### Người dùng (Có tài khoản mức khách hàng)
    Bao gồm các chức năng cơ bản dành cho Khách vãng lai, ngoài ra còn có:
        => Yêu cầu đặt tour -> điền thông tin hành khách -> xác nhận đặt tour -> hoàn tất thanh toán
        => Quản lý tài khoản -> Cập nhật thông tin cá nhân
        => Quản lý lịch sử đặt tour và thanh toán -> Xem danh sách các tour đã đặt (bao gồm đặt thành công, thất bại, quá hạn...)
### Nhân viên hệ thống (Có tài khoản dành cho việc truy cập hệ thống)
    Đối với tài khoản hệ thống mức nhân viên
        => Đặt tour cho khách hàng liên hệ qua tổng đài hoặc qua sale
        => Xem phân công
    Đối với tài khoản hệ thống mức quản lý
        => Quản lý thông tin nhân viên
        => Phân công công việc
    Đối với tài khoản hệ thống mức admin
        => Quản lý và triển khai tour
        => Xem báo cáo thống kê doanh thu
        => Quản lý tài khoản nhân viên hệ thống...
