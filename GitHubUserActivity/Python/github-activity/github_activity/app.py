"""
GitHub User Activity
"""

import requests

def fetch_activity(username):
    """
    Function to fetch user activity
    """
    url = f"https://api.github.com/users/{username}/events"
    response = requests.get(url, timeout=10)

    if response.status_code != 200:
        return f"Could not fetch data for {username}"

    events = response.json()
    if not events:
        return f"No recent public activity found for {username}"

    output = []

    for event in events[:5]:
        #date = event.get("created_at")
        event_type = event.get("type")
        #print(event_type)
        repo = event.get("repo", {}).get("name", "Unknow repo")
        payload = event.get("payload")

        match event_type:
            case "PushEvent":
                commit_count = len(payload.get("commits", []))
                output.append(
                    f"- Pushed {commit_count} commit{'s' if commit_count != 1 else ''} to {repo}"
                    )
            case "CreateEvent":
                ref_type = payload.get("ref_type", "item")
                output.append(f"- Created a new {ref_type} in {repo}")
            case "IssuesEvent":
                if payload.get("action") == "opened":
                    output.append(f"- Opened a new issue in {repo}")
            case "PullRequestEvent":
                action = payload.get("action", "acted on")
                output.append(f"- {action.capitalize()} a pull request in {repo}")
            case "ForkEvent":
                output.append(f"- Forked {repo}")

    return output
