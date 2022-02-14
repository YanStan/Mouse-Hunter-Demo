import select
import time

import win32file
from run_single import run_singlescreen_processing


def write_str_to_pipe(file_handle, pipe_str):
    test_data = bytes(pipe_str, 'utf-8')
    win32file.WriteFile(file_handle, test_data)


def read_str_from_pipe(file_handle):
    left, data = win32file.ReadFile(file_handle, 4096)
    return data.decode('utf-8')[1:]


if __name__ == '__main__':
    #Launching client for pipe connection
    file_descriptor = win32file.CreateFile(
        "\\\\.\\pipe\\MouseHunter",
        win32file.GENERIC_READ | win32file.GENERIC_WRITE,
        0,
        None,
        win32file.OPEN_EXISTING,
        0,
        None)
    while True:
        command = read_str_from_pipe(file_descriptor)
        command_splitted = command.split(':')
        file_args = command_splitted[0]
        python_args = command_splitted[1]

        file_args_arr = file_args.split()
        file_name = file_args_arr[0]
        pan_num = file_args_arr[1]

        if len(python_args) > 1:
            screen_args_array = python_args.split()
            #todo remove empty entries in args_array
            response = run_singlescreen_processing(file_name, pan_num, screen_args_array)
            write_str_to_pipe(file_descriptor, response)
        else:
            write_str_to_pipe(file_descriptor, run_singlescreen_processing(file_name, pan_num))
    time.sleep(0.2)

    #todo unreachable code, u can fix it!
    print('python work is finished!')
    file_descriptor.Close()
    # # создаем объект опроса
    # pollobj = select.poll()
    # # создаём пустой список дескрипторов
    # polled_descriptors = dict()
    #
    # # регистрируем Каждый наш файловый дескриптор, для отслеживания его объектом опроса pollobj
    # pollobj.register(file_descriptor, select.POLLIN)
    # # my_file_obj.fileno() даёт нам номер дескриптора
    # # записываем в словарь все файловые объекты, где ключ - это номера их #дескрипторов
    # polled_descriptors[file_descriptor.fileno()] = file_descriptor
    #
    # #pollobj.poll() Опрашивает набор зарегистрированных файловых дескрипторов и возвращает возможно пустой список,
    # # содержащий 2 кортежа для дескрипторов, которые имеют события или ошибки, о которых нужно сообщить.
    # # fd - это дескриптор файла, а событие - это битовая маска с битами,
    # # установленными для сообщаемых событий для этого дескриптора - для ожидания ввода, чтобы указать, что дескриптор может быть записан, и так далее.
    # for fd, evt in pollobj.poll():
    #     file_descriptor_current = polled_descriptors[fd]
    #     #process event


