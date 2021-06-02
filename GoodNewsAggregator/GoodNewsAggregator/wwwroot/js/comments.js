let commentsDisplaySwitcherElement = document.getElementById('comments-display-switcher');
let isCommentsDisplayed = false;

function toggleComments(newsId) {
    if (commentsDisplaySwitcherElement != null) {
        if (isCommentsDisplayed == true) {
            commentsDisplaySwitcherElement.innerHTML = 'Display comments';
            document.getElementById('comments-container').innerHTML = '';
        } else {
            commentsDisplaySwitcherElement.innerHTML = 'Hide comments';
            let commentsContainer = document.getElementById('comments-container');
            loadComments(newsId, commentsContainer);

        }
        isCommentsDisplayed = !isCommentsDisplayed;
    }

    commentsDisplaySwitcherElement.addEventListener('onclose', function () {
        alert('closed');
    }, false);
}

function loadComments(newsId, commentsContainer) {
    let request = new XMLHttpRequest();
    //create request
    request.open('GET', `/Comments/List?newsId=${newsId}`, true);
    //create request handler
    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            let resp = request.responseText;
            commentsContainer.innerHTML = resp;

            document.getElementById('create-comment-btn')
                .addEventListener("click", createComment);
        }
    }
    //send request
    request.send();
}

function validateCommentData() {

}

function createComment() {

    let commentText = document.getElementById('commentText').value;
    let newsId = document.getElementById('newsId').value;

    validateCommentData();

    var postRequest = new XMLHttpRequest();
    postRequest.open("POST", '/Comments/Create', true);
    postRequest.setRequestHeader('Content-Type', 'application/json');

    //let requestData = new {
    //    commentText: commentText
    //}

    postRequest.send(JSON.stringify({
        commentText: commentText,
        newsId: newsId
    }));

    postRequest.onload = function () {
        if (postRequest.status >= 200 && postRequest.status < 400) {
            document.getElementById('commentText').value = '';

            //commentsContainer.innerHTML += '';

            let commentsContainer = document.getElementById('comments-container');
            loadComments(newsId, commentsContainer);
        }
    }
}

var getCommentsIntervalId = setInterval(function () {
    let url = window.location.pathname;
    let newsId = url.substring(url.lastIndexOf('/') + 1);
    let commentsContainer = document.getElementById('comments-container');

    loadComments(newsId, commentsContainer);
}, 15000);