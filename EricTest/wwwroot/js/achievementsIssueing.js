window.issueBadgeCredential = function() {
    const achievementId = document.getElementById('achievementId').value;
    const recipientEmail = document.getElementById('recipientEmail').value;
    const expires = document.getElementById('expireDate').value;

    if (!recipientEmail) {
        alert('Please enter recipient email');
        return;
    }

    const request = {
        recipientEmail: recipientEmail,
        achievementId: achievementId,
        expires: new Date(expires).toISOString()
    };

    fetch('/api/Assertion', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(request)
    })
        .then(response => {
            if (response.ok) {
                alert('Badge awarded successfully!');
                $('#awardModal').modal('hide');
                document.getElementById('awardForm').reset();
            } else {
                return response.json().then(err => { throw err; });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Error: ' + (error.error || 'Failed to issue badge'));
        });
}