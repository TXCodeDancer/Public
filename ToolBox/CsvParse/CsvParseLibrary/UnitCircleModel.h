#pragma once
#include <corecrt_math.h>
#include "IModel.h"
#include <vector>
#include <list>
#include <set>
#include <xutility>

const double PI = 3.141592653589793238462643;

enum class Columns { angle, radians, x, y };

class UnitCircleModel : public IModel
{
public:

    UnitCircleModel()
    {
        _angle = NAN;
        _radians = NAN;
        _x = NAN;
        _y = NAN;
    }

    ~UnitCircleModel() { }

    vector<vector<double>> GetIOData(string csvIn) override;

private:
    double _angle;
    double _radians;
    double _x;
    double _y;
    set<string> _inputs{ "Angle" };
    set<string> _outputs{ "Angle","Radians", "X", "Y" };
    list<UnitCircleModel> _models;

    void Read(string csvIn) override;
    void Write(string csvOut) override;
    void Compute() override;
    void UpdateModels(vector<vector<double>>& inputs, set<string> columnSet) override;

    double Angle() const { return _angle; }
    void Angle(double val) { _angle = val; }
    double Radians() const { return _radians; }
    void Radians(double val) { _radians = val; }
    double X() const { return _x; }
    void X(double val) { _x = val; }
    double Y() const { return _y; }
    void Y(double val) { _y = val; }
};
