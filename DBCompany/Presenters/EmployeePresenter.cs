using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBCompany.Models;
using DBCompany.Views;

namespace DBCompany.Presenters
{
    public class EmployeePresenter
    {
        public IEmployeeRepository repository;
        public IEmployeeView view;

        public EmployeePresenter(IEmployeeRepository employeeRepository, IEmployeeView employeeView)
        {
            view = employeeView;
            repository = employeeRepository;
            view.Binding.DataSource = employeeRepository.Table;

            view.AddEvent += Add;
            view.EditEvent += Edit;
            view.DeleteEvent += Delete;
            view.UpdateEvent += Update;
        }

 
        private void Update(object? sender, EventArgs e)
        {
            repository.UpdateRepository();
        }

        private void Delete(object? sender, EventArgs e)
        {
            if (repository.ExistId(view.Employee.Id)==false)
            {
                view.ShowMessageNotExistId();
                return;
            }
            repository.Delete(view.Employee.Id);
        }

        private void Edit(object? sender, EventArgs e)
        {
            if (repository.ExistId(view.Employee.Id)==false)
            {
                view.ShowMessageNotExistId();
                return;
            }

            var formSub = new FormInsert_Edit(FormInsert_Edit.ModeForm.Changing);
            formSub.Employee = view.Employee;
            formSub.ApplyEvent += (s,e) =>
                {
                    if (repository.CheckUniqueFullNameByOtherId(formSub.Employee) == false)
                    {
                        formSub.ShowMessageError("Сотрудник с такими ФИО и должностью уже существует.");
                        return;
                    }
                    if (repository.CheckUniqueLoginByOtherId(formSub.Employee) == false)
                    {
                        formSub.ShowMessageError("Сотрудник с таким логином уже существует.");
                        return;
                    }
                    repository.Edit(formSub.Employee);
                    formSub.Close();
                };
            formSub.ShowDialog();
        }

        private void Add(object? sender, EventArgs e)
        {
            var formSub = new FormInsert_Edit(FormInsert_Edit.ModeForm.Adding);

            formSub.ApplyEvent += (s,e)=>
                {
                    if(repository.CheckUniqueFullNameByOtherId(formSub.Employee)==false)
                    {
                        formSub.ShowMessageError("Сотрудник с такими ФИО и должностью уже существует.");
                        return;
                    }
                    if (repository.CheckUniqueLoginByOtherId(formSub.Employee) == false)
                    {
                        formSub.ShowMessageError("Сотрудник с таким логином уже существует.");
                        return;
                    }
                    repository.Add(formSub.Employee);
                    formSub.Close();
                };
            formSub.ShowDialog();
        }
    }
}
