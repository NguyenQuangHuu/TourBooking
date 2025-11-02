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