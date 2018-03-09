/**
 * 定义User模型
 * @author 杨耿聪
 */

var mongoose = require('./ConnectedDB');

var userSchema = mongoose.Schema({
	userId: String,
	userName: String,
	password: String,
	phone: String,
	headSculpture: String,
	itemId: String,
	friendsId: String,
	score: String,
	selectedItemId: String,
	background: String
}); 

var userModel = mongoose.model('User', userSchema);

module.exports = userModel;