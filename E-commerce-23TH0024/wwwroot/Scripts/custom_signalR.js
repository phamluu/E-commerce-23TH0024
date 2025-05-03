$(function () {
    var currentPage = window.location.href;
    var hubConnection;
    var userHub;
    var Id = localStorage.getItem('Id');

    function reconnect() {
        if (hubConnection.state === $.signalR.connectionState.disconnected) {
            setTimeout(function () {
                hubConnection.start().done(function () {
                    console.log("SignalR reconnected successfully!");
                }).fail(function (err) {
                    console.error("Lỗi khi tái kết nối SignalR: ", err);
                });
            }, 3000); // Thử lại sau 3 giây
        }
    }
    function sendPageToServer(url) {
        if (!hubConnection) {
            hubConnection = $.hubConnection();
            userHub = hubConnection.createHubProxy('userHub');
        }

        hubConnection.start({ transport: ['longPolling'] }).done(function () {
            debugger;
            if (Id == null || Id === "") {
                Id = hubConnection.id;
                localStorage.setItem('Id', Id);
            }
            console.log("Kết nối SignalR đã được bắt đầu!");

            // Thiết lập timeout thủ công
            var invokePromise = userHub.invoke('TrackUserPage', url, Id);

            var timeoutPromise = new Promise(function (resolve, reject) {
                setTimeout(function () {
                    reject('Lỗi: Thời gian chờ đã hết (timeout)');
                }, 10000); // Timeout sau 10 giây
            });

            Promise.race([invokePromise, timeoutPromise])
                .then(function (result) {
                    console.log("TrackUserPage gọi thành công!");
                })
                .catch(function (err) {
                    // Kiểm tra xem lỗi là một đối tượng Error hay không
                    if (err instanceof Error) {
                        console.error("Lỗi khi gọi TrackUserPage: ", err.message);
                        console.error("Stack trace: ", err.stack);
                    } else {
                        // Nếu không phải là đối tượng Error, kiểm tra kiểu dữ liệu của lỗi
                        console.error("Lỗi không phải là một đối tượng Error:", err);

                        // Nếu lỗi là đối tượng, chuyển thành chuỗi và in ra
                        if (typeof err === 'object') {
                            console.error("Chi tiết lỗi (object):", JSON.stringify(err));
                        } else {
                            console.error("Chi tiết lỗi (string):", err);
                        }
                    }
                });
        }).fail(function (err) {
            console.error("Lỗi khi kết nối SignalR: ", err);
            reconnect();
        });
    }
    // Gửi URL lần đầu khi tải trang
    sendPageToServer(currentPage);

    // Theo dõi sự thay đổi URL (nếu ứng dụng không tải lại trang)
    $(window).on('popstate', function () {
        currentPage = window.location.href;
        sendPageToServer(currentPage); 
    });

    
    $(window).on('beforeunload', function () {
        if (hubConnection) {
            // Gửi thông báo ngắt kết nối (có thể sử dụng bất đồng bộ)
            userHub.invoke('NotifyDisconnected', hubConnection.id).catch(function (err) {
                console.error("Lỗi khi gửi thông báo ngắt kết nối: ", err);
            });

            hubConnection.stop().done(function () {
                console.log("Kết nối SignalR đã dừng.");
            }).fail(function (err) {
                console.error("Lỗi khi dừng kết nối SignalR: ", err);
            });
        }
    });
  

})