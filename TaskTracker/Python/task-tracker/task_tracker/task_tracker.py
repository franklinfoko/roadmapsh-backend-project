"""
Configure task-cli
"""

#from asyncio import tasks
import click
from commands import add_task, list_tasks, update_task, modify_status, delete_task

@click.group()
def cli():
    """
    Define cli
    """

@cli.command()
@click.argument('description')
def add(description):
    """
    Add cli
    """
    add_task(description)

@cli.command()
@click.argument('status', required=False)
def list(status=None):
    """
    List cli
    """
    tasks = list_tasks(status)
    click.echo(tasks)

@cli.command()
@click.argument('task_id')
@click.argument('description')
def update(task_id, description):
    """
    Update cli
    """
    result = update_task(task_id, description)
    click.echo(result)

@cli.command()
@click.argument('task_id')
def mark_in_progress(task_id):
    """
    Mark in progress cli
    """
    result = modify_status('in-progress', task_id)
    click.echo(result)

@cli.command()
@click.argument('task_id')
def mark_done(task_id):
    """
    Mark done cli
    """
    result = modify_status('done', task_id)
    click.echo(result)

@cli.command()
@click.argument('task_id')
def delete(task_id):
    """
    delete cli
    """
    result = delete_task(task_id)
    click.echo(result)

if __name__ == '__main__':
    cli()
