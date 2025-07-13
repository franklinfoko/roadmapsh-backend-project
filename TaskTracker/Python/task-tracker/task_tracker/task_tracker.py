from asyncio import tasks
import click
from commands import add_task, list_tasks, update_task, modify_status, delete_task

@click.group()
def cli():
    pass 

@cli.command()
@click.argument('description')
def add(description):
    add_task(description)

@cli.command()
@click.argument('status', required=False)
def list(status=None):
     tasks = list_tasks(status)
     click.echo(tasks)

@cli.command()
@click.argument('task_id')
@click.argument('description')
def update(task_id, description):
    result = update_task(task_id, description)
    click.echo(result)

@cli.command()
@click.argument('task_id')
def mark_in_progress(task_id):
    result = modify_status('in-progress', task_id)
    click.echo(result)

@cli.command()
@click.argument('task_id')
def mark_done(task_id):
    result = modify_status('done', task_id)
    click.echo(result)

@cli.command()
@click.argument('task_id')
def delete(task_id):
    result = delete_task(task_id)
    click.echo(result)

if __name__ == '__main__':
    cli()