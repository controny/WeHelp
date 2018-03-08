/**
 * 定义连接MongoDB数据库的模块
 * @author 杨耿聪
 */

var mongoose = require('mongoose');
mongoose.connect('mongodb://localhost/UWPproject');

var db = mongoose.connection;
db.on('error', function(error) {
	console.log(error);
});
db.once('open', function (callback) {
	console.log('connect to DataBase');
});

module.exports = mongoose;