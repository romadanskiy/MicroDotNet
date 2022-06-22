const mongoose = require('mongoose');

const CommentSchema = new mongoose.Schema({
    message: {
        type: String,
        required: true
    },
    userName: {
        type: String,
        required: true
    },
    datePosted: {
        type: String,
        required: true
    },
    questionId: {
        type: Number,
        required: true
    }
});

const Comment = mongoose.model('comment', CommentSchema);

module.exports = Comment;