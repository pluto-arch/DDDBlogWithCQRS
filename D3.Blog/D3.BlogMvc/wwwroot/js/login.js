
Vue.prototype.$http = axios;
var vm=new Vue({
    el: '#app',
    data: {
        message: 'hello',
        conn:Object
    },
    mounted: function () {
        this.$nextTick(function () {
            ///signalr
            //var connection = new signalR.HubConnectionBuilder().withUrl("/D3BlogHub").build();
            // connection.start().catch(function (err) {
            //    return console.error(err.toString());
            //});
            //this.conn=connection;
            //this.conn.on("ReceiveMessage", function (message) {
            //    alert(message)
            //});
        });
    },
    methods: {
        
    }
})